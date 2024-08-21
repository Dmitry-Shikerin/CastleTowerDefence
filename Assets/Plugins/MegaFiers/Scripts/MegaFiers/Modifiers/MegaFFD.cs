using UnityEngine;

namespace MegaFiers
{
	public class MegaFFD : MegaModifier
	{
		public enum ShowIndex
		{
			None,
			Index,
			XYZ,
		}
		[Adjust]
		public float			KnotSize		= 0.1f;
		[Adjust]
		public bool				inVol			= false;
		public Vector3[]		pt				= new Vector3[64];
		protected Vector3[]		lastpt			= new Vector3[64];
		[HideInInspector]
		public float			EPSILON			= 0.001f;
		public Vector3			lsize			= Vector3.one;
		public Vector3			bsize			= new Vector3();
		public Vector3			bcenter			= new Vector3();
		public ShowIndex		showIndex		= ShowIndex.None;

		public virtual int		GridSize()		{ return 1; }
		public virtual int		GridIndex(int i, int j, int k)	{ return 0; }
		public virtual void		GridXYZ(int index, out int x, out int y, out int z) { x = 0; y = 0; z = 0; }
		public override string	GetHelpURL()	{ return "?page_id=199"; }
		public virtual int		NumPoints()		{ int c = GridSize(); return c * c * c; }

		public override bool AnyDeform(float epsilon = 0.0001f)
		{
			int size = GridSize();
			float fsize = size - 1.0f;

			for ( int i = 0; i < size; i++ )
			{
				float isz = (float)(i) / fsize;

				for ( int j = 0; j < size; j++ )
				{
					float jsz = (float)(j) / fsize;

					for ( int k = 0; k < size; k++ )
					{
						float ksz = (float)(k) / fsize;

						int c = GridIndex(i, j, k);
						if ( Mathf.Abs(pt[c].x - isz) > epsilon )
							return true;
						if ( Mathf.Abs(pt[c].y - jsz) > epsilon )
							return true;
						if ( Mathf.Abs(pt[c].z - ksz) > epsilon )
							return true;
					}
				}
			}

			return false;
		}

		public void SetUpdate()
		{
			lastpt[0].x += 0.1f;
		}

		public override bool Changed()
		{
			if ( ModEnabled )
			{
				lastEnabled = ModEnabled;
				if ( alwaysChanging )
					return true;

				int size = GridSize();
				for ( int i = 0; i < size; i++ )
				{
					for ( int j = 0; j < size; j++ )
					{
						for ( int k = 0; k < size; k++ )
						{
							int c = GridIndex(i, j, k);

							if ( pt[c] != lastpt[c] )
							{
								lastpt[c] = pt[c];
								return true;
							}
						}
					}
				}
			}
			else
			{
				if ( ModEnabled != lastEnabled )
				{
					lastEnabled = ModEnabled;
					lastpt[0] = pt[0] + Vector3.up;
					return true;
				}
			}

			return false;
		}

		public override void PostCopy(MegaModifier src)
		{
			MegaFFD ffd = (MegaFFD)src;

			pt = new Vector3[64];

			for ( int c = 0; c < 64; c++ )
				pt[c] = ffd.pt[c];
		}

		public Vector3 LatticeSize()
		{
			Vector3 size = bsize;
			if ( size.x == 0.0f ) size.x = 0.001f;
			if ( size.y == 0.0f ) size.y = 0.001f;
			if ( size.z == 0.0f ) size.z = 0.001f;
			return size;
		}

		[ContextMenu("Reset Lattice")]
		public void ResetLattice()
		{
			Init();
		}

