using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

// Helper script to switch to mesh version if a deform and back to sprite if none
namespace MegaFiers
{
	public class MegaSpriteToMesh
	{
#if UNITY_EDITOR
		[MenuItem("GameObject/Convert Sprite To Mesh")]
		static public void Convert()
		{
			ConvertSpriteToMesh(Selection.activeGameObject);
		}

		[MenuItem("MegaFiers/Create Mesh Object for Sprite")]
		static public void Convert1()
		{
			GameObject obj = CreateMeshObjectFromSprite(Selection.activeGameObject, 4, Vector3.zero, null);
		}
#endif

		static public void ConvertSpriteToMesh(GameObject spriteObj)
		{
			if ( spriteObj )
			{
				SpriteRenderer sr = spriteObj.GetComponent<SpriteRenderer>();

				if ( sr )
				{
					Sprite sprite = sr.sprite;

					if ( sprite )
					{
						Mesh mesh = new Mesh();
						mesh.Clear();

						Vector3[] verts = new Vector3[sprite.vertices.Length];
						Vector2[] verts2d = sprite.vertices;

						for ( int i = 0; i < verts2d.Length; i++ )
						{
							Vector3 v = verts2d[i];
							verts[i] = v;
						}

						mesh.vertices = verts;
						mesh.uv = sprite.uv;

						ushort[] tris2d = sprite.triangles;
						int[] tris = new int[tris2d.Length];

						for ( int i = 0; i < tris2d.Length; i++ )
							tris[i] = (int)tris2d[i];

						mesh.triangles = tris;

						mesh.RecalculateBounds();
						mesh.RecalculateNormals();
						mesh.RecalculateTangents();

						MeshHelper.Subdivide(mesh, 4);

						Material mat = new Material(Shader.Find("Standard"));
						mat.SetTexture("_MainTex", sprite.texture);

						GameObject.DestroyImmediate(sr);

						MeshFilter mf = spriteObj.AddComponent<MeshFilter>();
						mf.sharedMesh = mesh;
						MeshRenderer mr = spriteObj.AddComponent<MeshRenderer>();

						mr.sharedMaterial = mat;

						mr.sortingLayerID = sr.sortingLayerID;
						mr.sortingOrder = sr.sortingOrder;
						mr.sortingLayerName = sr.sortingLayerName;
					}
				}
			}
		}

		static public void UpdateMesh(GameObject meshobj, GameObject spriteObj, int subdiv, Vector3 pivot, MegaSprite msprite)
		{
			if ( meshobj && spriteObj )
			{
				SpriteRenderer sr = spriteObj.GetComponent<SpriteRenderer>();

				if ( sr )
				{
					Sprite sprite = sr.sprite;

					if ( sprite )
					{
						Mesh mesh = new Mesh();
						mesh.Clear();

						Vector3[] verts = new Vector3[sprite.vertices.Length];
						Vector2[] verts2d = sprite.vertices;

						for ( int i = 0; i < verts2d.Length; i++ )
						{
							Vector3 v = verts2d[i];
							verts[i] = v - pivot;
						}

						mesh.vertices = verts;
						mesh.uv = sprite.uv;

						ushort[] tris2d = sprite.triangles;
						int[] tris = new int[tris2d.Length];

						for ( int i = 0; i < tris2d.Length; i++ )
							tris[i] = (int)tris2d[i];

						mesh.triangles = tris;

						mesh.RecalculateBounds();
						mesh.RecalculateNormals();
						mesh.RecalculateTangents();

						MeshHelper.Subdivide(mesh, subdiv);

						MeshFilter mf = meshobj.GetComponent<MeshFilter>();
						mf.sharedMesh = mesh;

						MegaModifyObject modobj = meshobj.GetComponent<MegaModifyObject>();
						if ( modobj )
							modobj.dynamicMesh = true;

						meshobj.transform.localPosition = pivot;

						Material mat = meshobj.GetComponent<MeshRenderer>().sharedMaterial;
						if ( mat )
							mat.SetTexture(msprite.texname, sprite.texture);
					}
				}
			}
		}

