using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Conform")]
	public class MegaConformMod : MegaModifier
	{
		// Will have multiple in the end or layer
		public GameObject	target;
		public float[]		offsets;
		public Collider		conformCollider;
		public Bounds		bounds;
		public float[]		last;
		public Vector3[]	last1;
		public Vector3[]	conformedVerts;
		[Adjust(0.0f, 1.0f)]
		public float		conformAmount	= 1.0f;
		[Adjust]
		public float		raystartoff		= 0.0f;
		[Adjust]
		public float		offset			= 0.0f;
		public float		raydist			= 100.0f;
		[Adjust]
		public MegaAxis		axis			= MegaAxis.Y;
		Matrix4x4			loctoworld;
		public bool			useLocalDown	= false;
		public bool			flipDown		= true;
		[Adjust]
		public MegaAxis		downAxis		= MegaAxis.Y;
		Matrix4x4			ctm;
		Matrix4x4			cinvtm;

		public override string ModName()	{ return "Conform"; }
		public override string GetHelpURL() { return "?page_id=4547"; }

// Set this to false to use non jobbed version
#if true
		ConformFillRayJob		rayFillJob;
		ConformRayResultsJob	rayResultsJob;
		JobHandle				fillHandle;
		JobHandle				rayHandle;
		JobHandle				conformHandle;

		// Jobs versions
		[BurstCompile]
		// Fills the commands array and gets the min value
		struct ConformFillRayJob : IJobParallelFor
		{
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>			jvertices;
			public NativeArray<RaycastHit>		results;
			public NativeArray<RaycastCommand>	commands;
			public Matrix4x4					ctm;
			public Vector3						down;
			public int							ax;
			public Vector3						rso;
			public float						raydist;

			public void Execute(int i)
			{
				RaycastCommand com = commands[i];

				com.direction	= down;
				com.from		= ctm.MultiplyPoint(jvertices[i]) + rso;
				com.distance	= raydist;
#if UNITY_2022_2_OR_NEWER
				com.queryParameters.layerMask = 65535;
				com.queryParameters.hitBackfaces = false;
				com.queryParameters.hitMultipleFaces = false;
#else
				com.layerMask = 65535;
				com.maxHits = 1;
#endif
				commands[i]		= com;
			}
		}

		[BurstCompile]
		// Gets the ray cast results
		struct ConformRayResultsJob : IJobParallelFor
		{
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>			jverts;
			public NativeArray<Vector3>			jsverts;
			//public NativeArray<Vector3>			last1;
			public NativeArray<RaycastHit>		results;
			public NativeArray<bool>			hits;
			public NativeArray<Vector3>			points;
			public Matrix4x4					cinvtm;
			public float						min;
			public int							ax;
			public float3						ldir;
			public float						offset;
			public float						conformAmount;

			public void Execute(int i)
			{
				// we hit something
				float3 lochit = cinvtm.MultiplyPoint(results[i].point);

				float off = jverts[i][ax] - min;

				jsverts[i] = math.lerp(jverts[i], lochit + (ldir * (off + offset)), conformAmount);
			}
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( target )
			{
				if ( conformCollider != target.GetComponent<Collider>() )
					conformCollider = target.GetComponent<Collider>();

				if ( conformCollider == null )
					return false;

				if ( !rayFillJob.results.IsCreated || rayFillJob.results.Length != mc.mod.jverts.Length )
				{
					if ( rayFillJob.results.IsCreated )
						rayFillJob.results.Dispose();

					rayFillJob.results = new NativeArray<RaycastHit>(mc.mod.jverts.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				}

				if ( !rayFillJob.commands.IsCreated || rayFillJob.commands.Length != mc.mod.jverts.Length )
				{
					if ( rayFillJob.commands.IsCreated )
						rayFillJob.commands.Dispose();

					rayFillJob.commands = new NativeArray<RaycastCommand>(mc.mod.jverts.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				}

				if ( !rayResultsJob.hits.IsCreated || rayResultsJob.hits.Length != mc.mod.jverts.Length )
				{
					if ( rayResultsJob.hits.IsCreated )
						rayResultsJob.hits.Dispose();

					rayResultsJob.hits = new NativeArray<bool>(mc.mod.jverts.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				}

				if ( !rayResultsJob.points.IsCreated || rayResultsJob.points.Length != mc.mod.jverts.Length )
				{
					if ( rayResultsJob.points.IsCreated )
						rayResultsJob.points.Dispose();

					rayResultsJob.points = new NativeArray<Vector3>(mc.mod.jverts.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				}

				//if ( useLocalDown && (last1 == null || last1.Length != last.Length) )
				//	last1 = new Vector3[last.Length];

				loctoworld = transform.localToWorldMatrix;

				ctm = loctoworld;
				cinvtm = transform.worldToLocalMatrix;

				return true;
			}
			else
				conformCollider = null;

			return true;
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null && conformCollider )
			{
				rayFillJob.jvertices = jverts;
				rayFillJob.ctm = transform.localToWorldMatrix;
				Vector3 down = Vector3.down;
				switch ( downAxis )
				{
					case MegaAxis.X: down = transform.right; break;
					case MegaAxis.Y: down = transform.up; break;
					case MegaAxis.Z: down = transform.forward; break;
				}

				if ( flipDown )
					down = -down;
				rayFillJob.down = down;

				int ax = (int)axis;
				rayFillJob.ax = (int)axis;
				rayFillJob.rso = -down * raystartoff;
				rayFillJob.raydist = raydist;

				fillHandle = rayFillJob.Schedule(jverts.Length, mc.batchCount);

				float min = float.MaxValue;
				for ( int i = 0; i < jverts.Length; i++ )
				{
					if ( jverts[i][ax] < min )
						min = jverts[i][ax];
				}

				fillHandle.Complete();

				rayHandle = RaycastCommand.ScheduleBatch(rayFillJob.commands, rayFillJob.results, 1, default(JobHandle));
				rayHandle.Complete();

				//rayResultsJob.last1			= last1;
				rayResultsJob.results = rayFillJob.results;
				rayResultsJob.jverts = jverts;
				rayResultsJob.jsverts = jsverts;
				rayResultsJob.conformAmount = conformAmount;
				rayResultsJob.ax = (int)axis;
				rayResultsJob.offset = offset;
				rayResultsJob.cinvtm = transform.worldToLocalMatrix;
				rayResultsJob.min = min;	//rayFillJob.min;
				rayResultsJob.ldir = -transform.InverseTransformDirection(down);

				conformHandle = rayResultsJob.Schedule(jverts.Length, mc.batchCount);
				conformHandle.Complete();
			}
		}

		public override void Dispose()
		{
			if ( rayFillJob.commands.IsCreated )
				rayFillJob.commands.Dispose();

			if ( rayFillJob.results.IsCreated )
				rayFillJob.results.Dispose();

			if ( rayResultsJob.hits.IsCreated )
				rayResultsJob.hits.Dispose();

			if ( rayResultsJob.points.IsCreated )
				rayResultsJob.points.Dispose();
		}
#else
		Ray					ray				= new Ray();
		RaycastHit			hit;

		public override void Modify(MegaModifyObject mc)
		{
			if ( conformCollider )
			{
				float min = float.MaxValue;
				int ax = (int)axis;
				NativeArray<Vector3> verts;

				verts = mc.GetSourceJVerts();

				for ( int i = 0; i < verts.Length; i++ )
				{
					if ( verts[i][ax] < min )
						min = verts[i][ax];
				}

				for ( int i = 0; i < verts.Length; i++ )
					offsets[i] = verts[i][ax] - min;

				if ( useLocalDown )
				{
					Vector3 down = Vector3.down;
					switch ( downAxis )
					{
						case MegaAxis.X: down = transform.right; break;
						case MegaAxis.Y: down = transform.up; break;
						case MegaAxis.Z: down = transform.forward; break;
					}

					if ( flipDown )
						down = -down;

					ray.direction = down;

					Vector3 rso = -down * raystartoff;

					Vector3 dir = ray.direction;
					Vector3 ldir = -transform.InverseTransformDirection(dir);

					for ( int i = 0; i < jverts.Length; i++ )
					{
						Vector3 origin = ctm.MultiplyPoint(jverts[i]) - rso;
						ray.origin = origin;

						jsverts[i] = jverts[i];

						if ( conformCollider.Raycast(ray, out hit, raydist) )
						{
							Vector3 lochit = cinvtm.MultiplyPoint(hit.point);

							jsverts[i] = Vector3.Lerp(jverts[i], lochit + (ldir * (offsets[i] + offset)), conformAmount);
							last1[i] = jsverts[i];
						}
						else
							jsverts[i] = last1[i];
					}
				}
				else
				{
					for ( int i = 0; i < jverts.Length; i++ )
					{
						Vector3 origin = ctm.MultiplyPoint(jverts[i]);
						origin.y += raystartoff;
						ray.origin = origin;
						ray.direction = Vector3.down;

						jsverts[i] = jverts[i];

						if ( conformCollider.Raycast(ray, out hit, raydist) )
						{
							Vector3 lochit = cinvtm.MultiplyPoint(hit.point);

							float v = Mathf.Lerp(jverts[i][ax], lochit[ax] + offsets[i] + offset, conformAmount);
							Vector3 vt = jsverts[i];
							vt[ax] = v;
							jsverts[i] = vt;    //[ax] = Mathf.Lerp(jverts[i][ax], lochit[ax] + offsets[i] + offset, conformAmount);
							last[i] = jsverts[i][ax];
						}
						else
						{
							Vector3 vt = jsverts[i];
							vt[ax] = last[i];
							jsverts[i] = vt;
						}
					}
				}
			}
			else
				jverts.CopyTo(jsverts);
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( target )
			{
				if ( conformCollider != target.GetComponent<Collider>() )
					conformCollider = target.GetComponent<Collider>();

				if ( conformCollider == null )
					return false;

				if ( conformedVerts == null || conformedVerts.Length != mc.mod.jverts.Length )
				{
					conformedVerts = new Vector3[mc.mod.jverts.Length];
					// Need to run through all the source meshes and find the vertical offset from the base

					offsets = new float[mc.mod.jverts.Length];
					last = new float[mc.mod.jverts.Length];

					for ( int i = 0; i < mc.mod.jverts.Length; i++ )
						offsets[i] = mc.mod.jverts[i][(int)axis] - mc.bbox.min[(int)axis];
				}

				if ( useLocalDown && (last1 == null || last1.Length != last.Length) )
					last1 = new Vector3[last.Length];

				loctoworld = transform.localToWorldMatrix;

				ctm = loctoworld;
				cinvtm = transform.worldToLocalMatrix;

				return true;
			}
			else
				conformCollider = null;

			return true;
		}
#endif

		public void SetTarget(GameObject targ)
		{
			target = targ;

			if ( target )
				conformCollider = target.GetComponent<Collider>();
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			return p;
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public void ChangeAxis()
		{
			MegaModifyObject mod = GetComponent<MegaModifyObject>();

			if ( mod )
			{
				for ( int i = 0; i < mod.jverts.Length; i++ )
					offsets[i] = mod.jverts[i][(int)axis] - mod.bbox.min[(int)axis];
			}
		}
	}
}