using UnityEngine;
using Unity.Collections;

namespace MegaFiers
{
	[ExecuteInEditMode]
	public class MegaAttachWrap : MonoBehaviour
	{
		public MegaWrap			target;
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

				Mesh mesh = target.mesh;
				Vector3 objSpacePt = target.transform.InverseTransformPoint(transform.position);
				NativeArray<Vector3> verts = target.verts;
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
					NativeArray<Vector3> verts = target.verts;
					if ( verts.IsCreated )
					{
						Vector3 pos = GetCoordMine(verts[BaryVerts[0]], verts[BaryVerts[1]], verts[BaryVerts[2]], BaryCoord);

						Vector3 worldPt = target.transform.TransformPoint(pos);
						Gizmos.color = Color.green;
						Gizmos.DrawSphere(worldPt, radius);
						Vector3 nw = target.transform.TransformDirection(norm * 40.0f);
						Gizmos.DrawLine(worldPt, worldPt + nw);
					}
				}
				else
				{
					Mesh mesh = target.mesh;
					Vector3 objSpacePt = target.transform.InverseTransformPoint(pt);
					NativeArray<Vector3> verts = target.verts;
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

		void LateUpdate()
		{
			if ( attached && target )
			{
				NativeArray<Vector3> verts = target.verts;

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

		Vector3 GetCoordMine(Vector3 A, Vector3 B, Vector3 C, Vector3 bary)
		{
			Vector3 p = Vector3.zero;
			p.x = (bary.x * A.x) + (bary.y * B.x) + (bary.z * C.x);
			p.y = (bary.x * A.y) + (bary.y * B.y) + (bary.z * C.y);
			p.z = (bary.x * A.z) + (bary.y * B.z) + (bary.z * C.z);

			return p;
		}
	}
}