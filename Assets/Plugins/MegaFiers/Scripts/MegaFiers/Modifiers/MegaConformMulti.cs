using UnityEngine;
using System.Collections.Generic;

namespace MegaFiers
{
	[System.Serializable]
	public class MegaConformTarget
	{
		public GameObject	target;
		public bool			children = false;
	}

	[AddComponentMenu("Modifiers/Conform Multi")]
	public class MegaConformMulti : MegaModifier
	{
		public List<MegaConformTarget>	targets				= new List<MegaConformTarget>();
		public List<Collider>			conformColliders	= new List<Collider>();
		public float[]					offsets;
		public Bounds					bounds;
		public float[]					last;
		public Vector3[]				conformedVerts;
		[Adjust(0.0f, 1.0f)]
		public float					conformAmount		= 1.0f;
		[Adjust]
		public float					raystartoff			= 0.0f;
		[Adjust]
		public float					offset				= 0.0f;
		public float					raydist				= 100.0f;
		[Adjust]
		public MegaAxis					axis				= MegaAxis.Y;
		Matrix4x4						loctoworld;
		Matrix4x4						ctm;
		Matrix4x4						cinvtm;
		Ray								ray					= new Ray();
		RaycastHit						hit;

		public override string ModName()	{ return "Conform Multi"; }
		public override string GetHelpURL()	{ return "?page_id=4547"; }

		public void BuildColliderList()
		{
			conformColliders.Clear();

			for ( int i = 0; i < targets.Count; i++ )
			{
				if ( targets[i].target )
				{
					if ( targets[i].children )
					{
						Collider[] cols = (Collider[])targets[i].target.GetComponentsInChildren<Collider>();

						for ( int c = 0; c < cols.Length; c++ )
							conformColliders.Add(cols[c]);
					}
					else
					{
						Collider col = targets[i].target.GetComponent<Collider>();

						if ( col )
							conformColliders.Add(col);
					}
				}
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			return p;
		}

		bool DoRayCast(Ray ray, ref Vector3 pos, float raydist)
		{
			bool retval = false;
			float min = float.MaxValue;
		
			for ( int i = 0; i < conformColliders.Count; i++ )
			{
				if ( conformColliders[i].Raycast(ray, out hit, raydist) )
				{
					retval = true;
					if ( hit.distance < min )
					{
						min = hit.distance;
						pos = hit.point;
					}
				}
			}

			return retval;
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( conformColliders.Count > 0 )
			{
				int ax = (int)axis;

				Vector3 hitpos = Vector3.zero;

				float min = float.MaxValue; //mc.bbox.min[(int)axis];
				Unity.Collections.NativeArray<Vector3> verts;

				verts = mc.GetSourceJVerts();

				for ( int i = 0; i < verts.Length; i++ )
				{
					if ( verts[i][ax] < min )
						min = verts[i][ax];
				}

				for ( int i = 0; i < verts.Length; i++ )
					offsets[i] = verts[i][ax] - min;

				for ( int i = 0; i < jverts.Length; i++ )
				{
					Vector3 origin = ctm.MultiplyPoint(jverts[i]);
					origin.y += raystartoff;
					ray.origin = origin;
					ray.direction = Vector3.down;

					jsverts[i] = jverts[i];

					if ( DoRayCast(ray, ref hitpos, raydist) )
					{
						Vector3 lochit = cinvtm.MultiplyPoint(hitpos);

						float v = Mathf.Lerp(jverts[i][ax], lochit[ax] + offsets[i] + offset, conformAmount);

						Vector3 vt = jsverts[i];
						vt[ax] = v;
						jsverts[i] = vt;
						//sverts[i][ax] = Mathf.Lerp(verts[i][ax], lochit[ax] + offsets[i] + offset, conformAmount);
						last[i] = v;	//sverts[i][ax];
					}
					else
					{
						Vector3 ht = ray.origin;
						ht.y -= raydist;

						Vector3 vt = jsverts[i];
						vt[ax] = last[i];
						jsverts[i] = vt;
					}
				}
			}
			else
				jverts.CopyTo(jsverts);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( targets.Count > 0 )
			{
				if ( conformColliders.Count == 0 )
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

#if false
				if ( !mc.gizmoPrepare && mc.modIndex > 0 )
				{
					float min = float.MaxValue; //mc.bbox.min[(int)axis];
					int ax = (int)axis;
					Unity.Collections.NativeArray<Vector3> verts;

					if ( (mc.modIndex & 1) != 0 )
						verts = mc.mod.GetSourceJVerts();
					else
						verts = mc.mod.GetDestJVerts();

					for ( int i = 0; i < verts.Length; i++ )
					{
						if ( verts[i][ax] < min )
							min = verts[i][ax];
					}

					for ( int i = 0; i < verts.Length; i++ )
						offsets[i] = verts[i][ax] - min;
				}
#endif
				loctoworld = transform.localToWorldMatrix;

				ctm = loctoworld;
				cinvtm = transform.worldToLocalMatrix;
			}

			return true;
		}
	}
}