using UnityEngine;
using Unity.Collections;

namespace MegaFiers
{
	[System.Serializable]
	public class MegaSkinVert
	{
		public MegaSkinVert()
		{
			weights		= new float[4];
			bones		= new Transform[4];
			bindposes	= new Matrix4x4[4];
		}

		public float[]		weights;
		public Transform[]	bones;
		public Matrix4x4[]	bindposes;
		public int			vert;
	}

	[ExecuteInEditMode]
	public class MegaAttach : MonoBehaviour
	{
		public MegaModifyObject	target;
		[HideInInspector]
		public Vector3			BaryCoord		= Vector3.zero;
		[HideInInspector]
		public int[]			BaryVerts		= new int[3];
		[HideInInspector]
		public bool				attached		= false;
		[HideInInspector]
		public Vector3			BaryCoord1		= Vector3.zero;
		[HideInInspector]
		public int[]			BaryVerts1		= new int[3];
		public Vector3			AxisRot			= Vector3.zero;
		public float			radius			= 0.1f;
		Vector3					norm			= Vector3.zero;
		public bool				skinned;
		public bool				usebakedmesh	= true;
		public MegaSkinVert[]	skinverts;
		Vector3[]				calcskinverts;
		public Quaternion		attachrot		= Quaternion.identity;
		public Vector3			attachoff		= Vector3.zero;
		public Vector3			preAttachPos;
		public Quaternion		preAttachRot;
		public Vector3			fnorm;

		[ContextMenu("Help")]
		public void Help()
		{
			Application.OpenURL("http://www.west-racing.com/mf/?page_id=2645");
		}
	
		public void DetachIt()
		{
			attached = false;
			transform.localPosition = preAttachPos;
			transform.localRotation = preAttachRot;
		}

		public void AttachIt()
		{
			if ( target )
			{
				attached = true;

				preAttachPos = transform.localPosition;
				preAttachRot = transform.localRotation;

				if ( !InitSkin() )
				{
					Mesh mesh = target.mesh;
					Vector3 objSpacePt = target.transform.InverseTransformPoint(transform.position);
					NativeArray<Vector3> verts = target.GetVerts();	//jsverts;
					if ( verts.IsCreated )
					{
						int[] tris = mesh.triangles;
						int index = -1;
						MegaNearestPointTest.NearestPointOnMesh1(objSpacePt, verts, tris, ref index, ref BaryCoord);

						if ( index >= 0 )
						{
							BaryVerts[0] = tris[index];
							BaryVerts[1] = tris[index + 1];
							BaryVerts[2] = tris[index + 2];
						}

						fnorm = MegaPlane.FaceNormal(verts[tris[index]], verts[tris[index + 1]], verts[tris[index + 2]]).normalized;
						Vector3 attachforward = target.transform.InverseTransformDirection((verts[tris[index]] - verts[tris[index + 1]]).normalized);

						MegaNearestPointTest.NearestPointOnMesh1(objSpacePt + attachforward, verts, tris, ref index, ref BaryCoord1);

						if ( index >= 0 )
						{
							BaryVerts1[0] = tris[index];
							BaryVerts1[1] = tris[index + 1];
							BaryVerts1[2] = tris[index + 2];
						}

						// Now Calc pos so we can get off and rot
						Vector3 v0 = verts[BaryVerts[0]];
						Vector3 v1 = verts[BaryVerts[1]];
						Vector3 v2 = verts[BaryVerts[2]];

						Vector3 pos = GetCoordMine(v0, v1, v2, BaryCoord);
						attachoff = preAttachPos - pos;

						// Rotation
						Vector3 va = v1 - v0;
						Vector3 vb = v2 - v1;

						norm = Vector3.Cross(va, vb);

						v0 = verts[BaryVerts1[0]];
						v1 = verts[BaryVerts1[1]];
						v2 = verts[BaryVerts1[2]];

						Vector3 fwd = GetCoordMine(v0, v1, v2, BaryCoord1) - pos;

						Quaternion rot = Quaternion.LookRotation(fwd, norm);
						attachrot = Quaternion.Inverse(rot) * preAttachRot;
					}
				}
			}
		}

