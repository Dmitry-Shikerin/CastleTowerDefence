using UnityEngine;

namespace MegaFiers
{
	[AddComponentMenu("MegaShapes/Helix")]
	public class MegaShapeHelix : MegaShape
	{
		public float radius1	= 1.0f;
		public float radius2	= 0.75f;
		public float height		= 0.0f;
		public float turns		= 1.0f;
		public float bias		= 0.0f;
		public float adjust		= 0.4f;
		public bool	clockwise	= true;
		public int	PointsPerTurn = 8;
		const float CIRCLE_VECTOR_LENGTH = 0.5517861843f;

		public override void MakeShape()
		{
			Matrix4x4 tm = GetMatrix();

			PointsPerTurn = Mathf.Clamp(PointsPerTurn, 3, 100);

			// Delete all points in the existing spline
			MegaSpline spline = NewSpline();

			float fromrad = 0.0f;
			float torad = turns * Mathf.PI * 2.0f;

			// Order angles properly
			if ( fromrad > torad )
				torad += Mathf.PI * 2.0f;

			float totalRadians = Mathf.PI * 2.0f * turns;
			if ( clockwise )
				totalRadians *= -1.0f;

			int points = (int)(turns * (float)PointsPerTurn);
			if ( points == 0 )
				points = 1;

			float fPoints = (float)points;

			float totAngle = torad - fromrad;
			float vector1 = veccalc(totAngle / fPoints);

			float deltaRadius = radius2 - radius1;

			float power = 1.0f;
			if ( bias > 0.0f )
				power = bias * 9.0f + 1.0f;
			else
			{
				if ( bias < 0.0f )
					power = -bias * 9.0f + 1.0f;
			}

			// Now add all the necessary points
			for ( int ix = 0; ix <= points; ++ix )
			{
				float pct = (float)ix / fPoints;
				float r = radius1 + deltaRadius * pct;

				float hpct = pct;
				if ( bias > 0.0f )
					hpct = 1.0f - Mathf.Pow(1.0f - pct, power);
				else
				{
					if ( bias < 0.0f )
						hpct = Mathf.Pow(pct, power);
				}

				float angle = totalRadians * pct;
				float sinfac = Mathf.Sin(angle);
				float cosfac = Mathf.Cos(angle);
				float vector = vector1 * r;
				Vector3 p = new Vector3(cosfac * r, sinfac * r, height * hpct);
				Vector3 rotvec = new Vector3(sinfac * vector, -cosfac * vector, 0.0f);
				Vector3 invec = p + rotvec;
				Vector3 outvec = p - rotvec;
				if ( !clockwise )
					spline.AddKnot(p, invec, outvec, tm);
				else
					spline.AddKnot(p, outvec, invec, tm);
			}

			CalcLength();
		}
	}
}