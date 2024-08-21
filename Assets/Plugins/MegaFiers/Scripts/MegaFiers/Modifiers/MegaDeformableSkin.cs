using UnityEngine;
using System.Collections.Generic;

namespace MegaFiers
{
	[System.Serializable]
	public class DefBone
	{
		public int			bone;
		public Transform	transform;
		public float		weight;
	}

	[ExecuteInEditMode]
	public class MegaDeformableSkin : MonoBehaviour
	{
		//public GameObject	target;
		[HideInInspector]
		public Mesh					mesh;
		MeshRenderer				mr;
		public SkinnedMeshRenderer	smr;
		Material[]					mats;
		public bool					showWeights		= false;
		public bool					useBoneWeights	= false;
		public List<DefBone>		bones			= new List<DefBone>();
		public string				search			= "";
		public float				setvalue		= 0;

		void Start()
		{
			smr = transform.parent.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

			if ( smr )
			{
				smr.enabled = false;
				mats = smr.sharedMaterials;

				if ( !mesh )
					mesh = new Mesh();

				smr.BakeMesh(mesh);

				MeshFilter mf = GetComponent<MeshFilter>();
				if ( !mf )
					mf = gameObject.AddComponent<MeshFilter>();

				mr = gameObject.GetComponent<MeshRenderer>();

				if ( !mr )
					mr = gameObject.AddComponent<MeshRenderer>();

				mf.sharedMesh = mesh;

				mr.sharedMaterials = mats;

				MegaModifyObject mo = GetComponent<MegaModifyObject>();

				if ( !mo )
				{
					mo = gameObject.AddComponent<MegaModifyObject>();
					mo.defSkin = this;
				}

				if ( bones.Count == 0 )
				{
					bones.Clear();
					for ( int i = 0; i < smr.bones.Length; i++ )
					{
						DefBone db = new DefBone();
						db.bone = i;
						db.transform = smr.bones[i];
						db.weight = 1.0f;
						bones.Add(db);
					}
				}
				mo.CalcBoneWeigths();
			}
		}

		public void CalcWeights()
		{
			MegaModifyObject mo = GetComponent<MegaModifyObject>();
			if ( mo )
			{
				mo.CalcBoneWeigths();
			}
		}

		void Update()
		{
			if ( smr )
				smr.BakeMesh(mesh);
		}
	}
}