		static public GameObject CreateMeshObjectFromSprite(GameObject spriteObj, int subdiv, Vector3 pivot, Shader shader, string shadername = "Unlit/Texture", string texname = "_MainTex")
		{
			GameObject newobj = null;

			if ( spriteObj )
			{
				SpriteRenderer sr = spriteObj.GetComponent<SpriteRenderer>();

				if ( sr )
				{
					Sprite sprite = sr.sprite;

					if ( sprite )
					{
						Mesh mesh = new Mesh();
						mesh.Clear();

						Vector3[] verts = new Vector3[sprite.vertices.Length];
						Vector2[] verts2d = sprite.vertices;

						for ( int i = 0; i < verts2d.Length; i++ )
						{
							Vector3 v = verts2d[i];
							verts[i] = v - pivot;
						}

						mesh.vertices = verts;
						mesh.uv = sprite.uv;

						ushort[] tris2d = sprite.triangles;
						int[] tris = new int[tris2d.Length];

						for ( int i = 0; i < tris2d.Length; i++ )
							tris[i] = (int)tris2d[i];

						mesh.triangles = tris;

						mesh.RecalculateBounds();
						mesh.RecalculateNormals();
						mesh.RecalculateTangents();

						MeshHelper.Subdivide(mesh, subdiv);

						Material mat;
						if ( shader )
							mat = new Material(shader);
						else
							mat = new Material(Shader.Find(shadername));

						if ( mat )
							mat.SetTexture(texname, sprite.texture);

						newobj = new GameObject(spriteObj.name + " Mesh");
						newobj.transform.parent			= spriteObj.transform;
						newobj.transform.localPosition	= pivot;	//Vector3.zero;
						newobj.transform.localRotation	= Quaternion.identity;
						newobj.transform.localScale		= Vector3.one;

						MeshFilter mf = newobj.AddComponent<MeshFilter>();
						mf.sharedMesh = mesh;
						MeshRenderer mr = newobj.AddComponent<MeshRenderer>();
						mr.sharedMaterial = mat;

						mr.sortingLayerID = sr.sortingLayerID;
						mr.sortingOrder = sr.sortingOrder;
						mr.sortingLayerName = sr.sortingLayerName;
					}
				}
			}

			return newobj;
		}
	}

	public static class MeshHelper
	{
		static List<Vector3>			vertices;
		static List<Vector3>			normals;
		static List<Color>				colors;
		static List<Vector2>			uv;
		static List<Vector2>			uv2;
		static List<int>				indices;
		static Dictionary<uint, int>	newVectices;

		static void InitArrays(Mesh mesh)
		{
			vertices	= new List<Vector3>(mesh.vertices);
			normals		= new List<Vector3>(mesh.normals);
			colors		= new List<Color>(mesh.colors);
			uv			= new List<Vector2>(mesh.uv);
			uv2			= new List<Vector2>(mesh.uv2);
			indices		= new List<int>();
		}
		static void CleanUp()
		{
			vertices	= null;
			normals		= null;
			colors		= null;
			uv			= null;
			uv2			= null;
			indices		= null;
		}

		#region Subdivide4 (2x2)
		static int GetNewVertex4(int i1, int i2)
		{
			int newIndex = vertices.Count;
			uint t1 = ((uint)i1 << 16) | (uint)i2;
			uint t2 = ((uint)i2 << 16) | (uint)i1;
			if ( newVectices.ContainsKey(t2) )
				return newVectices[t2];
			if ( newVectices.ContainsKey(t1) )
				return newVectices[t1];

			newVectices.Add(t1, newIndex);

			vertices.Add((vertices[i1] + vertices[i2]) * 0.5f);
			if ( normals.Count > 0 )
				normals.Add((normals[i1] + normals[i2]).normalized);
			if ( colors.Count > 0 )
				colors.Add((colors[i1] + colors[i2]) * 0.5f);
			if ( uv.Count > 0 )
				uv.Add((uv[i1] + uv[i2]) * 0.5f);
			if ( uv2.Count > 0 )
				uv2.Add((uv2[i1] + uv2[i2]) * 0.5f);

			return newIndex;
		}

		// Devides each triangles into 4. A quad(2 tris) will be splitted into 2x2 quads( 8 tris )
		public static void Subdivide4(Mesh mesh)
		{
			newVectices = new Dictionary<uint, int>();

			InitArrays(mesh);

			int[] triangles = mesh.triangles;
			for ( int i = 0; i < triangles.Length; i += 3 )
			{
				int i1 = triangles[i + 0];
				int i2 = triangles[i + 1];
				int i3 = triangles[i + 2];

				int a = GetNewVertex4(i1, i2);
				int b = GetNewVertex4(i2, i3);
				int c = GetNewVertex4(i3, i1);
				indices.Add(i1); indices.Add(a); indices.Add(c);
				indices.Add(i2); indices.Add(b); indices.Add(a);
				indices.Add(i3); indices.Add(c); indices.Add(b);
				indices.Add(a); indices.Add(b); indices.Add(c); // center triangle
			}
			mesh.vertices = vertices.ToArray();
			if ( normals.Count > 0 )
				mesh.normals = normals.ToArray();
			if ( colors.Count > 0 )
				mesh.colors = colors.ToArray();
			if ( uv.Count > 0 )
				mesh.uv = uv.ToArray();
			if ( uv2.Count > 0 )
				mesh.uv2 = uv2.ToArray();

			mesh.triangles = indices.ToArray();

			CleanUp();
		}
		#endregion Subdivide4 (2x2)

