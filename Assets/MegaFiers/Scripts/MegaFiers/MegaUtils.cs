using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;

namespace MegaFiers
{
	public class MegaPlane
	{
		// Utils for plane stuff
		static public Vector4 Plane(Vector3 v1, Vector3 v2, Vector3 v3)
		{
			Vector3 normal = Vector4.zero;
			normal.x = (v2.y - v1.y) * (v3.z - v1.z) - (v2.z - v1.z) * (v3.y - v1.y);
			normal.y = (v2.z - v1.z) * (v3.x - v1.x) - (v2.x - v1.x) * (v3.z - v1.z);
			normal.z = (v2.x - v1.x) * (v3.y - v1.y) - (v2.y - v1.y) * (v3.x - v1.x);

			normal = normal.normalized;
			return new Vector4(normal.x, normal.y, normal.z, -Vector3.Dot(v2, normal));
		}

		static public float PlaneDist(Vector3 p, Vector4 plane)
		{
			Vector3 n = plane;
			return Vector3.Dot(n, p) + plane.w;
		}

		static public float GetDistance(Vector3 p, Vector3 p0, Vector3 p1, Vector3 p2)
		{
			return MegaNearestPointTest.DistPoint3Triangle3Dbl(p, p0, p1, p2);
		}

		static public float GetPlaneDistance(Vector3 p, Vector3 p0, Vector3 p1, Vector3 p2)
		{
			Vector4 pl = Plane(p0, p1, p2);
			return PlaneDist(p, pl);
		}

		static public Vector3 FaceNormal(Vector3 p0, Vector3 p1, Vector3 p2)
		{
			float e11x = p1.x - p0.x;
			float e11y = p1.y - p0.y;
			float e11z = p1.z - p0.z;

			float e22x = p2.x - p0.x;
			float e22y = p2.y - p0.y;
			float e22z = p2.z - p0.z;

			return new Vector3(e11y * e22z - e22y * e11z, -(e11x * e22z - e22x * e11z), e11x * e22y - e22x * e11y);
		}

		static public Vector3 MyBary(Vector3 p, Vector3 p0, Vector3 p1, Vector3 p2)
		{
			Vector3 bary = Vector3.zero;
			Vector3 normal = MegaPlane.FaceNormal(p0, p1, p2);

			float areaABC = Vector3.Dot(normal, Vector3.Cross((p1 - p0), (p2 - p0)));
			float areaPBC = Vector3.Dot(normal, Vector3.Cross((p1 - p), (p2 - p)));
			float areaPCA = Vector3.Dot(normal, Vector3.Cross((p2 - p), (p0 - p)));

			bary.x = areaPBC / areaABC; // alpha
			bary.y = areaPCA / areaABC; // beta
			bary.z = 1.0f - bary.x - bary.y; // gamma
			return bary;
		}

		static public Vector3 CalcBary(Vector3 p, Vector3 p0, Vector3 p1, Vector3 p2)
		{
			return MyBary(p, p0, p1, p2);
		}

		static public Vector3 GetCoordMine(Vector3 A, Vector3 B, Vector3 C, Vector3 bary)
		{
			return new Vector3((bary.x * A.x) + (bary.y * B.x) + (bary.z * C.x), (bary.x * A.y) + (bary.y * B.y) + (bary.z * C.y), (bary.x * A.z) + (bary.y * B.z) + (bary.z * C.z));
		}
	}

	public class MegaModBut
	{
		public MegaModBut() { }
		public MegaModBut(string _but, string tooltip, System.Type _classname, Color _col)
		{
			name = _but;
			color = _col;
			classname = _classname;
			content = new GUIContent(_but, tooltip);
		}
		public string		name;
		public Color		color;
		public System.Type	classname;
		public GUIContent	content;
	}

	public enum MegaAxis
	{
		X = 0,
		Y = 1,
		Z = 2,
	};

	public enum MegaRepeatMode
	{
		Loop,
		Clamp,
		PingPong,
	};

