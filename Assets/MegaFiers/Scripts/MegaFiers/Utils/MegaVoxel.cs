using UnityEngine;
using System.Collections.Generic;

namespace MegaFiers
{
	public class MegaVoxelNormals
	{
		public class Voxel_t
		{
			public List<int>	tris = new List<int>();

			public void AddTri(int tri)
			{
				if ( !tris.Contains(tri) )
					tris.Add(tri);
			}
		}

		public static void GetGridIndex(Vector3 p, out int x, out int y, out int z, float unit)
		{
			x = (int)((p.x - start.x) / unit);
			y = (int)((p.y - start.y) / unit);
			z = (int)((p.z - start.z) / unit);
		}

		public static Voxel_t[,,]	volume;
		public static int			width;
		public static int			height;
		public static int			depth;
		static Vector3				start;

		public static void Voxelize(Vector3[] vertices, int[] indices, Bounds bounds, int resolution, out float unit)
		{
			float maxLength = Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z));
			unit = maxLength / resolution;

			start = bounds.min;
			Vector3 size = bounds.size;

			width	= Mathf.CeilToInt(size.x / unit);
			height	= Mathf.CeilToInt(size.y / unit);
			depth	= Mathf.CeilToInt(size.z / unit);

			volume = new Voxel_t[width + 1, height + 1, depth + 1];
			Vector3 voxelSize = Vector3.one * unit;

			for ( int x = 0; x < width + 1; x++ )
			{
				for ( int y = 0; y < height + 1; y++ )
				{
					for ( int z = 0; z < depth + 1; z++ )
						volume[x, y, z] = new Voxel_t();
				}
			}

			int gx, gy, gz;

