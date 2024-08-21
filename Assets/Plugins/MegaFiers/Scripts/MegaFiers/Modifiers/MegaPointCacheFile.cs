using UnityEngine;
using Unity.Collections;

namespace MegaFiers
{
	[System.Serializable]
	public class MegaPCVert
	{
		public int[]		indices;
		public Vector3[]	points;
	}

	public class MegaPointCacheFile : ScriptableObject
	{
		[HideInInspector]
		public Vector3[]			cacheValues;
		[HideInInspector]
		public NativeArray<Vector3> points;
		[HideInInspector]
		public MegaPCVert[]			Verts;  // editor only so lose this once we have it all working
	}
}