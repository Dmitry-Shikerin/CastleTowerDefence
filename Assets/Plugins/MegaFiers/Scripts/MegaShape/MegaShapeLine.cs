using UnityEngine;

namespace MegaFiers
{
	[AddComponentMenu("MegaShapes/Line")]
	public class MegaShapeLine : MegaShape
	{
		public int			points = 2;
		public float		length	= 1.0f;
		public float		dir = 0.0f;
		public Transform	end;

		public override void MakeShape()
		{
			Matrix4x4 tm = GetMatrix();

			// Delete all points in the existing spline
			MegaSpline spline = NewSpline();

			float len = length;
			Vector3 ep = Vector3.zero;

			if ( end )
			{
				ep = transform.worldToLocalMatrix.MultiplyPoint(end.position);
				len = ep.magnitude;
			}
			else
			{
				ep.x = Mathf.Sin(Mathf.Deg2Rad * dir) * len;
				ep.z = Mathf.Cos(Mathf.Deg2Rad * dir) * len;
			}

			Vector3 norm = ep.normalized;

			if ( points < 2 )
				points = 2;

			float vlen = (len / (float)(points + 0)) / 2.0f;

			for ( int ix = 0; ix < points; ++ix )
			{
				float alpha = (float)ix / (float)(points - 1);
				Vector3 p = Vector3.Lerp(Vector3.zero, ep, alpha);
				Vector3 rotvec = new Vector3(norm.x * vlen, norm.y * vlen, norm.z * vlen);
				Vector3 invec = p - rotvec;
				Vector3 outvec = p + rotvec;
				spline.AddKnot(p, invec, outvec, tm);
			}

			CalcLength();
		}
	}
}