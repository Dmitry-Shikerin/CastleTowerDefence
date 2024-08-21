using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
	public struct CalcBoundsJob : IJob
	{
		[ReadOnly]	public NativeArray<Vector3> verts;
		[WriteOnly] public Vector3	pos;
		[WriteOnly] public Vector3	size;

		public void Execute()
		{
			float3 minPos = new float3(float.MaxValue, float.MaxValue, float.MaxValue);
			float3 maxPos = new float3(float.MinValue, float.MinValue, float.MinValue);

			for ( int i = 0; i < verts.Length; i++ )
			{
				minPos = math.min(minPos, verts[i]);
				maxPos = math.max(maxPos, verts[i]);
			}

			pos = (maxPos + minPos) * 0.5f;
			size = maxPos - minPos;
		}
	}
}