		void OnDrawGizmosSelected()
		{
			if ( !gameObject.activeInHierarchy )
				return;

			Vector3 pt = transform.position;

			Gizmos.color = Color.white;
			Gizmos.DrawSphere(pt, radius);

			if ( target )
			{
				if ( attached )
				{
					SkinnedMeshRenderer skin = target.GetComponent<SkinnedMeshRenderer>();

					if ( skin )
					{
						Vector3 pos = transform.position;

						Vector3 worldPt = pos;
						Gizmos.color = Color.green;
						Gizmos.DrawSphere(worldPt, radius);
					}
					else
					{
						NativeArray<Vector3> verts = target.GetVerts();
						if ( verts.IsCreated )
						{
							Vector3 pos = GetCoordMine(verts[BaryVerts[0]], verts[BaryVerts[1]], verts[BaryVerts[2]], BaryCoord);

							Vector3 worldPt = target.transform.TransformPoint(pos);
							Gizmos.color = Color.green;
							Gizmos.DrawSphere(worldPt, radius);
							Vector3 nw = target.transform.TransformDirection(norm.normalized * 1.0f);
							Gizmos.DrawLine(worldPt, worldPt + nw);
						}
					}
				}
				else
				{
					SkinnedMeshRenderer skin = target.GetComponent<SkinnedMeshRenderer>();

					if ( skin )
					{
						CalcSkinVerts();
						Mesh mesh = target.mesh;
						Vector3 objSpacePt = pt;
						Vector3[] verts = calcskinverts;

						if ( verts.Length > 0 )
						{
							int[] tris = mesh.triangles;
							int index = -1;
							Vector3 tribary = Vector3.zero;
							Vector3 meshPt = MegaNearestPointTest.NearestPointOnMesh1(objSpacePt, verts, tris, ref index, ref tribary);
							Vector3 worldPt = meshPt;	// target.transform.TransformPoint(meshPt);

							if ( index >= 0 )
							{
								Vector3 cp2 = GetCoordMine(verts[tris[index]], verts[tris[index + 1]], verts[tris[index + 2]], tribary);
								worldPt = cp2;
							}

							Gizmos.color = Color.red;
							Gizmos.DrawSphere(worldPt, radius);
						}
					}
					else
					{
						Mesh mesh = target.mesh;
						Vector3 objSpacePt = target.transform.InverseTransformPoint(pt);
						NativeArray<Vector3> verts = target.GetVerts();
						if ( verts.IsCreated )
						{
							int[] tris = mesh.triangles;
							int index = -1;
							Vector3 tribary = Vector3.zero;
							Vector3 meshPt = MegaNearestPointTest.NearestPointOnMesh1(objSpacePt, verts, tris, ref index, ref tribary);
							Vector3 worldPt = target.transform.TransformPoint(meshPt);

							if ( index >= 0 )
							{
								Vector3 cp2 = GetCoordMine(verts[tris[index]], verts[tris[index + 1]], verts[tris[index + 2]], tribary);
								worldPt = target.transform.TransformPoint(cp2);
							}

							Gizmos.color = Color.red;
							Gizmos.DrawSphere(worldPt, radius);
						}
					}
				}
			}
		}

		void LateUpdate()
		{
			if ( attached && target )
			{
				NativeArray<Vector3> verts = target.GetVerts();

				if ( skinned )
				{
					if ( verts.Length > 0 )
						GetSkinPos1();

					return;
				}

				if ( verts.IsCreated )
				{
					Vector3 v0 = verts[BaryVerts[0]];
					Vector3 v1 = verts[BaryVerts[1]];
					Vector3 v2 = verts[BaryVerts[2]];

					Vector3 pos = GetCoordMine(v0, v1, v2, BaryCoord);

					transform.localPosition = pos + attachoff;

					// Rotation
					Vector3 va = v1 - v0;
					Vector3 vb = v2 - v1;

					norm = Vector3.Cross(va, vb);

					v0 = verts[BaryVerts1[0]];
					v1 = verts[BaryVerts1[1]];
					v2 = verts[BaryVerts1[2]];

					Vector3 fwd = GetCoordMine(v0, v1, v2, BaryCoord1) - pos;

					Quaternion erot = Quaternion.Euler(AxisRot);
					Quaternion rot = Quaternion.LookRotation(fwd, norm) * erot;
					transform.localRotation = rot * attachrot;
				}
			}
		}

