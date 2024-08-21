using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

#if false
namespace MegaFiers
{
	[AddComponentMenu("Modifiers/DisplaceRT")]
	public class MegaDisplaceRT : MegaModifier
	{
		public RenderTexture		rtmap;
		public float				amount		= 0.0f;
		public Vector2				offset		= Vector2.zero;
		public float				vertical	= 0.0f;
		public Vector2				scale		= Vector2.one;
		public MegaChannel			channel		= MegaChannel.Red;
		public bool					CentLum		= true;
		public float				CentVal		= 0.5f;
		public float				Decay		= 0.0f;
		[HideInInspector]
		public Vector2[]			uvs;
		[HideInInspector]
		public Vector3[]			normals;
		Texture2D					map;
		Job							job;
		JobHandle					jobHandle;

		public override string		ModName()		{ return "DisplaceRT"; }
		public override string		GetHelpURL()	{ return "?page_id=168"; }

		public override MegaModChannel ChannelsReq() { return MegaModChannel.Verts | MegaModChannel.UV; }
		public override MegaModChannel ChannelsChanged() { return MegaModChannel.Verts; }

		[ContextMenu("Init")]
		public virtual void Init()
		{
			MegaModifyObject mod = (MegaModifyObject)GetComponent<MegaModifyObject>();
			uvs = mod.cachedMesh.uv;
			normals = mod.cachedMesh.normals;
		}

		public override void MeshChanged()
		{
			Init();
		}

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				Decay;
			public bool					CentLum;
			public float				CentVal;
			public float				vertical;
			public int					channel;
			public float				amount;
			public Vector2				scale;
			public Vector2				offset;
			public int					width;
			public int					height;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>	normals;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector2>	uvs;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			[Unity.Collections.ReadOnly]
			public NativeArray<Color32>	texture;

			public static float repeat(float t, float length)
			{
				if ( t > 0 )
					return t % length;
				else
					return length - (math.abs(t) % length);
			}

			public static int repeat(int t, int length)
			{
				length -= 1;
				if ( t > 0 )
					return t % length;
				else
					return length - (math.abs(t) % length);
			}

			public Color32 GetPixel(int x, int y)
			{
				return texture[x + y * width];
			}

			public Color32 GetPixelBilinear(float x, float y)
			{
				x = repeat(x, 1.0f);
				y = repeat(y, 1.0f);

				int xMin = repeat((int)(x * width), width);
				int yMin = repeat((int)(y * height), height);
				int xMax = repeat((int)((x + 1) * width), width);
				int yMax = repeat((int)((y + 1) * height), height);

				Color32 bottomLeft	= GetPixel(xMin, yMin);
				Color32 bottomRight	= GetPixel(xMax, yMin);
				Color32 topLeft		= GetPixel(xMin, yMax);
				Color32 topRight	= GetPixel(xMax, yMax);

				float xt = (x * width) - xMin;
				float yt = (y * height) - yMin;

				Color32 leftColor = Color32.Lerp(bottomLeft, topLeft, yt);
				Color32 rightColor = Color32.Lerp(bottomRight, topRight, yt);

				return Color32.Lerp(leftColor, rightColor, xt);
			}

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				Vector2 uv = Vector2.Scale(uvs[vi] + offset, scale);
				Color col = GetPixelBilinear(uv.x, uv.y);

				float str = amount;

				if ( Decay != 0.0f )
					str *= math.exp(-Decay * math.length(p));

				if ( CentLum )
					str *= (col[channel] + CentVal);
				else
					str *= (col[channel]);

				float of = col[(int)channel] * str;
				p.x += (normals[vi].x * of) + (normals[vi].x * vertical);
				p.y += (normals[vi].y * of) + (normals[vi].y * vertical);
				p.z += (normals[vi].z * of) + (normals[vi].z * vertical);

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null && map != null )
			{
				job.Decay		= Decay;
				job.CentLum		= CentLum;
				job.CentVal		= CentVal;
				job.vertical	= vertical;
				job.channel		= (int)channel;
				job.amount		= amount;
				job.scale		= scale;
				job.offset		= offset;
				job.width		= map.width;
				job.height		= map.height;
				job.normals		= mc.normals;
				job.uvs			= new NativeArray<Vector2>(mc.Uvs, Allocator.TempJob);
				job.texture		= new NativeArray<Color32>(map.GetPixels32(), Allocator.TempJob);
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();

				job.uvs.Dispose();
				job.texture.Dispose();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			if ( i >= 0 )
			{
				Vector2 uv = Vector2.Scale(uvs[i] + offset, scale);
				Color col = map.GetPixelBilinear(uv.x, uv.y);

				float str = amount;

				if ( Decay != 0.0f )
					str *= (float)Mathf.Exp(-Decay * p.magnitude);

				if ( CentLum )
					str *= (col[(int)channel] + CentVal);
				else
					str *= (col[(int)channel]);

				float of = col[(int)channel] * str;
				p.x += (normals[i].x * of) + (normals[i].x * vertical);
				p.y += (normals[i].y * of) + (normals[i].y * vertical);
				p.z += (normals[i].z * of) + (normals[i].z * vertical);
			}

			return invtm.MultiplyPoint3x4(p);
		}
#if false
		public override void Modify(MegaModifyObject mc)
		{
			for ( int i = 0; i < verts.Length; i++ )
				sverts[i] = Map(i, verts[i]);
		}
#endif
		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;	//Prepare(mc);
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( rtmap == null )
				return false;

			if ( map == null || rtmap.width != map.width || rtmap.height != map.height )
				map = new Texture2D(rtmap.width, rtmap.height);

			if ( uvs == null || uvs.Length == 0 )
				uvs = mc.mod.mesh.uv;

			if ( normals == null || normals.Length == 0 )
			{
				MegaModifyObject mobj = (MegaModifyObject)GetComponent<MegaModifyObject>();
				if ( mobj )
					normals = mobj.cachedMesh.normals;
				else
					normals = mc.mod.mesh.normals;
			}

			if ( uvs.Length == 0 )
				return false;

			if ( normals.Length == 0 )
				return false;

			if ( map == null )
				return false;

			RenderTexture.active = rtmap;

			map.ReadPixels(new Rect(0, 0, rtmap.width, rtmap.height), 0, 0);
			return true;
		}
	}
}
#endif