	public class MegaUtils
	{
		static public Bounds AddBounds(Transform o1, Bounds box, Transform o2, Bounds add)
		{
			Vector3[] corners = new Vector3[8];

			corners[0] = new Vector3(add.min.x, add.min.y, add.min.z);
			corners[1] = new Vector3(add.min.x, add.max.y, add.min.z);
			corners[2] = new Vector3(add.max.x, add.max.y, add.min.z);
			corners[3] = new Vector3(add.max.x, add.min.y, add.min.z);

			corners[4] = new Vector3(add.min.x, add.min.y, add.max.z);
			corners[5] = new Vector3(add.min.x, add.max.y, add.max.z);
			corners[6] = new Vector3(add.max.x, add.max.y, add.max.z);
			corners[7] = new Vector3(add.max.x, add.min.y, add.max.z);

			for ( int i = 0; i < 8; i++ )
				box.Encapsulate(o1.InverseTransformPoint(o2.TransformPoint(corners[i])));

			return box;
		}

		static public void Bez3D(out Vector3 b, ref Vector3[] p, float u)
		{
			Vector3 t01 = p[0];

			t01.x += (p[1].x - p[0].x) * u;
			t01.y += (p[1].y - p[0].y) * u;
			t01.z += (p[1].z - p[0].z) * u;

			Vector3 t12 = p[1];

			t12.x += (p[2].x - p[1].x) * u;
			t12.y += (p[2].y - p[1].y) * u;
			t12.z += (p[2].z - p[1].z) * u;

			Vector3 t02 = t01 + (t12 - t01) * u;

			t01.x = p[2].x + (p[3].x - p[2].x) * u;
			t01.y = p[2].y + (p[3].y - p[2].y) * u;
			t01.z = p[2].z + (p[3].z - p[2].z) * u;

			t01.x = t12.x + (t01.x - t12.x) * u;
			t01.y = t12.y + (t01.y - t12.y) * u;
			t01.z = t12.z + (t01.z - t12.z) * u;

			b.x = t02.x + (t01.x - t02.x) * u;
			b.y = t02.y + (t01.y - t02.y) * u;
			b.z = t02.z + (t01.z - t02.z) * u;
		}

		static public float WaveFunc(float radius, float t, float amp, float waveLen, float phase, float decay)
		{
			if ( waveLen == 0.0f )
				waveLen = 0.0000001f;

			float ang = Mathf.PI * 2.0f * (radius / waveLen + phase);
			return amp * Mathf.Sin(ang) * Mathf.Exp(-decay * Mathf.Abs(radius));
		}

		static public int LargestComponent(Vector3 p)
		{
			if ( p.x > p.y )
				return (p.x > p.z) ? 0 : 2;
			else
				return (p.y > p.z) ? 1 : 2;
		}

		static public float LargestValue(Vector3 p)
		{
			if ( p.x > p.y )
				return (p.x > p.z) ? p.x : p.z;
			else
				return (p.y > p.z) ? p.y : p.z;
		}

		static public float LargestValue1(Vector3 p)
		{
			if ( Mathf.Abs(p.x) > Mathf.Abs(p.y) )
				return (Mathf.Abs(p.x) > Mathf.Abs(p.z)) ? p.x : p.z;
			else
				return (Mathf.Abs(p.y) > Mathf.Abs(p.z)) ? p.y : p.z;
		}

		static public int SmallestComponent(Vector3 p)
		{
			if ( p.x < p.y )
				return (p.x < p.z) ? 0 : 2;
			else
				return (p.y < p.z) ? 1 : 2;
		}

		static public float SmallestValue(Vector3 p)
		{
			if ( p.x < p.y )
				return (p.x < p.z) ? p.x : p.z;
			else
				return (p.y < p.z) ? p.y : p.z;
		}

		static public float SmallestLargestValueAbs(Vector3 p)
		{
			if ( Mathf.Abs(p.x) < Mathf.Abs(p.y) )
				return (Mathf.Abs(p.x) < Mathf.Abs(p.z)) ? p.x : p.z;
			else
				return (Mathf.Abs(p.y) < Mathf.Abs(p.z)) ? p.y : p.z;
		}

