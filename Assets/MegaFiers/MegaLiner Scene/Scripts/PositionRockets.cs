using UnityEngine;

namespace MegaFiers
{
	public class PositionRockets : MonoBehaviour
	{
		public GameObject		frontRocket;
		public GameObject[]		targets;
		public float			spacing	= 12.0f;
		public float			angle	= 0.0f;
		public int				frontIndex;
		float					lastspacing;
		float					lastangle;

		static public int Compare(GameObject o1, GameObject o2)
		{
			if ( o1.transform.position.x < o2.transform.position.x )
				return -1;

			if ( o1.transform.position.x > o2.transform.position.x )
				return 1;

			return 0;
		}

		void Start()
		{
			targets = GameObject.FindGameObjectsWithTag("LookAt");

			System.Array.Sort(targets, Compare);

			for ( int i = 0; i < targets.Length; i++ )
			{
				if ( targets[i].transform.root.gameObject == frontRocket )
				{
					frontIndex = i;
					break;
				}
			}

			Position();
		}

		private void Update()
		{
			if ( spacing != lastspacing || angle != lastangle )
			{
				lastspacing = spacing;
				lastangle = angle;
				Position();
			}
		}

		void Position()
		{
			Vector3 lessDir = new Vector3(-Mathf.Cos(angle * Mathf.Deg2Rad), 0.0f, Mathf.Sin(angle * Mathf.Deg2Rad));
			Vector3 greatDir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0.0f, Mathf.Sin(angle * Mathf.Deg2Rad));

			for ( int i = 0; i < targets.Length; i++ )
			{
				if ( i != frontIndex )
				{
					if ( i < frontIndex )
						targets[i].transform.root.position = frontRocket.transform.root.position + (lessDir * (frontIndex - i) * spacing);
					else
						targets[i].transform.root.position = frontRocket.transform.root.position + (greatDir * (i - frontIndex) * spacing);
				}
			}
		}
	}
}