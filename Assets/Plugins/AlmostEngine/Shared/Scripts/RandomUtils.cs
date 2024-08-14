using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AlmostEngine
{
	public class RandomUtils
	{
		public static Vector3 Vector3(float min = 0f, float max = 1f)
		{
			Vector3 rnd = new Vector3();
			rnd.x = Float(min, max);
			rnd.y = Float(min, max);
			rnd.z = Float(min, max);
			return rnd;
		}

		public static float Float(float min = 0f, float max = 1f)
		{
			return min + (max - min) * Random.value;
		}

		public static List<T> GetRandomIn<T>(List<T> list, int nb) where T : class
		{

			if (list.Count < nb) {
				Debug.LogError("Can not get that much object.");
			}

			List<T> objs = new List<T>();
			int id;
			while (objs.Count < nb && objs.Count < list.Count) {
				id = Random.Range(0, list.Count - 1);
				if (!objs.Contains(list[id])) {
					objs.Add(list[id]);
				}
			}

			return objs;
		}
	}
}

