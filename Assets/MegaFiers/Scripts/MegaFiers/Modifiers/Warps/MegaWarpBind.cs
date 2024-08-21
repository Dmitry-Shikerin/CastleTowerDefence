using UnityEngine;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Bind")]
	public class MegaWarpBind : MegaModifier
	{
		[Adjust("Warp")]
		public GameObject	SourceWarpObj;
		GameObject			current;
		[Adjust]
		public float		decay = 0.0f;
		MegaWarp			warp;

		public override string ModName()	{ return "WarpBind"; }
		public override string GetHelpURL() { return "?page_id=576"; }

		[ContextMenu("Add To Siblings")]
		public void AddSiblings()
		{
			Transform parent = transform.parent;

			MegaModifyObject thismod = GetComponent<MegaModifyObject>();

			for ( int i = 0; i < parent.childCount; i++ )
			{
				Transform child = parent.GetChild(i);

				MegaWarpBind bind = child.gameObject.GetComponent<MegaWarpBind>();

				if ( !bind )
				{
					bind = child.gameObject.AddComponent<MegaWarpBind>();

					MegaModifyObject mod = child.gameObject.GetComponent<MegaModifyObject>();

					mod.NormalMethod = thismod.NormalMethod;

					bind.SetTarget(SourceWarpObj);
				}
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			// Get point to World Space as its a WSM
			p = tm.MultiplyPoint3x4(p);

			// So this mod should transform world point into local space of mod (gizmo offset if OSM, node tm if warp) 
			p = warp.Map(i, p);

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( SourceWarpObj != current )
			{
				current = SourceWarpObj;
				warp = null;
			}

			if ( SourceWarpObj != null )
			{
				if ( warp == null )
					warp = SourceWarpObj.GetComponent<MegaWarp>();

				if ( warp != null && warp.Enabled )
				{
					tm = transform.localToWorldMatrix;
					invtm = tm.inverse;

					return warp.Prepare(decay);
				}
			}

			return false;
		}

		public void SetTarget(GameObject go)
		{
			SourceWarpObj = go;
		}

		public override void Modify(MegaModifyObject mc)
		{
			warp.Modify(this);
	#if false
			for ( int i = 0; i < verts.Length; i++ )
			{
				Vector3 p = tm.MultiplyPoint3x4(verts[i]);

				// So this mod should transform world point into local space of mod (gizmo offset if OSM, node tm if warp) 
				p = warp.Map(i, p);

				sverts[i] = invtm.MultiplyPoint3x4(p);
			}
	#endif
		}

		public override void ModStart(MegaModifyObject mc)
		{
			if ( SourceWarpObj != null && SourceWarpObj != current )
			{
				current = SourceWarpObj;
				warp = SourceWarpObj.GetComponent<MegaWarp>();
			}
		}

		public override bool Changed()
		{
			if ( ModEnabled )
			{
				lastEnabled = ModEnabled;
				if ( alwaysChanging )
					return true;

				if ( warp && warp.changed )
					return true;

				for ( int i = 0; i < checkCorners.Length; i++ )
				{
					Vector3 c = Map(-2, checkCorners[i]);
					float delta = (c - lastCorners[i]).sqrMagnitude;

					if ( delta > 0.0001f )
					{
						lastCorners[i] = c;
						return true;
					}
				}
			}
			else
			{
				if ( ModEnabled != lastEnabled )
				{
					lastEnabled = ModEnabled;
					lastCorners[0] = checkCorners[0] + Vector3.up;
					return true;
				}
			}

			return false;
		}

		public override bool AnyDeform(float epsilon = 0.0001f)
		{
			if ( ModEnabled )
			{
				for ( int i = 0; i < checkCorners.Length; i++ )
				{
					if ( prepared )
					{
						Vector3 c = Map(-1, checkCorners[i]);
						float delta = (c - checkCorners[i]).sqrMagnitude;

						if ( delta > epsilon )
							return true;
					}
				}
			}

			return false;
		}
	}
}