using UnityEngine;
using Unity.Collections;

#if false
namespace MegaFiers
{
	[System.Serializable]
	public class MegaMorphTarget
	{
		public string				name = "Empty";
		public float				percent;
		public bool					showparams = true;
		//public Vector3[]	points;
		public NativeArray<Vector3>	points;
		public MOMVert[]			mompoints;
		public MOPoint[]			loadpoints;
	}

	[System.Serializable]
	public class MOPoint
	{
		public int		id;
		public Vector3	p;
		public float	w;
	}
}
#endif