		#region Subdivide9 (3x3)
		static int GetNewVertex9(int i1, int i2, int i3)
		{
			int newIndex = vertices.Count;

			// center points don't go into the edge list
			if ( i3 == i1 || i3 == i2 )
			{
				uint t1 = ((uint)i1 << 16) | (uint)i2;
				if ( newVectices.ContainsKey(t1) )
					return newVectices[t1];
				newVectices.Add(t1, newIndex);
			}

			// calculate new vertex
			vertices.Add((vertices[i1] + vertices[i2] + vertices[i3]) / 3.0f);
			if ( normals.Count > 0 )
				normals.Add((normals[i1] + normals[i2] + normals[i3]).normalized);
			if ( colors.Count > 0 )
				colors.Add((colors[i1] + colors[i2] + colors[i3]) / 3.0f);
			if ( uv.Count > 0 )
				uv.Add((uv[i1] + uv[i2] + uv[i3]) / 3.0f);
			if ( uv2.Count > 0 )
				uv2.Add((uv2[i1] + uv2[i2] + uv2[i3]) / 3.0f);
			return newIndex;
		}


		// Devides each triangles into 9. A quad(2 tris) will be splitted into 3x3 quads( 18 tris )
		public static void Subdivide9(Mesh mesh)
		{
			newVectices = new Dictionary<uint, int>();

			InitArrays(mesh);

			int[] triangles = mesh.triangles;
			for ( int i = 0; i < triangles.Length; i += 3 )
			{
				int i1 = triangles[i + 0];
				int i2 = triangles[i + 1];
				int i3 = triangles[i + 2];

				int a1 = GetNewVertex9(i1, i2, i1);
				int a2 = GetNewVertex9(i2, i1, i2);
				int b1 = GetNewVertex9(i2, i3, i2);
				int b2 = GetNewVertex9(i3, i2, i3);
				int c1 = GetNewVertex9(i3, i1, i3);
				int c2 = GetNewVertex9(i1, i3, i1);

				int d = GetNewVertex9(i1, i2, i3);

				indices.Add(i1); indices.Add(a1); indices.Add(c2);
				indices.Add(i2); indices.Add(b1); indices.Add(a2);
				indices.Add(i3); indices.Add(c1); indices.Add(b2);
				indices.Add(d); indices.Add(a1); indices.Add(a2);
				indices.Add(d); indices.Add(b1); indices.Add(b2);
				indices.Add(d); indices.Add(c1); indices.Add(c2);
				indices.Add(d); indices.Add(c2); indices.Add(a1);
				indices.Add(d); indices.Add(a2); indices.Add(b1);
				indices.Add(d); indices.Add(b2); indices.Add(c1);
			}

			if ( vertices.Count > 65535 )
				mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
			else
				mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;

			mesh.vertices = vertices.ToArray();
			if ( normals.Count > 0 )
				mesh.normals = normals.ToArray();
			if ( colors.Count > 0 )
				mesh.colors = colors.ToArray();
			if ( uv.Count > 0 )
				mesh.uv = uv.ToArray();
			if ( uv2.Count > 0 )
				mesh.uv2 = uv2.ToArray();

			mesh.triangles = indices.ToArray();

			CleanUp();
		}
		#endregion Subdivide9 (3x3)

		#region Subdivide
		// This functions subdivides the mesh based on the level parameter
		// Note that only the 4 and 9 subdivides are supported so only those divides
		// are possible. [2,3,4,6,8,9,12,16,18,24,27,32,36,48,64, ...]
		// The function tried to approximate the desired level 
		public static void Subdivide(Mesh mesh, int level)
		{
			if ( level < 2 )
				return;
			while ( level > 1 )
			{
				// remove prime factor 3
				while ( level % 3 == 0 )
				{
					Subdivide9(mesh);
					level /= 3;
				}
				// remove prime factor 2
				while ( level % 2 == 0 )
				{
					Subdivide4(mesh);
					level /= 2;
				}
				// try to approximate. All other primes are increased by one
				// so they can be processed
				if ( level > 3 )
					level++;
			}
		}
		#endregion Subdivide

		public static Mesh DuplicateMesh(Mesh mesh)
		{
			return (Mesh)UnityEngine.Object.Instantiate(mesh);
		}
	}
}