		static public Vector3 Extents(Vector3[] verts, out Vector3 min, out Vector3 max)
		{
			Vector3 extent = Vector3.zero;

			min = Vector3.zero;
			max = Vector3.zero;

			if ( verts != null && verts.Length > 0 )
			{
				min = verts[0];
				max = verts[0];

				for ( int i = 1; i < verts.Length; i++ )
				{
					if ( verts[i].x < min.x ) min.x = verts[i].x;
					if ( verts[i].y < min.y ) min.y = verts[i].y;
					if ( verts[i].z < min.z ) min.z = verts[i].z;

					if ( verts[i].x > max.x ) max.x = verts[i].x;
					if ( verts[i].y > max.y ) max.y = verts[i].y;
					if ( verts[i].z > max.z ) max.z = verts[i].z;
				}

				extent = max - min;
			}

			return extent;
		}

		static public Vector3 Extents(List<Vector3> verts, out Vector3 min, out Vector3 max)
		{
			Vector3 extent = Vector3.zero;

			min = Vector3.zero;
			max = Vector3.zero;

			if ( verts != null && verts.Count > 0 )
			{
				min = verts[0];
				max = verts[0];

				for ( int i = 1; i < verts.Count; i++ )
				{
					if ( verts[i].x < min.x ) min.x = verts[i].x;
					if ( verts[i].y < min.y ) min.y = verts[i].y;
					if ( verts[i].z < min.z ) min.z = verts[i].z;

					if ( verts[i].x > max.x ) max.x = verts[i].x;
					if ( verts[i].y > max.y ) max.y = verts[i].y;
					if ( verts[i].z > max.z ) max.z = verts[i].z;
				}

				extent = max - min;
			}

			return extent;
		}

		static public Vector3 Extents(NativeArray<Vector3> verts, out Vector3 min, out Vector3 max)
		{
			Vector3 extent = Vector3.zero;

			min = Vector3.zero;
			max = Vector3.zero;

			if ( verts != null && verts.Length > 0 )
			{
				min = verts[0];
				max = verts[0];

				for ( int i = 1; i < verts.Length; i++ )
				{
					if ( verts[i].x < min.x ) min.x = verts[i].x;
					if ( verts[i].y < min.y ) min.y = verts[i].y;
					if ( verts[i].z < min.z ) min.z = verts[i].z;

					if ( verts[i].x > max.x ) max.x = verts[i].x;
					if ( verts[i].y > max.y ) max.y = verts[i].y;
					if ( verts[i].z > max.z ) max.z = verts[i].z;
				}

				extent = max - min;
			}

			return extent;
		}

		static public int FindVert(Vector3 vert, List<Vector3> verts, float tolerance, float scl, bool flipyz, bool negx, int vn)
		{
			int find = 0;

			if ( negx )
				vert.x = -vert.x;

			if ( flipyz )
			{
				float z = vert.z;
				vert.z = vert.y;
				vert.y = z;
			}

			vert /= scl;

			float closest = Vector3.SqrMagnitude(verts[0] - vert);

			for ( int i = 0; i < verts.Count; i++ )
			{
				float dif = Vector3.SqrMagnitude(verts[i] - vert);

				if ( dif < closest )
				{
					closest = dif;
					find = i;
				}
			}

			if ( closest > tolerance )
				return -1;

			return find;
		}

