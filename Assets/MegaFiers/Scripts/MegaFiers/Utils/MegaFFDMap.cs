using UnityEngine;
using System.Collections.Generic;

namespace MegaFiers
{
	public class MegaFFDMap : MonoBehaviour
	{
		[System.Serializable]
		public class FFDNode
		{
			public bool			active;
			public int			x;
			public int			y;
			public int			z;
			public Transform	target;
		}

		public MegaFFD			ffd;
		public bool				onStartSetTargets = false;
		public List<FFDNode>	nodes = new List<FFDNode>();

		void Start()
		{
			if ( onStartSetTargets )
			{
				for ( int i = 0; i < nodes.Count; i++ )
				{
					if ( nodes[i].active )
					{
						if ( nodes[i].target )
							nodes[i].target.position = ffd.GetPointWorld(nodes[i].x, nodes[i].y, nodes[i].z);
					}
				}
			}
		}

		void LateUpdate()
		{
			if ( ffd )
			{
				for ( int i = 0; i < nodes.Count; i++ )
				{
					if ( nodes[i].active )
					{
						if ( nodes[i].target )
							ffd.SetPointWorld(nodes[i].x, nodes[i].y, nodes[i].z, nodes[i].target.position);
					}
				}
			}
		}

		public void SetActive(Transform t, bool act)
		{
			for ( int i = 0; i < nodes.Count; i++ )
			{
				if ( nodes[i].target == t )
					nodes[i].active = act;
			}
		}

		public void AddTarget(int x, int y, int z, Transform t)
		{
			if ( ffd )
			{
				if ( x >= 0 && x < ffd.GridSize() )
				{
					if ( y >= 0 && y < ffd.GridSize() )
					{
						if ( z >= 0 && z < ffd.GridSize() )
						{
							FFDNode node = new FFDNode();
							node.x = x;
							node.y = y;
							node.z = z;
							node.target = t;
						}
					}
				}
			}
		}

		public void RemoveTarget(Transform t)
		{
			for ( int i = 0; i < nodes.Count; i++ )
			{
				if ( nodes[i].target == t )
				{
					nodes.RemoveAt(i);
					return;
				}
			}
		}
	}
}