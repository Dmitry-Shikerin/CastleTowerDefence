using UnityEngine;
using System.Collections;

namespace MegaFiers
{
	public class MegaMeshRef : MonoBehaviour
	{
		public GameObject	target;
		MeshFilter			mf;
		MeshRenderer		mr;
		GameObject			ctarget;

		void Start()
		{
			mf = GetComponent<MeshFilter>();
			mr = GetComponent<MeshRenderer>();
			//SetTarget(target);
		}

		public void SetTarget(GameObject trg)
		{
			if ( trg && trg != ctarget )
			{
				ctarget = trg;
				MeshFilter tmf = trg.GetComponent<MeshFilter>();
				if ( tmf && mf )
				{
					mf.sharedMesh = tmf.sharedMesh;
				}

				MeshRenderer tmr = trg.GetComponent<MeshRenderer>();
				if ( tmr && mr )
				{
					mr.sharedMaterials = tmr.sharedMaterials;
				}
			}
		}

		void Update()
		{
			SetTarget(target);
		}
	}
}