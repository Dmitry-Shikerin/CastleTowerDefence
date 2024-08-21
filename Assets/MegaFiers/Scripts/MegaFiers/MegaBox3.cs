using UnityEngine;

namespace MegaFiers
{
	[System.Serializable]
	public struct MegaBox3
	{
		public Vector3		center;
		public Vector3		min;
		public Vector3		max;

		public Vector3 Size()
		{
			return max - min;
		}

		public void SetSize(Vector3 size)
		{
			min = -(size * 0.5f);
			max = (size * 0.5f);
			center = Vector3.zero;
		}

		public float Radius()
		{
			return (max - min).magnitude;
		}

		public override string ToString()
		{
			return "cen " + center + " min " + min + " max " + max;
		}
	}
}