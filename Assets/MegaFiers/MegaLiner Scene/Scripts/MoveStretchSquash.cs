#if false
using UnityEngine;
using System.Collections;

namespace MegaFiers
{
	[ExecuteInEditMode]
	public class MoveStretchSquash : MonoBehaviour
	{
		MegaStretch			stretch;
		MegaModifyObject	modobj;
		Vector3				lastPos;
		float				amount;
		float				camount;
		float				vel;
		float				lastSpd;
		public float		stretchAmt	= 1.0f;
		public float		damp		= 0.25f;
		Vector3				rvel;

		void Start()
		{
			modobj = GetComponent<MegaModifyObject>();
			if ( modobj )
				stretch = modobj.FindMod<MegaStretch>();

			lastPos = transform.position;
		}

		void Update()
		{
			if ( stretch )
			{
				Vector3 delta = transform.position - lastPos;

				float spd = delta.magnitude / Time.deltaTime;

				Quaternion rot = Quaternion.FromToRotation(delta.normalized, transform.forward);

				Vector3 r = rot.eulerAngles;
				r.y += 90.0f;
				stretch.gizmoRot = Vector3.SmoothDamp(stretch.gizmoRot, r, ref rvel, damp);

				float acc = (spd - lastSpd);

				amount = acc * stretchAmt;

				stretch.amount = Mathf.SmoothDamp(stretch.amount, amount, ref vel, damp);

				lastSpd = spd;
				lastPos = transform.position;
			}
		}
	}
}
#endif