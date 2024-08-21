using UnityEngine;

namespace MegaFiers
{
	[ExecuteInEditMode]
	public class MegaScroll : MonoBehaviour
	{
		public float	angle1	= 2500.0f;
		public float	angle2	= 2500.0f;
		public float	pos		= -2.0f;
		public float	gap		= -2.4f;
		public Vector3	wpos;
		MegaBend[]		bends;
		public float	size;
		public bool		useLimits	= true;

		private void Start()
		{
			bends = GetComponents<MegaBend>();

			if ( bends.Length == 0 )
			{
				Mesh mesh = MegaUtils.GetSharedMesh(gameObject);

				MegaModifyObject modobj = gameObject.AddComponent<MegaModifyObject>();
				modobj.UpdateMode = MegaUpdateMode.LateUpdate;

				Bounds box = mesh.bounds;

				MegaBend bend1	= gameObject.AddComponent<MegaBend>();
				bend1.angle		= -angle1;
				bend1.doRegion	= true;
				bend1.from		= -box.size.x;
				bend1.gizmoPos	= new Vector3(-box.size.x * 0.5f, 0.0f, 0.0f);
				bend1.gizmoRot	= new Vector3(0.0f, -0.5f, -0.5f);
				bend1.Offset	= new Vector3(0.0f, 0.0f, 0.0f);
				bend1.axis		= MegaAxis.X;

				MegaBend bend2	= gameObject.AddComponent<MegaBend>();
				bend2.angle		= -angle2;
				bend2.doRegion	= true;
				bend2.to		= box.size.x;
				bend2.gizmoPos	= new Vector3(box.size.x * 0.5f, 0.0f, 0.0f);
				bend2.gizmoRot	= new Vector3(0.0f, 1.0f, 1.5f);
				bend2.Offset	= new Vector3(-box.size.x * 0.5f, 0.0f, 0.0f);
				bend2.axis		= MegaAxis.X;

				pos = 0.0f;
				gap = -box.size.x * 0.5f;
				size = box.size.x;
			}
		}

		void Update()
		{
			if ( bends.Length >= 2 )
			{
				if ( useLimits )
				{
					float gh = (size * 0.5f) - (gap);
					pos = Mathf.Clamp(pos, gap, (size * 0.5f) - gap);	// + gap);
					//gap = Mathf.Clamp(gap, -size + 1.0f, -1.4f);
				}

				bends[0].angle = -angle1;
				bends[1].angle = -angle2;
				bends[1].gizmoPos.x = pos - gap;
				bends[0].gizmoPos.x = pos + gap;

				Vector3 p = transform.localPosition;

				p.x = wpos.x + pos;
				transform.localPosition = p;
			}
			else
			{
				bends = GetComponents<MegaBend>();
			}
		}
	}
}