		void Init()
		{
			lsize = LatticeSize();

			int size = GridSize();
			float fsize = size - 1.0f;

			for ( int i = 0; i < size; i++ )	
			{
				for ( int j = 0; j < size; j++ )
				{
					for ( int k = 0; k < size; k++ )
					{
						int c = GridIndex(i, j, k);
						pt[c].x = (float)(i) / fsize;
						pt[c].y = (float)(j) / fsize;
						pt[c].z = (float)(k) / fsize;
					}
				}
			}
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			Vector3 s = LatticeSize();

			for ( int i = 0; i < 3; i++ )
			{
				if ( s[i] == 0.0f )
					s[i] = 1.0f;
				else
					s[i] = 1.0f / s[i];
			}

			Vector3 c = MegaMatrix.GetTrans(ref tm);
			MegaMatrix.SetTrans(ref tm, c - bbox.min - Offset);
			MegaMatrix.Scale(ref tm, s, false);

			invtm = tm.inverse;

			return true;
		}

		public Vector3 GetPointWorld(int i, int j, int k)
		{
			Matrix4x4 tm1 = transform.localToWorldMatrix * Matrix4x4.TRS(-(gizmoPos + Offset), Quaternion.Euler(gizmoRot), gizmoScale);
			return tm1.MultiplyPoint3x4(GetPoint(i, j, k));
		}

		public Vector3 GetPointWorld(int i)
		{
			Matrix4x4 tm1 = transform.localToWorldMatrix * Matrix4x4.TRS(-(gizmoPos + Offset), Quaternion.Euler(gizmoRot), gizmoScale);
			return tm1.MultiplyPoint3x4(GetPoint(i));
		}

		public void SetPointWorld(int i, Vector3 p)
		{
			Matrix4x4 tm1 = transform.localToWorldMatrix * Matrix4x4.TRS(-(gizmoPos + Offset), Quaternion.Euler(gizmoRot), gizmoScale);
			SetPointLocal(i, tm1.inverse.MultiplyPoint3x4(p));
		}

		public void SetPointWorld(int i, int j, int k, Vector3 p)
		{
			Matrix4x4 tm1 = transform.localToWorldMatrix * Matrix4x4.TRS(-(gizmoPos + Offset), Quaternion.Euler(gizmoRot), gizmoScale);
			SetPointLocal(i, j, k, tm1.inverse.MultiplyPoint3x4(p));
		}

		public Vector3 GetPoint(int i)
		{
			Vector3 p = pt[i];

			p.x -= 0.5f;
			p.y -= 0.5f;
			p.z -= 0.5f;

			return Vector3.Scale(p, lsize) + bcenter;
		}

		public Vector3 GetPoint(int i, int j, int k)
		{
			Vector3 p = pt[GridIndex(i, j, k)];

			p.x -= 0.5f;
			p.y -= 0.5f;
			p.z -= 0.5f;

			return Vector3.Scale(p, lsize) + bcenter;
		}

		public void SetPoint(int i, int j, int k, Vector3 pos)
		{
			Vector3 lpos = transform.worldToLocalMatrix.MultiplyPoint(pos);
			SetPointLocal(i, j, k, lpos);
		}

		public void SetPointLocal(int i, int j, int k, Vector3 lpos)
		{
			Vector3 size = lsize;
			Vector3 osize = lsize;
			osize.x = 1.0f / size.x;
			osize.y = 1.0f / size.y;
			osize.z = 1.0f / size.z;

			lpos -= bcenter;
			Vector3 p = Vector3.Scale(lpos, osize);
			p.x += 0.5f;
			p.y += 0.5f;
			p.z += 0.5f;

			pt[GridIndex(i, j, k)] = p;
		}

		public void SetPoint(int index, Vector3 pos)
		{
			Vector3 lpos = transform.worldToLocalMatrix.MultiplyPoint(pos);
			SetPointLocal(index, lpos);
		}

		public void SetPointLocal(int index, Vector3 lpos)
		{
			Vector3 size = lsize;
			Vector3 osize = lsize;
			osize.x = 1.0f / size.x;
			osize.y = 1.0f / size.y;
			osize.z = 1.0f / size.z;

			lpos -= bcenter;
			Vector3 p = Vector3.Scale(lpos, osize);
			p.x += 0.5f;
			p.y += 0.5f;
			p.z += 0.5f;

			pt[index] = p;
		}

