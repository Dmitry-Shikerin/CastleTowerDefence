using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[BurstCompile]
	public struct splinepoint
	{
		public float3	p;
		public float3	invec;
		public float3	outvec;
		public float	seglength;
		public float	length;
		public float	twist;
	}

	[BurstCompile]
	struct megaspline
	{
		[Unity.Collections.ReadOnly]
		public NativeArray<splinepoint>	points;
		public bool						closed;
		public float					length;
		public MegaShapeEase			twistmode;

		// New method that handles open splines better
		public float3 InterpCurve3D(float alpha, bool type, ref int k)
		{
			k = 0;

			if ( points.Length == 0 )
				return Vector3.zero;

			if ( alpha < 0.0f )
			{
				if ( closed )
					alpha = Mathf.Repeat(alpha, 1.0f);
				else
				{
					float3 ps = Interpolate(0.0f, type, ref k);

					// Need a proper tangent function
					float3 ps1 = Interpolate(0.001f, type, ref k);

					// Calc the spline in out vecs
					float3 delta = math.normalize(ps1 - ps);
					return ps + ((length * alpha) * delta);
				}
			}
			else
			{
				if ( alpha > 1.0f )
				{
					if ( closed )
						alpha = alpha % 1.0f;
					else
					{
						float3 ps = Interpolate(1.0f, type, ref k);

						// Need a proper tangent function
						float3 ps1 = Interpolate(0.999f, type, ref k);

						// Calc the spline in out vecs
						float3 delta = math.normalize(ps1 - ps);
						return ps + ((length * (1.0f - alpha)) * delta);
					}
				}
			}

			return Interpolate(alpha, type, ref k);
		}

		public float3 InterpCurve3D(float alpha, bool type, ref float twist)
		{
			int k = 0;
			//float i;

			if ( alpha < 0.0f )
			{
				if ( closed )
					alpha = Mathf.Repeat(alpha, 1.0f);
				else
				{
					float3 ps = Interpolate(0.0f, type, ref k, ref twist);
					float3 ps1 = Interpolate(0.001f, type, ref k);
					return ps + ((length * alpha) * math.normalize(ps1 - ps));
				}
			}
			else
			{
				if ( alpha > 1.0f )
				{
					if ( closed )
						alpha = Mathf.Repeat(alpha, 1.0f);
					else
					{
						float3 ps = Interpolate(1.0f, type, ref k, ref twist);
						float3 ps1 = Interpolate(0.999f, type, ref k);
						return ps + ((length * (1.0f - alpha)) * math.normalize(ps1 - ps));
					}
				}
			}

			return Interpolate(alpha, type, ref k, ref twist);
		}

		public float3 InterpCurve3D(float alpha, bool type)
		{
			int k = 0;
			//float i;

			if ( alpha < 0.0f )
			{
				if ( closed )
					alpha = Mathf.Repeat(alpha, 1.0f);
				else
				{
					float3 ps = Interpolate(0.0f, type, ref k);
					float3 ps1 = Interpolate(0.001f, type, ref k);
					return ps + ((length * alpha) * math.normalize(ps1 - ps));
				}
			}
			else
			{
				if ( alpha > 1.0f )
				{
					if ( closed )
						alpha = Mathf.Repeat(alpha, 1.0f);
					else
					{
						float3 ps = Interpolate(1.0f, type, ref k);
						float3 ps1 = Interpolate(0.999f, type, ref k);
						return ps + ((length * (1.0f - alpha)) * math.normalize(ps1 - ps));
					}
				}
			}

			return Interpolate(alpha, type, ref k);
		}

		public float3 Interpolate(float alpha, bool type, ref int k, ref float twist)
		{
			int seg = 0;

			//if ( constantSpeed )
			//return InterpolateCS(alpha, type, ref k, ref twist);

			// Special case if closed
			if ( closed )
			{
				if ( type )
				{
					float dist = alpha * length;

					if ( dist > points[points.Length - 1].length )
					{
						k = points.Length - 1;
						alpha = 1.0f - ((length - dist) / points[k].seglength);
						twist = TwistVal(points[k].twist, points[0].twist, alpha);
						return Interpolate(alpha, k, 0);    //knots[0]);
					}
					else
					{
						// Check passed in k sector first
						for ( seg = 0; seg < points.Length; seg++ )
						{
							if ( dist <= points[seg].length )
								break;
						}
					}
					alpha = 1.0f - ((points[seg].length - dist) / points[seg].seglength);
				}
				else
				{
					float segf = alpha * points.Length;

					seg = (int)segf;

					if ( seg == points.Length )
					{
						seg--;
						alpha = 1.0f;
					}
					else
						alpha = segf - seg;
				}

				if ( seg < points.Length - 1 )
				{
					k = seg;

					twist = TwistVal(points[seg].twist, points[seg + 1].twist, alpha);
					return Interpolate(alpha, seg, seg + 1);    //points[seg + 1]);
				}
				else
				{
					k = seg;

					twist = TwistVal(points[seg].twist, points[0].twist, alpha);
					return Interpolate(alpha, seg, 0);  //knots[0]);
				}
			}
			else
			{
				if ( type )
				{
					float dist = alpha * length;

					for ( seg = 0; seg < points.Length; seg++ )
					{
						if ( dist <= points[seg].length )
							break;
					}

					alpha = 1.0f - ((points[seg].length - dist) / points[seg].seglength);
				}
				else
				{
					float segf = alpha * points.Length;

					seg = (int)segf;

					if ( seg == points.Length )
					{
						seg--;
						alpha = 1.0f;
					}
					else
						alpha = segf - seg;
				}

				// Should check alpha
				if ( seg < points.Length - 1 )
				{
					k = seg;
					twist = TwistVal(points[seg].twist, points[seg + 1].twist, alpha);
					return Interpolate(alpha, seg, seg + 1);    //knots[seg + 1]);
				}
				else
				{
					k = seg;
					twist = points[seg].twist;
					return points[seg].p;
				}
			}
		}

		public float3 Interpolate(float alpha, bool type, ref int k)
		{
			int seg = 0;

			//if ( constantSpeed )
			//return InterpolateCS(alpha, type, ref k, ref twist);

			// Special case if closed
			if ( closed )
			{
				if ( type )
				{
					float dist = alpha * length;

					if ( dist > points[points.Length - 1].length )
					{
						k = points.Length - 1;
						alpha = 1.0f - ((length - dist) / points[k].seglength);
						return Interpolate(alpha, k, 0);    //knots[0]);
					}
					else
					{
						// Check passed in k sector first
						for ( seg = 0; seg < points.Length; seg++ )
						{
							if ( dist <= points[seg].length )
								break;
						}
					}
					alpha = 1.0f - ((points[seg].length - dist) / points[seg].seglength);
				}
				else
				{
					float segf = alpha * points.Length;

					seg = (int)segf;

					if ( seg == points.Length )
					{
						seg--;
						alpha = 1.0f;
					}
					else
						alpha = segf - seg;
				}

				if ( seg < points.Length - 1 )
				{
					k = seg;
					return Interpolate(alpha, seg, seg + 1);    //points[seg + 1]);
				}
				else
				{
					k = seg;
					return Interpolate(alpha, seg, 0);  //knots[0]);
				}
			}
			else
			{
				if ( type )
				{
					float dist = alpha * length;

					for ( seg = 0; seg < points.Length; seg++ )
					{
						if ( dist <= points[seg].length )
							break;
					}

					alpha = 1.0f - ((points[seg].length - dist) / points[seg].seglength);
				}
				else
				{
					float segf = alpha * points.Length;

					seg = (int)segf;

					if ( seg == points.Length )
					{
						seg--;
						alpha = 1.0f;
					}
					else
						alpha = segf - seg;
				}

				// Should check alpha
				if ( seg < points.Length - 1 )
				{
					k = seg;
					return Interpolate(alpha, seg, seg + 1);    //knots[seg + 1]);
				}
				else
				{
					k = seg;
					return points[seg].p;
				}
			}
		}

		public float3 Interpolate(float t, int k, int k1)
		{
			float omt = 1.0f - t;

			float omt2 = omt * omt;
			float omt3 = omt2 * omt;

			float t2 = t * t;
			float t3 = t2 * t;

			omt2 = 3.0f * omt2 * t;
			omt = 3.0f * omt * t2;

			return (omt3 * points[k].p) + (omt2 * points[k].outvec) + (omt * points[k1].invec) + (t3 * points[k1].p);
		}

		float TwistVal(float v1, float v2, float alpha)
		{
			if ( twistmode == MegaShapeEase.Linear )
				return math.lerp(v1, v2, alpha);

			return easeInOutSine(v1, v2, alpha);
		}

		float easeInOutSine(float start, float end, float value)
		{
			end -= start;
			return -end / 2.0f * (math.cos(math.PI * value / 1.0f) - 1.0f) + start;
		}

		// Find nearest
		public float3 FindNearestPoint(float3 p, int iterations, ref int kn, ref float3 tangent, ref float alpha)
		{
			float positiveInfinity = float.PositiveInfinity;
			float num2 = 0.0f;
			iterations = math.clamp(iterations, 0, 5);
			int kt = 0;

			for ( float i = 0.0f; i <= 1.0f; i += 0.01f )
			{
				float3 vector = Interpolate(i, true, ref kt) - p;
				float sqrMagnitude = math.lengthsq(vector);	//.sqrMagnitude;
				if ( positiveInfinity > sqrMagnitude )
				{
					positiveInfinity = sqrMagnitude;
					num2 = i;
				}
			}

			for ( int j = 0; j < iterations; j++ )
			{
				float num6 = 0.01f * math.pow(10.0f, -((float)j));
				float num7 = num6 * 0.1f;
				for ( float k = math.clamp(num2 - num6, 0, 1); k <= math.clamp(num2 + num6, 0, 1); k += num7 )
				{
					float3 vector2 = Interpolate(k, true, ref kt) - p;
					float num9 = math.lengthsq(vector2);	//.sqrMagnitude;

					if ( positiveInfinity > num9 )
					{
						positiveInfinity = num9;
						num2 = k;
					}
				}
			}

			kn = kt;
			tangent = InterpCurve3D(num2 + 0.01f, true);
			alpha = num2;
			return InterpCurve3D(num2, true);
		}

		public float3 FindNearestPointXZ(float3 p, int iterations, ref int kn, ref float3 tangent, ref float alpha)
		{
			float positiveInfinity = float.PositiveInfinity;
			float num2 = 0.0f;
			iterations = math.clamp(iterations, 0, 5);
			int kt = 0;

			for ( float i = 0.0f; i <= 1.0f; i += 0.01f )
			{
				float3 vector = Interpolate(i, true, ref kt) - p;
				vector.y = 0.0f;
				float sqrMagnitude = math.lengthsq(vector);	//.sqrMagnitude;
				if ( positiveInfinity > sqrMagnitude )
				{
					positiveInfinity = sqrMagnitude;
					num2 = i;
				}
			}

			for ( int j = 0; j < iterations; j++ )
			{
				float num6 = 0.01f * math.pow(10.0f, -((float)j));
				float num7 = num6 * 0.1f;
				for ( float k = math.clamp(num2 - num6, 0, 1); k <= math.clamp(num2 + num6, 0, 1); k += num7 )
				{
					float3 vector2 = Interpolate(k, true, ref kt) - p;

					vector2.y = 0.0f;
					float num9 = math.lengthsq(vector2);	//.sqrMagnitude;

					if ( positiveInfinity > num9 )
					{
						positiveInfinity = num9;
						num2 = k;
					}
				}
			}

			kn = kt;
			tangent = InterpCurve3D(num2 + 0.01f, true);
			alpha = num2;
			float3 rval = InterpCurve3D(num2, true);
			rval.y = 0.0f;
			return rval;
		}

		public float3 FindNearestPoint(float3 p, int iterations, ref float alpha)
		{
			float positiveInfinity = float.PositiveInfinity;
			float num2 = 0.0f;
			iterations = math.clamp(iterations, 0, 5);
			int kt = 0;

			for ( float i = 0.0f; i <= 1.0f; i += 0.01f )
			{
				float3 vector = Interpolate(i, true, ref kt) - p;
				float sqrMagnitude = math.lengthsq(vector); //.sqrMagnitude;
				if ( positiveInfinity > sqrMagnitude )
				{
					positiveInfinity = sqrMagnitude;
					num2 = i;
				}
			}

			for ( int j = 0; j < iterations; j++ )
			{
				float num6 = 0.01f * math.pow(10.0f, -((float)j));
				float num7 = num6 * 0.1f;
				for ( float k = math.clamp(num2 - num6, 0, 1); k <= math.clamp(num2 + num6, 0, 1); k += num7 )
				{
					float3 vector2 = Interpolate(k, true, ref kt) - p;
					float num9 = math.lengthsq(vector2);    //.sqrMagnitude;

					if ( positiveInfinity > num9 )
					{
						positiveInfinity = num9;
						num2 = k;
					}
				}
			}

			alpha = num2;
			return InterpCurve3D(num2, true);
		}
	}
}