		static public void BuildTangents(Mesh mesh)
		{
			int triangleCount = mesh.triangles.Length;
			int vertexCount = mesh.vertices.Length;

			Vector3[] tan1 = new Vector3[vertexCount];
			Vector3[] tan2 = new Vector3[vertexCount];
			Vector4[] tangents = new Vector4[vertexCount];

			Vector3[] verts	= mesh.vertices;
			Vector2[] uvs		= mesh.uv;
			Vector3[] norms	= mesh.normals;
			int[]			tris	= mesh.triangles;

			for ( int a = 0; a < triangleCount; a += 3 )
			{
				long i1 = tris[a];
				long i2 = tris[a + 1];
				long i3 = tris[a + 2];

				Vector3 v1 = verts[i1];
				Vector3 v2 = verts[i2];
				Vector3 v3 = verts[i3];

				Vector2 w1 = uvs[i1];
				Vector2 w2 = uvs[i2];
				Vector2 w3 = uvs[i3];

				float x1 = v2.x - v1.x;
				float x2 = v3.x - v1.x;
				float y1 = v2.y - v1.y;
				float y2 = v3.y - v1.y;
				float z1 = v2.z - v1.z;
				float z2 = v3.z - v1.z;

				float s1 = w2.x - w1.x;
				float s2 = w3.x - w1.x;
				float t1 = w2.y - w1.y;
				float t2 = w3.y - w1.y;

				float r = 1.0f / (s1 * t2 - s2 * t1);

				Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
				Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

				tan1[i1] += sdir;
				tan1[i2] += sdir;
				tan1[i3] += sdir;

				tan2[i1] += tdir;
				tan2[i2] += tdir;
				tan2[i3] += tdir;
			}

			for ( int a = 0; a < vertexCount; a++ )
			{
				Vector3 n = norms[a];
				Vector3 t = tan1[a];

				Vector3.OrthoNormalize(ref n, ref t);
				tangents[a].x = t.x;
				tangents[a].y = t.y;
				tangents[a].z = t.z;
				tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
			}

			mesh.tangents = tangents;
		}

		static public Mesh GetMesh(GameObject go)
		{
			if ( !Application.isPlaying )
				return GetSharedMesh(go);

			MeshFilter meshFilter = (MeshFilter)go.GetComponent(typeof(MeshFilter));

			if ( meshFilter != null )
				return meshFilter.mesh;
			else
			{
				SkinnedMeshRenderer smesh = (SkinnedMeshRenderer)go.GetComponent(typeof(SkinnedMeshRenderer));

				if ( smesh != null )
					return smesh.sharedMesh;
			}

			return null;
		}

		static public Mesh GetSharedMesh(GameObject go)
		{
			MeshFilter meshFilter = (MeshFilter)go.GetComponent(typeof(MeshFilter));

			if ( meshFilter != null )
				return meshFilter.sharedMesh;
			else
			{
				SkinnedMeshRenderer smesh = (SkinnedMeshRenderer)go.GetComponent(typeof(SkinnedMeshRenderer));

				if ( smesh != null )
					return smesh.sharedMesh;
			}

			return null;
		}

		static public void SetMesh(GameObject go, Mesh mesh)
		{
			if ( go )
			{
				Transform[] trans = (Transform[])go.GetComponentsInChildren<Transform>(true);

				for ( int i = 0; i < trans.Length; i++ )
				{
					MeshFilter mf = (MeshFilter)trans[i].GetComponent<MeshFilter>();

					if ( mf )
					{
						mf.sharedMesh = mesh;
						return;
					}

					SkinnedMeshRenderer skin = (SkinnedMeshRenderer)trans[i].GetComponent<SkinnedMeshRenderer>();
					if ( skin )
					{
						skin.sharedMesh = mesh;
						return;
					}
				}
			}
		}

		static public Mesh FindMesh(GameObject go, out GameObject obj)
		{
			if ( go )
			{
				Transform[] trans = (Transform[])go.GetComponentsInChildren<Transform>(true);

				for ( int i = 0; i < trans.Length; i++ )
				{
					MeshFilter mf = (MeshFilter)trans[i].GetComponent<MeshFilter>();

					if ( mf )
					{
						if ( mf.gameObject != go )
							obj = mf.gameObject;
						else
							obj = null;

						return mf.sharedMesh;
					}

					SkinnedMeshRenderer skin = (SkinnedMeshRenderer)trans[i].GetComponent<SkinnedMeshRenderer>();
					if ( skin )
					{
						if ( skin.gameObject != go )
							obj = skin.gameObject;
						else
							obj = null;

						return skin.sharedMesh;
					}
				}
			}

			obj = null;
			return null;
		}