		public void MovePoint(int x, int y, int z, Vector3 localmove)
		{
			Vector3 p = GetPoint(x, y, z);
			p += localmove;
			SetPointLocal(x, y, z, p);
		}

		void Reset()
		{
			MegaModifyObject modobj = (MegaModifyObject)gameObject.GetComponent<MegaModifyObject>();

			if ( modobj != null )
				modobj.ModReset(this);

			Renderer rend = GetComponent<Renderer>();

			if ( rend != null )
			{
				Mesh ms = MegaUtils.GetSharedMesh(gameObject);

				if ( ms != null )
				{
					Bounds b = ms.bounds;
					Offset = -b.center;
					bbox.min = b.center - b.extents;
					bbox.max = b.center + b.extents;
				}
			}

			if ( modobj.selection != null )
			{
				Bounds bb = new Bounds();
				for ( int i = 0; i < modobj.jverts.Length; i++ )
				{
					if ( modobj.selection[i] > 0.001f )
						bb.Encapsulate(modobj.jverts[i]);
				}

				Offset = -bb.center;
				bbox.min = bb.center - bb.extents;
				bbox.max = bb.center + bb.extents;
			}

			bsize = bbox.Size();
			bcenter = bbox.center;

			Init();
		}

		[ContextMenu("Fit FFD to Selection")]
		public void FitFFD()
		{
			Reset();
		}

		[ContextMenu("Fit FFD to Mesh")]
		public void FitFFDToMesh()
		{
			Renderer rend = GetComponent<Renderer>();

			if ( rend != null )
			{
				Mesh ms = MegaUtils.GetSharedMesh(gameObject);

				if ( ms != null )
				{
					Bounds b = ms.bounds;
					Offset = -b.center;
					bbox.min = b.center - b.extents;
					bbox.max = b.center + b.extents;
				}
			}

			bsize = bbox.Size();
			bcenter = bbox.center;
			Init();
		}

		[ContextMenu("Fit FFD to Group")]
		public void FitFFDToGroup()
		{
			MegaModifyObject mobj = GetComponent<MegaModifyObject>();

			if ( mobj )
			{
				Bounds b = mobj.bbox;

				for ( int i = 0; i < mobj.group.Count; i++ )
				{
					MegaModifyObject gobj = mobj.group[i];

					if ( gobj )
					{
						for ( int j = 0; j < gobj.jverts.Length; j++ )
						{
							Vector3 v = gobj.transform.TransformPoint(gobj.jverts[j]);
							v = transform.InverseTransformPoint(v);
							b.Encapsulate(v);
						}
					}
				}

				Offset = -b.center;
				bbox.min = b.center - b.extents;
				bbox.max = b.center + b.extents;

				bsize = bbox.Size();
				bcenter = bbox.center;
				Init();
			}
		}

		public override bool InitMod(MegaModifyObject mc)
		{
			bsize = mc.bbox.size;
			bcenter = mc.bbox.center;
			Init();
			return true;
		}

		static MegaFFD Create(GameObject go, int type)
		{
			switch ( type )
			{
				case 0: return go.AddComponent<MegaFFD2x2x2>();
				case 1: return go.AddComponent<MegaFFD3x3x3>();
				case 2: return go.AddComponent<MegaFFD4x4x4>();
			}

			return null;
		}

		public override void DrawGizmo(MegaModContext context)
		{
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaFFD mod = root as MegaFFD;

			if ( mod )
			{
				for ( int i = 0; i < mod.pt.Length; i++ )
					pt[i] = mod.pt[i];

				bsize		= mod.bsize;
				lsize		= mod.lsize;
				bcenter		= mod.bcenter;
				bbox		= mod.bbox;
				inVol		= mod.inVol;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
			}
		}
	}
}