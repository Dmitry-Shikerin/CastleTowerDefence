using UnityEngine;

namespace MegaFiers
{
	public class MegaFFDWarp : MegaWarp
	{
		public float KnotSize = 0.1f;
		public bool inVol = false;
		public Vector3[] pt = new Vector3[64];
		[HideInInspector]
		public float EPSILON = 0.001f;
		[HideInInspector]
		public Vector3 lsize = Vector3.one;
		[HideInInspector]
		public Vector3 bsize = new Vector3();
		[HideInInspector]
		public Vector3 bcenter = new Vector3();

		public virtual int GridSize() { return 1; }
		public virtual int GridIndex(int i, int j, int k) { return 0; }
		public override string GetHelpURL() { return "?page_id=199"; }

		public float hw;
		public float hh;
		public float hl;

		public Vector3 LatticeSize()
		{
			bsize.x = Width;
			bsize.y = Height;
			bsize.z = Length;

			Vector3 size = bsize;
			if ( size.x == 0.0f ) size.x = 0.001f;
			if ( size.y == 0.0f ) size.y = 0.001f;
			if ( size.z == 0.0f ) size.z = 0.001f;
			return size;
		}

		public void Init()
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

		public override bool Prepare(float decay)
		{
			if ( bsize.x != Width || bsize.y != Height || bsize.z != Length )
				Init();

			Vector3 s = LatticeSize();

			for ( int i = 0; i < 3; i++ )
			{
				if ( s[i] == 0.0f )
					s[i] = 1.0f;
				else
					s[i] = 1.0f / s[i];
			}

			tm = transform.worldToLocalMatrix;
			Vector3 c = MegaMatrix.GetTrans(ref tm);

			MegaMatrix.SetTrans(ref tm, c - (-bsize * 0.5f));

			MegaMatrix.Scale(ref tm, s, false);

			invtm = tm.inverse;

			totaldecay = Decay + decay;
			if ( totaldecay < 0.0f )
				totaldecay = 0.0f;

			hw = Width * 0.5f;
			hh = Height * 0.5f;
			hl = Length * 0.5f;


			return true;
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
		}

		static MegaFFDWarp Create(GameObject go, int type)
		{
			switch ( type )
			{
				case 0: return go.AddComponent<MegaFFD2x2x2Warp>();
			}

			return null;
		}

		public override void DrawGizmo(Color col)
		{
		}
	}
}