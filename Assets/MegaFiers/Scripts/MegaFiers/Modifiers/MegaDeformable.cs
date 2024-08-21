using UnityEngine;

namespace MegaFiers
{
	// Beta modifier
	[AddComponentMenu("Modifiers/Deformable")]
	public class MegaDeformable : MegaModifier
	{
		[Adjust]
		public float			Hardness			= 0.5f; // Impact resistance to calculate amount of deformation applied to the mesh	
		[Adjust]
		public bool				DeformMeshCollider	= true; // Configure if the mesh at collider must also be deformed (only works if a MeshCollider is in use)
		[Adjust]
		public float			UpdateFrequency		= 0.0f; // Configure the mesh (and mesh collider) update frequency in times per second. 0 for real time (high CPU usage)
		[Adjust]
		public float			MaxVertexMov		= 0.0f;	// Maximum movement allowed for a vertex from its original position (0 means no limit)
		public Color32			DeformedVertexColor	= Color.gray;	// Color to be applied at deformed vertices (only works for shaders that handle vertices colors)
		public Texture2D		HardnessMap;    // Texture to modulate maximum movement allowed (uses alpha channel)
		[Adjust]
		public bool				usedecay			= false;
		[Adjust]
		public float			decay				= 0.999f;
		public Color[]			baseColors; // Backup of original mesh vertices colors
		[Adjust]
		public float			sizeFactor;	// Size factor of mesh
		public float[]			map;
		public Vector3[]		offsets;
		[Adjust]
		public float			impactFactor		= 0.1f;
		[Adjust]
		public float			ColForce			= 0.5f;
		public MegaModifyObject	modobj;
		ContactPoint[]			colpoints;

		public override string ModName() { return "Deformable"; }
		public override MegaModChannel ChannelsReq() { return MegaModChannel.Verts | MegaModChannel.Col; }
		public override MegaModChannel ChannelsChanged() { return MegaModChannel.Verts | MegaModChannel.Col; }

		public override void ModStart(MegaModifyObject mc)
		{
			Vector3 s = mc.mesh.bounds.size;
			sizeFactor = Mathf.Min(s.x, s.y, s.z);

			if ( mc.mesh.colors != null )
				baseColors = mc.mesh.colors;

			LoadMap(mc);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( offsets == null || offsets.Length != mc.mod.jverts.Length )
				offsets = new Vector3[mc.mod.jverts.Length];

			return true;
		}

		private void LoadMesh()
		{
		}

		public void LoadMap()
		{
			MegaModifyObject mc = GetComponent<MegaModifyObject>();
			if ( mc )
			{
				LoadMap(mc);
			}
		}

		private void LoadMap(MegaModifyObject mc)
		{
			// Load hardness map
			if ( HardnessMap )
			{
				Vector2[] uvs = mc.mesh.uv;
				map = new float[uvs.Length];

				for ( int c = 0; c < uvs.Length; c++ )
				{
					Vector2 uv = uvs[c];
					map[c] = HardnessMap.GetPixelBilinear(uv.x, uv.y).a;
				}
			}
			else
				map = null;
		}

		public override void Modify(MegaModifyObject mc)
		{
			// Calc collision force	
			float colForce = ColForce;

			sizeFactor = mc.bbox.size.magnitude;
			// distFactor is the amount of deforming in local coordinates
			float distFactor = colForce * (sizeFactor * (impactFactor / Mathf.Max(impactFactor, Hardness)));

			// Deform process
			if ( colpoints != null )
			{
				for ( int c = 0; c < colpoints.Length; c++ )
				{
					ContactPoint contact = colpoints[c];

					for ( int i = 0; i < verts.Length; i++ )
					{
						// Apply deformation only on vertex inside "blast area"
						Vector3 vp = verts[i] + offsets[i];

						Vector3 p = transform.InverseTransformPoint(contact.point);
						float d  = (vp - p).sqrMagnitude;
						if ( d <= distFactor )
						{
							// Deformation is the collision normal with local deforming ratio Vertices near the impact point must also be more deformed
							Vector3 n = transform.InverseTransformDirection(contact.normal * (1.0f - (d / distFactor)) * distFactor);

							// Apply hardness map if any
							if ( map != null )
								n *= 1.0f - map[i];

							// Deform vertex
							offsets[i] += n;

							// Apply max vertex movement if configured
							// Here the deformed vertex position is just scaled down to keep the best deformation while respecting limits
							if ( MaxVertexMov > 0.0f )
							{
								float max = MaxVertexMov;
								n = offsets[i];
								d = n.magnitude;
								if ( d > max )
									offsets[i] = (n * (max / d));

								// Apply vertex color deformation
								// Deform color is applied in proportional amount with vertex distance and max deform
								if ( baseColors.Length > 0 )
								{
									d = (d / MaxVertexMov);
									mc.cols[i] = Color.Lerp(baseColors[i], DeformedVertexColor, d);
								}
							}
							else
							{
								if ( mc.cols.Length > 0 )
									mc.cols[i] = Color.Lerp(baseColors[i], DeformedVertexColor, offsets[i].magnitude / (distFactor * 10.0f));
							}
						}
					}
				}
			}

			colpoints = null;

			if ( !usedecay )
			{
				for ( int i = 0; i < jverts.Length; i++ )
					jsverts[i] = jverts[i] + offsets[i];
			}
			else
			{
				for ( int i = 0; i < jverts.Length; i++ )
				{
					offsets[i].x *= decay;
					offsets[i].y *= decay;
					offsets[i].z *= decay;

					jsverts[i] = jverts[i] + offsets[i];
				}
			}
		}

		// We could find the megamod comp
		public void Repair(float repair, MegaModifyObject mc)
		{
			Repair(repair, Vector3.zero, 0.0f, mc);
		}

		public void Repair(float repair, Vector3 point, float radius, MegaModifyObject mc)
		{
			// Check mesh assigned
			if ( (!mc.mesh) )
				return;

			// Transform world point to mesh local
			point = transform.InverseTransformPoint(point);

			// Repairing is done returning vertices position and color to original positions by requested amount
			for ( int i = 0; i < mc.jverts.Length; i++ )
			{
				// Check for repair inside radius
				if ( radius > 0.0f )
				{
					if ( (point - mc.jsverts[i]).magnitude >= radius )
						continue;
				}

				// Repair
				Vector3 n = offsets[i];
				offsets[i] = n * (1.0f - repair);
				if ( baseColors.Length > 0 )
					mc.cols[i] = Color.Lerp(mc.cols[i], baseColors[i], repair);
			}
		}

		void OnCollisionEnter(Collision collision)
		{
			if ( modobj == null )
				modobj = GetComponent<MegaModifyObject>();

			if ( modobj )
				modobj.Enabled = true;

			colpoints = collision.contacts;
		}

		void OnCollisionStay(Collision collision)
		{
			colpoints = collision.contacts;
		}

		void OnCollisionExit(Collision collision)
		{
			if ( modobj )
			{
				if ( !usedecay )
					modobj.Enabled = false;
			}
		}
	}
}