		void GetSkinPos1()
		{
			Vector3 v0 = GetSkinPos(0);
			Vector3 v1 = GetSkinPos(1);
			Vector3 v2 = GetSkinPos(2);

			Vector3 pos = GetCoordMine(v0, v1, v2, BaryCoord);

			transform.position = pos + attachoff;

			Vector3 va = v1 - v0;
			Vector3 vb = v2 - v1;

			norm = Vector3.Cross(va, vb);

			v0 = GetSkinPos(3);
			v1 = GetSkinPos(4);
			v2 = GetSkinPos(5);

			Vector3 fwd = GetCoordMine(v0, v1, v2, BaryCoord1) - pos;

			Quaternion erot = Quaternion.Euler(AxisRot);
			Quaternion rot = Quaternion.LookRotation(fwd, norm) * erot;	// * attachrot;
			transform.rotation = rot * attachrot;
		}

		Vector3 GetCoordMine(Vector3 A, Vector3 B, Vector3 C, Vector3 bary)
		{
			Vector3 p = Vector3.zero;
			p.x = (bary.x * A.x) + (bary.y * B.x) + (bary.z * C.x);
			p.y = (bary.x * A.y) + (bary.y * B.y) + (bary.z * C.y);
			p.z = (bary.x * A.z) + (bary.y * B.z) + (bary.z * C.z);

			return p;
		}

		bool InitSkin()
		{
			if ( target )
			{
				SkinnedMeshRenderer skin = target.GetComponent<SkinnedMeshRenderer>();

				if ( skin )
				{
					Quaternion rot = transform.rotation;
					attachrot = Quaternion.identity;

					skinned = true;

					Mesh ms = skin.sharedMesh;

					Vector3 pt = transform.position;

					CalcSkinVerts();
					Vector3 objSpacePt = pt;
					Vector3[] verts = calcskinverts;
					int[] tris = ms.triangles;
					int index = -1;
					MegaNearestPointTest.NearestPointOnMesh1(objSpacePt, verts, tris, ref index, ref BaryCoord);

					if ( index >= 0 )
					{
						BaryVerts[0] = tris[index];
						BaryVerts[1] = tris[index + 1];
						BaryVerts[2] = tris[index + 2];
					}

					fnorm = MegaPlane.FaceNormal(verts[tris[index]], verts[tris[index + 1]], verts[tris[index + 2]]).normalized;
					Vector3 attachforward = target.transform.InverseTransformDirection((verts[tris[index]] - verts[tris[index + 1]]).normalized);

					Vector3 v0 = verts[BaryVerts[0]];
					Vector3 v1 = verts[BaryVerts[1]];
					Vector3 v2 = verts[BaryVerts[2]];

					Vector3 pos = GetCoordMine(v0, v1, v2, BaryCoord);
					attachoff = preAttachPos - pos;

					MegaNearestPointTest.NearestPointOnMesh1(objSpacePt + attachforward, verts, tris, ref index, ref BaryCoord1);

					if ( index >= 0 )
					{
						BaryVerts1[0] = tris[index];
						BaryVerts1[1] = tris[index + 1];
						BaryVerts1[2] = tris[index + 2];
					}

					skinverts = new MegaSkinVert[6];

					for ( int i = 0; i < 3; i++ )
					{
						int vert = BaryVerts[i];
						BoneWeight bw = ms.boneWeights[vert];
						skinverts[i] = new MegaSkinVert();

						skinverts[i].vert = vert;
						skinverts[i].weights[0] = bw.weight0;
						skinverts[i].weights[1] = bw.weight1;
						skinverts[i].weights[2] = bw.weight2;
						skinverts[i].weights[3] = bw.weight3;

						skinverts[i].bones[0] = skin.bones[bw.boneIndex0];
						skinverts[i].bones[1] = skin.bones[bw.boneIndex1];
						skinverts[i].bones[2] = skin.bones[bw.boneIndex2];
						skinverts[i].bones[3] = skin.bones[bw.boneIndex3];

						skinverts[i].bindposes[0] = ms.bindposes[bw.boneIndex0];
						skinverts[i].bindposes[1] = ms.bindposes[bw.boneIndex1];
						skinverts[i].bindposes[2] = ms.bindposes[bw.boneIndex2];
						skinverts[i].bindposes[3] = ms.bindposes[bw.boneIndex3];
					}

					for ( int i = 3; i < 6; i++ )
					{
						int vert = BaryVerts1[i - 3];
						BoneWeight bw = ms.boneWeights[vert];
						skinverts[i] = new MegaSkinVert();

						skinverts[i].vert = vert;

						skinverts[i].weights[0] = bw.weight0;
						skinverts[i].weights[1] = bw.weight1;
						skinverts[i].weights[2] = bw.weight2;
						skinverts[i].weights[3] = bw.weight3;

						skinverts[i].bones[0] = skin.bones[bw.boneIndex0];
						skinverts[i].bones[1] = skin.bones[bw.boneIndex1];
						skinverts[i].bones[2] = skin.bones[bw.boneIndex2];
						skinverts[i].bones[3] = skin.bones[bw.boneIndex3];

						skinverts[i].bindposes[0] = ms.bindposes[bw.boneIndex0];
						skinverts[i].bindposes[1] = ms.bindposes[bw.boneIndex1];
						skinverts[i].bindposes[2] = ms.bindposes[bw.boneIndex2];
						skinverts[i].bindposes[3] = ms.bindposes[bw.boneIndex3];
					}

					GetSkinPos1();

					attachrot = Quaternion.Inverse(transform.rotation) * rot;
					return true;
				}
				else
					skinned = false;
			}

			return false;
		}

