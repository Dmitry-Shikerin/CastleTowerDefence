using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[BurstCompile]
	struct VertexBlendColJob : IJobParallelFor
	{
		[Unity.Collections.ReadOnly]
		public NativeArray<Vector3> origverts;
		public NativeArray<Vector3> verts;
		[Unity.Collections.ReadOnly]
		public NativeArray<Color>	cols;
		[Unity.Collections.ReadOnly]
		public float				weight;
		[Unity.Collections.ReadOnly]
		public int					channel;

		public void Execute(int i)
		{
			verts[i] = math.lerp(origverts[i], verts[i], cols[i][channel] * weight);
		}
	}

	[BurstCompile]
	struct VertexBlendJob : IJobParallelFor
	{
		[Unity.Collections.ReadOnly]
		public NativeArray<Vector3> origverts;
		public NativeArray<Vector3> verts;
		[Unity.Collections.ReadOnly]
		public float				weight;

		public void Execute(int i)
		{
			verts[i] = math.lerp(origverts[i], verts[i], weight);
		}
	}

	[BurstCompile]
	struct VertexBlendBoneJob : IJobParallelFor
	{
		[Unity.Collections.ReadOnly]
		public NativeArray<Vector3> origverts;
		public NativeArray<Vector3> verts;
		[Unity.Collections.ReadOnly]
		public NativeArray<float>	weights;
		[Unity.Collections.ReadOnly]
		public float weight;
		//[Unity.Collections.ReadOnly]
		//public int channel;

		public void Execute(int i)
		{
			verts[i] = math.lerp(origverts[i], verts[i], weights[i] * weight);
		}
	}
}