			for ( int i = 0, n = indices.Length; i < n; i += 3 )
			{
				GetGridIndex(vertices[indices[i]], out gx, out gy, out gz, unit);
				volume[gx, gy, gz].AddTri(i / 3);

				GetGridIndex(vertices[indices[i + 1]], out gx, out gy, out gz, unit);
				volume[gx, gy, gz].AddTri(i / 3);

				GetGridIndex(vertices[indices[i + 2]], out gx, out gy, out gz, unit);
				volume[gx, gy, gz].AddTri(i / 3);
			}
		}

		static public List<int> GetFaces(Vector3 p, float unit)
		{
			int gx, gy, gz;
			GetGridIndex(p, out gx, out gy, out gz, unit);
			return volume[gx, gy, gz].tris;
		}
	}

	public class MegaTriangle
	{
		public int		t;
		public Vector3	a, b, c;
		public Bounds	bounds;

		public MegaTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 dir, int t)
		{
			this.t = t;
			this.a = a;
			this.b = b;
			this.c = c;

			Vector3 min = Vector3.Min(Vector3.Min(a, b), c);
			Vector3 max = Vector3.Max(Vector3.Max(a, b), c);
			bounds.SetMinMax(min, max);
		}

		public void Barycentric(Vector3 p, out float u, out float v, out float w)
		{
			Vector3 v0 = b - a, v1 = c - a, v2 = p - a;
			float d00 = Vector3.Dot(v0, v0);
			float d01 = Vector3.Dot(v0, v1);
			float d11 = Vector3.Dot(v1, v1);
			float d20 = Vector3.Dot(v2, v0);
			float d21 = Vector3.Dot(v2, v1);
			float denom = 1f / (d00 * d11 - d01 * d01);
			v = (d11 * d20 - d01 * d21) * denom;
			w = (d00 * d21 - d01 * d20) * denom;
			u = 1.0f - v - w;
		}
	}

	public class MegaVoxel
	{
		public class Voxel_t
		{
			public Vector3 position;
			public List<MegaTriangle> tris;

			public Voxel_t()
			{
				position = Vector3.zero;
				tris = new List<MegaTriangle>();
			}
		}

		public static void GetGridIndex(Vector3 p, out int x, out int y, out int z, float unit)
		{
			x = (int)((p.x - start.x) / unit);
			y = (int)((p.y - start.y) / unit);
			z = (int)((p.z - start.z) / unit);
		}

		public static Voxel_t[,,]	volume;
		public static int			width;
		public static int			height;
		public static int			depth;
		static Vector3				start;

		public static void Voxelize(Vector3[] vertices, int[] indices, Bounds bounds, int resolution, out float unit)
		{
			float maxLength = Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z));
			unit = maxLength / resolution;
			float hunit = unit * 0.5f;

			Vector3 hv = new Vector3(hunit, hunit, hunit);

			start = bounds.min - new Vector3(hunit, hunit, hunit);
			Vector3 end = bounds.max + new Vector3(hunit, hunit, hunit);
			Vector3 size = end - start;

			width = Mathf.CeilToInt(size.x / unit);
			height = Mathf.CeilToInt(size.y / unit);
			depth = Mathf.CeilToInt(size.z / unit);

			volume = new Voxel_t[width, height, depth];
			Bounds[,,] boxes = new Bounds[width, height, depth];
			Vector3 voxelSize = Vector3.one * unit;

			for ( int x = 0; x < width; x++ )
			{
				for ( int y = 0; y < height; y++ )
				{
					for ( int z = 0; z < depth; z++ )
					{
						Vector3 p = new Vector3(x, y, z) * unit + start + hv;
						Bounds aabb = new Bounds(p, voxelSize);
						boxes[x, y, z] = aabb;
						volume[x, y, z] = new Voxel_t();
					}
				}
			}

			Vector3 direction = Vector3.forward;

			for ( int i = 0, n = indices.Length; i < n; i += 3 )
			{
				MegaTriangle tri = new MegaTriangle(vertices[indices[i]], vertices[indices[i + 1]], vertices[indices[i + 2]], direction, i);

				Vector3 min = tri.bounds.min - start;
				Vector3 max = tri.bounds.max - start;
				int iminX = (int)(min.x / unit), iminY = (int)(min.y / unit), iminZ = (int)(min.z / unit);
				int imaxX = (int)(max.x / unit), imaxY = (int)(max.y / unit), imaxZ = (int)(max.z / unit);

				iminX = Mathf.Clamp(iminX, 0, width - 1);
				iminY = Mathf.Clamp(iminY, 0, height - 1);
				iminZ = Mathf.Clamp(iminZ, 0, depth - 1);
				imaxX = Mathf.Clamp(imaxX, 0, width - 1);
				imaxY = Mathf.Clamp(imaxY, 0, height - 1);
				imaxZ = Mathf.Clamp(imaxZ, 0, depth - 1);

				for ( int x = iminX; x <= imaxX; x++ )
				{
					for ( int y = iminY; y <= imaxY; y++ )
					{
						for ( int z = iminZ; z <= imaxZ; z++ )
						{
							if ( Intersects(tri, boxes[x, y, z]) )
							{
								Voxel_t voxel = volume[x, y, z];
								voxel.position = boxes[x, y, z].center;
								voxel.tris.Add(tri);
								volume[x, y, z] = voxel;
							}
						}
					}
				}
			}
		}

		public static bool Intersects(MegaTriangle tri, Bounds aabb)
		{
			//return IntersectsOld(tri, aabb);
			return aabb.Intersects(tri.bounds);
		}

		public static bool IntersectsOld(MegaTriangle tri, Bounds aabb)
		{
			Vector3 center = aabb.center, extents = aabb.max - center;

			Vector3 v0 = tri.a - center,
				v1 = tri.b - center,
				v2 = tri.c - center;

			Vector3 f0 = v1 - v0,
				f1 = v2 - v1,
				f2 = v0 - v2;

			Vector3 a00 = new Vector3(0, -f0.z, f0.y),
				a01 = new Vector3(0, -f1.z, f1.y),
				a02 = new Vector3(0, -f2.z, f2.y),
				a10 = new Vector3(f0.z, 0, -f0.x),
				a11 = new Vector3(f1.z, 0, -f1.x),
				a12 = new Vector3(f2.z, 0, -f2.x),
				a20 = new Vector3(-f0.y, f0.x, 0),
				a21 = new Vector3(-f1.y, f1.x, 0),
				a22 = new Vector3(-f2.y, f2.x, 0);

			// Test axis a00
			float p0 = Vector3.Dot(v0, a00);
			float p1 = Vector3.Dot(v1, a00);
			float p2 = Vector3.Dot(v2, a00);
			float r = extents.y * Mathf.Abs(f0.z) + extents.z * Mathf.Abs(f0.y);

			if ( Mathf.Max(-Mathf.Max(p0, p1, p2), Mathf.Min(p0, p1, p2)) > r )
				return false;

			// Test axis a01
			p0 = Vector3.Dot(v0, a01);
			p1 = Vector3.Dot(v1, a01);
			p2 = Vector3.Dot(v2, a01);
			r = extents.y * Mathf.Abs(f1.z) + extents.z * Mathf.Abs(f1.y);

			if ( Mathf.Max(-Mathf.Max(p0, p1, p2), Mathf.Min(p0, p1, p2)) > r )
				return false;

			// Test axis a02
			p0 = Vector3.Dot(v0, a02);
			p1 = Vector3.Dot(v1, a02);
			p2 = Vector3.Dot(v2, a02);
			r = extents.y * Mathf.Abs(f2.z) + extents.z * Mathf.Abs(f2.y);

			if ( Mathf.Max(-Mathf.Max(p0, p1, p2), Mathf.Min(p0, p1, p2)) > r )
				return false;

			// Test axis a10
			p0 = Vector3.Dot(v0, a10);
			p1 = Vector3.Dot(v1, a10);
			p2 = Vector3.Dot(v2, a10);
			r = extents.x * Mathf.Abs(f0.z) + extents.z * Mathf.Abs(f0.x);
			if ( Mathf.Max(-Mathf.Max(p0, p1, p2), Mathf.Min(p0, p1, p2)) > r )
				return false;

			// Test axis a11
			p0 = Vector3.Dot(v0, a11);
			p1 = Vector3.Dot(v1, a11);
			p2 = Vector3.Dot(v2, a11);
			r = extents.x * Mathf.Abs(f1.z) + extents.z * Mathf.Abs(f1.x);

			if ( Mathf.Max(-Mathf.Max(p0, p1, p2), Mathf.Min(p0, p1, p2)) > r )
				return false;

			// Test axis a12
			p0 = Vector3.Dot(v0, a12);
			p1 = Vector3.Dot(v1, a12);
			p2 = Vector3.Dot(v2, a12);
			r = extents.x * Mathf.Abs(f2.z) + extents.z * Mathf.Abs(f2.x);

			if ( Mathf.Max(-Mathf.Max(p0, p1, p2), Mathf.Min(p0, p1, p2)) > r )
				return false;

			// Test axis a20
			p0 = Vector3.Dot(v0, a20);
			p1 = Vector3.Dot(v1, a20);
			p2 = Vector3.Dot(v2, a20);
			r = extents.x * Mathf.Abs(f0.y) + extents.y * Mathf.Abs(f0.x);

			if ( Mathf.Max(-Mathf.Max(p0, p1, p2), Mathf.Min(p0, p1, p2)) > r )
				return false;

			// Test axis a21
			p0 = Vector3.Dot(v0, a21);
			p1 = Vector3.Dot(v1, a21);
			p2 = Vector3.Dot(v2, a21);
			r = extents.x * Mathf.Abs(f1.y) + extents.y * Mathf.Abs(f1.x);

			if ( Mathf.Max(-Mathf.Max(p0, p1, p2), Mathf.Min(p0, p1, p2)) > r )
				return false;

			// Test axis a22
			p0 = Vector3.Dot(v0, a22);
			p1 = Vector3.Dot(v1, a22);
			p2 = Vector3.Dot(v2, a22);
			r = extents.x * Mathf.Abs(f2.y) + extents.y * Mathf.Abs(f2.x);

			if ( Mathf.Max(-Mathf.Max(p0, p1, p2), Mathf.Min(p0, p1, p2)) > r )
				return false;

			if ( Mathf.Max(v0.x, v1.x, v2.x) < -extents.x || Mathf.Min(v0.x, v1.x, v2.x) > extents.x )
				return false;

			if ( Mathf.Max(v0.y, v1.y, v2.y) < -extents.y || Mathf.Min(v0.y, v1.y, v2.y) > extents.y )
				return false;

			if ( Mathf.Max(v0.z, v1.z, v2.z) < -extents.z || Mathf.Min(v0.z, v1.z, v2.z) > extents.z )
				return false;

			Vector3 normal = Vector3.Cross(f1, f0).normalized;
			Plane pl = new Plane(normal, Vector3.Dot(normal, tri.a));
			return Intersects(pl, aabb);
		}

		public static bool Intersects(Plane pl, Bounds aabb)
		{
			Vector3 center = aabb.center;
			Vector3 extents = aabb.max - center;

			float r = extents.x * Mathf.Abs(pl.normal.x) + extents.y * Mathf.Abs(pl.normal.y) + extents.z * Mathf.Abs(pl.normal.z);
			float s = Vector3.Dot(pl.normal, center) - pl.distance;

			return Mathf.Abs(s) <= r;
		}
	}
}