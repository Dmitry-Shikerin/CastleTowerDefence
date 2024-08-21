using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[BurstCompile]
	struct animationcurve
	{
		[Unity.Collections.ReadOnly]
		public NativeArray<Keyframe>	keys;
		public int						lastkey;

		public float Evaluate(float t)
		{
			if ( t >= keys[lastkey].time && t <= keys[lastkey + 1].time )
			{
				Keyframe start = keys[lastkey];
				Keyframe end = keys[lastkey + 1];

				float distanceTime = end.time - start.time;

				float m0 = start.outTangent * distanceTime;
				float m1 = end.inTangent * distanceTime;

				t = math.clamp((t - start.time) / (end.time - start.time), 0.0f, 1.0f);

				float t2 = t * t;
				float t3 = t2 * t;

				float a = 2.0f * t3 - 3.0f * t2 + 1.0f;
				float b = t3 - 2.0f * t2 + t;
				float c = t3 - t2;
				float d = -2.0f * t3 + 3.0f * t2;

				return (a * start.value + b * m0 + c * m1 + d * end.value);
			}

			for ( int i = keys.Length - 2; i >= 0; i-- )
			{
				if ( t >= keys[i].time )
				{
					lastkey = i;
					Keyframe start = keys[i];
					Keyframe end = keys[i + 1];

					float distanceTime = end.time - start.time;

					float m0 = start.outTangent * distanceTime;
					float m1 = end.inTangent * distanceTime;

					t = math.clamp((t - start.time) / (end.time - start.time), 0.0f, 1.0f);

					float t2 = t * t;
					float t3 = t2 * t;

					float a = 2.0f * t3 - 3.0f * t2 + 1.0f;
					float b = t3 - 2.0f * t2 + t;
					float c = t3 - t2;
					float d = -2.0f * t3 + 3.0f * t2;

					return (a * start.value + b * m0 + c * m1 + d * end.value);
				}
			}

			return 0.0f;
		}
	}
}