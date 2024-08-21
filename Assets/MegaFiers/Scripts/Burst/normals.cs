using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[BurstCompile]
	struct FaceNormalsJob : IJobParallelFor
	{
		[Unity.Collections.ReadOnly]
		public NativeArray<Vector3>	verts;
		[Unity.Collections.ReadOnly]
		public NativeArray<int>		tris;
		public NativeArray<Vector3>	facenorms;

		public void Execute(int f)
		{
			int fi = f * 3;
			float3 v30 = verts[tris[fi]];
			float3 v31 = verts[tris[fi + 1]];
			float3 v32 = verts[tris[fi + 2]];

			float vax = v31.x - v30.x;
			float vay = v31.y - v30.y;
			float vaz = v31.z - v30.z;

			float vbx = v32.x - v31.x;
			float vby = v32.y - v31.y;
			float vbz = v32.z - v31.z;

			v30.x = vay * vbz - vaz * vby;
			v30.y = vaz * vbx - vax * vbz;
			v30.z = vax * vby - vay * vbx;

			// Uncomment this if you dont want normals weighted by poly size
			//float l = v30.x * v30.x + v30.y * v30.y + v30.z * v30.z;
			//l = 1.0f / mathf.sqrt(l);
			//v30.x *= l;
			//v30.y *= l;
			//v30.z *= l;

			facenorms[f] = v30;
		}
	}

	[BurstCompile]
	struct CalcNormalsJob : IJobParallelFor
	{
		[Unity.Collections.ReadOnly]
		public NativeArray<int>		faces;
		[Unity.Collections.ReadOnly]
		public NativeArray<int>		indexmap;
		[Unity.Collections.ReadOnly]
		public NativeArray<int>		mapping;
		[Unity.Collections.ReadOnly]
		public NativeArray<Vector3> facenorms;
		public NativeArray<Vector3> normals;

		public void Execute(int i)	// i is normal index
		{
			int fi = indexmap[i];	// offset into faces array that holds the facenorm indices 

			int count = mapping[fi];	// how many faces are used by this normal

			if ( count > 0 )
			{
				Vector3 norm = facenorms[mapping[fi + 1]];

				for ( int f = 1; f < count; f++ )
					norm += facenorms[mapping[fi + f + 1]];

				normals[i] = math.normalize(norm);
			}
			else
				normals[i] = Vector3.up;
		}
	}
}