		Vector3 GetSkinPos(int i)
		{
			NativeArray<Vector3> verts = target.GetVerts();
			Vector3 pos = verts[skinverts[i].vert];
			Vector3 bpos = skinverts[i].bindposes[0].MultiplyPoint(pos);
			Vector3 p = skinverts[i].bones[0].TransformPoint(bpos) * skinverts[i].weights[0];

			bpos = skinverts[i].bindposes[1].MultiplyPoint(pos);
			p += skinverts[i].bones[1].TransformPoint(bpos) * skinverts[i].weights[1];
			bpos = skinverts[i].bindposes[2].MultiplyPoint(pos);
			p += skinverts[i].bones[2].TransformPoint(bpos) * skinverts[i].weights[2];
			bpos = skinverts[i].bindposes[3].MultiplyPoint(pos);
			p += skinverts[i].bones[3].TransformPoint(bpos) * skinverts[i].weights[3];
			return p;
		}

		void CalcSkinVerts()
		{
			SkinnedMeshRenderer skin = target.GetComponent<SkinnedMeshRenderer>();
			Mesh mesh = target.mesh;

			NativeArray<Vector3> verts = target.GetVerts();

			if ( calcskinverts == null || calcskinverts.Length != verts.Length )
				calcskinverts = new Vector3[verts.Length];

			Matrix4x4[] bindposes = mesh.bindposes;
			BoneWeight[] boneweights = mesh.boneWeights;

			for ( int i = 0; i < verts.Length; i++ )
			{
				Vector3 p = Vector3.zero;

				Vector3 pos = verts[i];
				Vector3 bpos = bindposes[boneweights[i].boneIndex0].MultiplyPoint(pos);
				p += skin.bones[boneweights[i].boneIndex0].TransformPoint(bpos) * boneweights[i].weight0;

				bpos = bindposes[boneweights[i].boneIndex1].MultiplyPoint(pos);
				p += skin.bones[boneweights[i].boneIndex1].TransformPoint(bpos) * boneweights[i].weight1;

				bpos = bindposes[boneweights[i].boneIndex2].MultiplyPoint(pos);
				p += skin.bones[boneweights[i].boneIndex2].TransformPoint(bpos) * boneweights[i].weight2;

				bpos = bindposes[boneweights[i].boneIndex3].MultiplyPoint(pos);
				p += skin.bones[boneweights[i].boneIndex3].TransformPoint(bpos) * boneweights[i].weight3;

				calcskinverts[i] = p;
			}
		}
	}
}