		static public Mesh FindMesh(GameObject go)
		{
			if ( go )
			{
				Transform[] trans = (Transform[])go.GetComponentsInChildren<Transform>(true);

				for ( int i = 0; i < trans.Length; i++ )
				{
					MeshFilter mf = (MeshFilter)trans[i].GetComponent<MeshFilter>();

					if ( mf )
						return mf.sharedMesh;

					SkinnedMeshRenderer skin = (SkinnedMeshRenderer)trans[i].GetComponent<SkinnedMeshRenderer>();
					if ( skin )
						return skin.sharedMesh;
				}
			}

			return null;
		}

		static public bool SetMeshNew(GameObject go, Mesh m)
		{
			if ( go )
			{
				Transform[] trans = (Transform[])go.GetComponentsInChildren<Transform>(true);

				for ( int i = 0; i < trans.Length; i++ )
				{
					MeshFilter mf = (MeshFilter)trans[i].GetComponent<MeshFilter>();

					if ( mf )
					{
						mf.sharedMesh = m;
						return true;
					}

					SkinnedMeshRenderer skin = (SkinnedMeshRenderer)trans[i].GetComponent<SkinnedMeshRenderer>();
					if ( skin )
					{
						skin.sharedMesh = m;
						return true;
					}
				}
			}

			return false;
		}

		static public Sprite FindSprite(GameObject go)
		{
			if ( go )
			{
				Transform[] trans = (Transform[])go.GetComponentsInChildren<Transform>(true);

				for ( int i = 0; i < trans.Length; i++ )
				{
					SpriteRenderer sr = (SpriteRenderer)trans[i].GetComponent<SpriteRenderer>();

					if ( sr )
						return sr.sprite;
				}
			}

			return null;
		}

		// We dont need this, sprite doesnt change
		static public bool SetSpriteNew(GameObject go, Sprite s)
		{
			if ( go )
			{
				Transform[] trans = (Transform[])go.GetComponentsInChildren<Transform>(true);

				for ( int i = 0; i < trans.Length; i++ )
				{
					SpriteRenderer sr = (SpriteRenderer)trans[i].GetComponent<SpriteRenderer>();

					if ( sr )
					{
						sr.sprite = s;
						return true;
					}
				}
			}

			return false;
		}

		static public void CopyBlendShapes(Mesh mesh, Mesh clonemesh)
		{
			int bcount = mesh.blendShapeCount;

			Vector3[] deltaverts = new Vector3[mesh.vertexCount];
			Vector3[] deltanorms = new Vector3[mesh.vertexCount];
			Vector3[] deltatans = new Vector3[mesh.vertexCount];

			for ( int j = 0; j < bcount; j++ )
			{
				int frames = mesh.GetBlendShapeFrameCount(j);
				string bname = mesh.GetBlendShapeName(j);

				for ( int f = 0; f < frames; f++ )
				{
					mesh.GetBlendShapeFrameVertices(j, f, deltaverts, deltanorms, deltatans);
					float weight = mesh.GetBlendShapeFrameWeight(j, f);

					clonemesh.AddBlendShapeFrame(bname, weight, deltaverts, deltanorms, deltatans);
				}
			}
		}

		public static Mesh DupMesh(Mesh mesh, string suffix)
		{
			Mesh clonemesh = new Mesh();
			clonemesh.indexFormat = mesh.indexFormat;
			clonemesh.vertices = mesh.vertices;
			clonemesh.uv2 = mesh.uv2;
			clonemesh.uv3 = mesh.uv3;
			clonemesh.uv4 = mesh.uv4;
			clonemesh.uv = mesh.uv;
			clonemesh.normals = mesh.normals;
			clonemesh.tangents = mesh.tangents;
			clonemesh.colors = mesh.colors;

			clonemesh.subMeshCount = mesh.subMeshCount;

			for ( int s = 0; s < mesh.subMeshCount; s++ )
				clonemesh.SetTriangles(mesh.GetTriangles(s), s);

			CopyBlendShapes(mesh, clonemesh);
			clonemesh.boneWeights = mesh.boneWeights;
			clonemesh.bindposes = mesh.bindposes;
			clonemesh.name = mesh.name + suffix;
			clonemesh.RecalculateBounds();

			return clonemesh;
		}
	}
}