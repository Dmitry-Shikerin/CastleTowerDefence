using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using Unity.Jobs;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

// 0.72ms for 625 model bend 9.28ms old version 12.9 times faster 44365 verts
namespace MegaFiers
{
	public struct MegaModContext
	{
		public MegaBox3			bbox;
		public Vector3			Offset;
		public MegaModifyObject	mod;
		public GameObject		go;
		public int				modIndex;
		public bool				gizmoPrepare;
	}

	public enum MegaNormalMethod
	{
		Unity,
		Mega,
		None,
	}

	public enum MegaTangentMethod
	{
		None,
		Unity,
		Mega,
	}

	[System.Serializable]
	public class MegaNormMap
	{
		public int[] faces;
	}

	public enum MegaUpdateMode
	{
		Update,
		LateUpdate,
		OnRender,
	}

	public enum MegaColor
	{
		Red		= 0,
		Green	= 1,
		Blue	= 2,
		Alpha	= 3,
	}

	[AddComponentMenu("Modifiers/Modify Object")]
	[ExecuteInEditMode]
	public class MegaModifyObject : MonoBehaviour
	{
		public Mesh						originalMesh;
		[HideInInspector]
		public Mesh						cachedMesh;		// The original mesh for this object
		public bool						InvisibleUpdate	= false;
		[HideInInspector]
		public Bounds					bbox			= new Bounds();
		public MegaNormalMethod			NormalMethod	= MegaNormalMethod.Mega;
		public MegaTangentMethod		TangentMethod	= MegaTangentMethod.None;
		public bool						recalcbounds	= false;
		public bool						recalcCollider	= false;
		public bool						dynamicMesh		= false;
		public bool						Enabled			= true;
		public MegaUpdateMode			UpdateMode		= MegaUpdateMode.LateUpdate;
		public bool						DrawGizmos		= true;
		[System.NonSerialized]
		Vector2[]						uvs;
		[System.NonSerialized]
		Vector2[]						suvs;
		[HideInInspector]
		public Mesh						mesh;
		public MegaModifier[]			mods			= null;
		[HideInInspector]
		public int						UpdateMesh		= 0;
		[HideInInspector]
		public MegaModChannel			dirtyChannels;
		[HideInInspector]
		public GameObject				sourceObj;
		[System.NonSerialized]
		public NativeArray<Color>		cols;
		[System.NonSerialized]
		public float[]					selection;
		public static bool				GlobalDisplay	= true;
		public MegaModContext			modContext		= new MegaModContext();
		public MegaNormMap[]			mapping;
		[System.NonSerialized]
		int[]							tris;
		[System.NonSerialized]
		Vector3[]						norms;
		[Unity.Collections.ReadOnly][System.NonSerialized]
		public NativeArray<Vector3>		startverts;
		[System.NonSerialized]
		public NativeArray<Vector3>		jverts;
		[System.NonSerialized]
		public NativeArray<Vector3>		jsverts;
		[System.NonSerialized]
		public NativeArray<Vector3>		facenormals;
		[System.NonSerialized]
		public NativeArray<Vector3>		normals;
		[Unity.Collections.ReadOnly][System.NonSerialized]
		public NativeArray<int>			faceTris;
		[Unity.Collections.ReadOnly][System.NonSerialized]
		public NativeArray<int>			faceCount;
		[Unity.Collections.ReadOnly][System.NonSerialized]
		public NativeArray<int>			newMapping;
		bool							visible			= true;
		public int						restorekeep		= 0;
		bool							JVertsSource	= false;
		bool							UVsSource		= false;
		MeshCollider					meshCol			= null;
		Vector4[]						tangents;
		Vector3[]						tan1;
		Vector3[]						tan2;
		FaceNormalsJob					faceNormsJob;
		JobHandle						faceNormsJobHandle;
		CalcNormalsJob					calcNormsJob;
		JobHandle						calcNormsJobHandle;
		CalcBoundsJob					calcBoundsJob;
		JobHandle						calcBoundsJobHandle;
		VertexBlendColJob				vertexBlendColJob;
		JobHandle						vertexBlendColHandle;
		VertexBlendJob					vertexBlendJob;
		JobHandle						vertexBlendHandle;
		VertexBlendBoneJob				vertexBlendBoneJob;
		JobHandle						vertexBlendBoneHandle;
		bool playing;
		public List<int>				indexmap		= new List<int>();
		public List<int>				newmap			= new List<int>();
		public bool						autoDisable		= true;
		public bool						noUpdate;
		public bool						showGroup;
		public bool						groupEnabled	= true;
		public List<MegaModifyObject>	group			= new List<MegaModifyObject>();
		public MegaModifyObject			colliderObject;
		public TMP_Text					tmproObj;
		TMP_MeshInfo[]					tmpMeshInfo;
		public bool						showParams;
		public bool						quickEdit		= true;
		public int						quickEditSize	= 200;
		int								lastUpdateFrame;
		public bool						inGroup			= false;
		SkinnedMeshRenderer				sr;
		MeshFilter						mf;
		public int						batchCount		= 64;
		public Vector2					scrollpos;
		public bool						useColSection	= false;
		public float					selectionWeight	= 1.0f;
		public MegaColor				selectChannel	= MegaColor.Red;
		public Texture2D				mask;
		float							lastWeight;
		bool							lastUseCol;
		MegaColor						lastSelectChannel;
		Texture2D						lastMask;
		public bool						noJobs			= false;
		public bool						changed;
		public string					blendShapeName	= " New Blend Shape";
		public float					blendShapeWeight	= 0.0f;
		public bool						showBlendShapes	= false;
		public Vector2					blendScrollpos;
		public int						blendIndex;
		public int						subDivide		= 0;
		//public bool						useDefSkin		= true;
		//MegaDeformableSkin				defSkin;

		[System.NonSerialized]
		public NativeArray<float> weights;

		public Vector2[]				Uvs
		{
			get { return uvs; }
			set { uvs = value; }
		}
		public int[]					Tris
		{
			get { return tris; }
			set { tris = value; }
		}
		public Vector3[]				Norms
		{
			get { return norms; }
			set { norms = value; }
		}

		public bool IsSkinned()
		{
			if ( sr )
				return true;

			return false;
		}

		private void Awake()
		{
			DisposeArrays();
			SetUpdate();
			noUpdate = false;
		}

		public MegaDeformableSkin	defSkin;

		void Start()
		{
			if ( !originalMesh )
			{
				GameObject sourceObj;

				tmproObj = GetComponent<TMP_Text>();
				if ( !tmproObj )
					originalMesh = MegaUtils.FindMesh(gameObject, out sourceObj);
				else
				{
					GetTMProMesh(true);
					dynamicMesh = true;
				}
			}
			if ( dynamicMesh )
				cachedMesh = null;

			sr = GetComponent<SkinnedMeshRenderer>();

			mf = GetComponent<MeshFilter>();

			if ( Application.platform == RuntimePlatform.WebGLPlayer && !Application.isEditor )
			{
				noJobs = true;	// Turn off jobs in built webgl as has no support for jobs or threading
			}

			//defSkin = GetComponent<MegaDeformableSkin>();
			//autoDisable = true;
			//noJobs = false;
		}

		static public void CreateDeformableSkin(GameObject obj)
		{
			if ( obj )
			{
				SkinnedMeshRenderer sr = obj.GetComponent<SkinnedMeshRenderer>();
				if ( sr )
				{
					GameObject sobj = new GameObject();
					sobj.name = obj.name + " - DefSkin";
					sobj.transform.parent = obj.transform;
					sobj.transform.localPosition = Vector3.zero;
					sobj.transform.localRotation = Quaternion.identity;
					//sobj.transform.localScale = Vector3.one;
					//sobj.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideAndDontSave;

					sobj.AddComponent<MegaDeformableSkin>();
				}
			}
		}


		bool TextChanged()
		{
			if ( tmproObj )
			{
				if ( tmproObj.havePropertiesChanged )
				{
					dynamicMesh = true;
					return true;
				}
			}

			return false;
		}

		public void ForceUpdate()
		{
			lastUpdateFrame = Time.frameCount - 1;
		}

		void Update()
		{
			if ( noUpdate )
				return;

			TextChanged();

			if ( dynamicMesh )
			{
				DisposeArrays();

				if ( tmproObj )
				{
					tmproObj.ForceMeshUpdate(true, true);
					tmpMeshInfo = tmproObj.textInfo.CopyMeshInfoVertexData();
				}
				else
					MeshChanged();

				dynamicMesh = false;

				if ( mods != null )
				{
					for ( int i = 0; i < mods.Length; i++ )
						mods[i].ResetCorners();
				}
			}

			if ( groupEnabled && group.Count > 0 )
			{
				for ( int g = 0; g < group.Count; g++ )
				{
					MegaModifyObject modobj = group[g];

					if ( modobj )
					{
						modobj.InvisibleUpdate	= InvisibleUpdate;
						modobj.NormalMethod		= NormalMethod;
						modobj.TangentMethod	= TangentMethod;
						modobj.recalcCollider	= recalcCollider;
						modobj.recalcbounds		= recalcbounds;
						modobj.UpdateMode		= UpdateMode;
						modobj.DrawGizmos		= DrawGizmos;
						modobj.useColSection	= useColSection;
						modobj.mask				= mask;
						modobj.selectionWeight	= selectionWeight;

						for ( int m = 0; m < mods.Length; m++ )
						{
							MegaModifier mod = mods[m];
							modobj.mods[m].ModEnabled = mod.ModEnabled;
							modobj.mods[m].GroupParams(mod);
						}
					}
				}
			}

			if ( colliderObject )
			{
				colliderObject.InvisibleUpdate	= InvisibleUpdate;
				colliderObject.NormalMethod		= NormalMethod;
				colliderObject.TangentMethod	= TangentMethod;
				colliderObject.recalcbounds		= recalcbounds;
				colliderObject.UpdateMode		= UpdateMode;
				colliderObject.DrawGizmos		= DrawGizmos;
				colliderObject.useColSection	= useColSection;
				colliderObject.mask				= mask;
				colliderObject.selectionWeight	= selectionWeight;

				for ( int m = 0; m < mods.Length; m++ )
				{
					MegaModifier mod = mods[m];
					colliderObject.mods[m].ModEnabled = mod.ModEnabled;
					colliderObject.mods[m].GroupParams(mod);
				}

				if ( !colliderObject.gameObject.activeInHierarchy )
				{
					//colliderObject.ModifyObject();
				}
			}

			if ( !Application.isPlaying )
				mods = GetComponents<MegaModifier>();

#if UNITY_EDITOR
			if ( EditorApplication.isCompiling )
				DisposeArrays();

			if ( EditorApplication.isPlayingOrWillChangePlaymode )
			{
				if ( !playing )
					DisposeArrays();

				playing = true;
			}
			else
				playing = false;
#endif

			if ( noUpdate || inGroup )
				return;

			if ( UpdateMode == MegaUpdateMode.Update )
			{
				if ( visible || InvisibleUpdate )
				{
					if ( Enabled )
						GetMesh(false);

					//MegaDeformableSkin ds = GetComponent<MegaDeformableSkin>();
					if ( defSkin )
					{
						startverts.CopyFrom(defSkin.mesh.vertices);
						jverts.CopyFrom(startverts);
					}

					ModifyObject();
				}
			}
		}

		void LateUpdate()
		{
			if ( noUpdate || inGroup )
				return;

			if ( UpdateMode == MegaUpdateMode.LateUpdate )
			{
				if ( visible || InvisibleUpdate )
				{
					if ( Enabled )
						GetMesh(false);

					//MegaDeformableSkin ds = GetComponent<MegaDeformableSkin>();
					if ( defSkin )
					{
						startverts.CopyFrom(defSkin.mesh.vertices);
						jverts.CopyFrom(startverts);
					}

					ModifyObject();
				}
			}
		}

		void OnRenderObject()
		{
			if ( noUpdate || inGroup )
				return;

			if ( UpdateMode == MegaUpdateMode.OnRender )
			{
				if ( visible || InvisibleUpdate )
				{
					if ( Enabled )
						GetMesh(false);

					//MegaDeformableSkin ds = GetComponent<MegaDeformableSkin>();
					if ( defSkin )
					{
						startverts.CopyFrom(defSkin.mesh.vertices);
						jverts.CopyFrom(startverts);
					}

					ModifyObject();
				}
			}
		}

		void OnBecameVisible()
		{
			visible = true;
		}

		void OnBecameInvisible()
		{
			if ( Application.isPlaying )
				visible = false;
		}

		void OnDestroy()
		{
			DisposeArrays();

			if ( mesh != cachedMesh )
			{
				if ( restorekeep == 0 || restorekeep == 2 )
				{
					if ( !MegaUtils.SetMeshNew(gameObject, cachedMesh) )
					{
						if ( Application.isEditor )
							DestroyImmediate(mesh);
						else
							Destroy(mesh);
					}
				}
			}

#if false
			for ( int i = 0; i < group.Count; i++ )
			{
				if ( group[i] )
				{
					if ( Application.isEditor )
					{
						for ( int m = 0; m < group[i].mods.Length; i++ )
							DestroyImmediate(group[i].mods[m]);

						DestroyImmediate(group[i]);
					}
					else
					{
						for ( int m = 0; m < group[i].mods.Length; i++ )
							Destroy(group[i].mods[m]);

						Destroy(group[i]);
					}
				}
			}
#endif
		}

		public void ResetCorners()
		{
			if ( mods != null )
			{
				for ( int i = 0; i < mods.Length; i++ )
					mods[i].ResetCorners();
			}
		}

		public bool AnyDeform(float epsilon = 0.0001f)
		{
			if ( mods != null )
			{
				for ( int i = 0; i < mods.Length; i++ )
				{
					if ( mods[i].ModEnabled )
					{
						if ( mods[i].AnyDeform(epsilon) )
							return true;
					}
				}
			}

			return false;
		}

		public void ModifyObject()
		{
			if ( lastUpdateFrame == Time.frameCount )
				return;

			if ( Enabled && mods != null )
			{
				dirtyChannels	= MegaModChannel.None;
				JVertsSource	= true;
				UVsSource		= true;
				modContext.mod	= this;
				selection		= null;

				bool firstmod	= true;

				int modIndex = 0;

				modContext.gizmoPrepare = false;

				for ( int i = 0; i < mods.Length; i++ )
				{
					modContext.Offset = mods[i].Offset;
					modContext.bbox = mods[i].bbox;
					modContext.modIndex = modIndex;
					if ( mods[i].ModEnabled )
						modIndex++;

					mods[i].SetTM();

					mods[i].prepared = mods[i].Prepare(modContext);
				}

				changed = true;

				if ( autoDisable )
				{
					changed = false;

					if ( defSkin )
					{
						changed = true;
					}
					else
					{
						if ( selectionWeight != lastWeight || useColSection != lastUseCol || selectChannel != lastSelectChannel || mask != lastMask )
						{
							lastWeight = selectionWeight;
							lastUseCol = useColSection;
							lastSelectChannel = selectChannel;
							lastMask = mask;
							changed = true;
						}

						if ( !changed )
						{
							for ( int i = 0; i < mods.Length; i++ )
							{
								if ( mods[i].prepared && mods[i].Changed() )
								{
									changed = true;
									break;
								}
							}

							if ( !changed )
								return;
						}
					}
				}

				for ( int i = 0; i < mods.Length; i++ )
				{
					MegaModifier mod = mods[i];

					if ( mod != null && mod.ModEnabled )
					{
						if ( mod.instance )
							mod.SetValues(mod.instance);

						bool vertschange = false;
						if ( (mod.ChannelsReq() & MegaModChannel.Verts) != 0 )  // should be changed
							vertschange = true;

						modContext.Offset = mod.Offset;
						modContext.bbox = mod.bbox;

						if ( mod.ModLateUpdate(modContext) )
						{
							if ( vertschange )
							{
								if ( firstmod )
								{
									mod.jverts = startverts;
									mod.jsverts = jverts;
									firstmod = false;
								}
								else
								{
									mod.jverts = GetSourceJVerts();
									mod.jsverts = GetDestJVerts();
									SwapBuffers();
								}
							}

							if ( selection != null )
							{
								mod.ModifyWeighted(this);
								if ( UpdateMesh < 1 )
									UpdateMesh = 1;
							}
							else
							{
								if ( UpdateMesh < 1 )
								{
									if ( noJobs )
										mod.ModifyNoJobs(this);
									else
										mod.Modify(this);
									UpdateMesh = 1;
								}
								else
								{
									if ( noJobs )
										mod.ModifyNoJobs(this);
									else
										mod.Modify(this);
								}
							}

							dirtyChannels |= mod.ChannelsChanged();
							mod.ModEnd(this);
						}
					}
				}

				if ( UpdateMesh == 1 )
				{
					if ( JVertsSource )
						SetMesh(ref jverts);
					else
						SetMesh(ref jsverts);
					UpdateMesh = 0;
				}
				else
				{
					if ( UpdateMesh == 0 )
					{
						dirtyChannels |= MegaModChannel.Verts;
						SetMesh(ref startverts);
						UpdateMesh = -1;    // Dont need to set verts again until a mod is enabled
					}
				}

				lastUpdateFrame = Time.frameCount;

				for ( int i = 0; i < group.Count; i++ )
				{
					if ( group[i] )
					{
						group[i].GetMesh(false);
						group[i].ModifyObject();
					}
				}

				if ( colliderObject )
				{
					colliderObject.GetMesh(false);
					colliderObject.ModifyObject();
				}
			}
		}

		public NativeArray<Vector3> GetVerts()
		{
			if ( JVertsSource )
				return jverts;
			else
				return jsverts;
		}

		void Done()
		{
			if ( mods != null )
			{
				for ( int m = 0; m < mods.Length; m++ )
				{
					MegaModifier mod = mods[m];
					if ( mod.valid )
						mod.ModEnd(this);
				}
			}
		}

		public void BuildList()
		{
			mods = GetComponents<MegaModifier>();
		}

		public void InitVertSource()
		{
			JVertsSource = true;
			UVsSource = true;
		}

		public void SwapBuffers()
		{
			JVertsSource = !JVertsSource;
		}

		public NativeArray<Vector3> GetSourceJVerts()
		{
			if ( JVertsSource )
				return jverts;

			return jsverts;
		}

		public NativeArray<Vector3> GetDestJVerts()
		{
			if ( JVertsSource )
				return jsverts;

			return jverts;
		}

		public Vector2[] GetSourceUvs()
		{
			if ( UVsSource )
			{
				UVsSource = false;
				return uvs;
			}
			return suvs;
		}

		public Vector2[] GetDestUvs()
		{
			return suvs;
		}

		public void SetMesh(ref NativeArray<Vector3> _verts)
		{
			if ( tmproObj )
			{
				if ( !_verts.IsCreated )
					return;

				tmpMeshInfo = tmproObj.textInfo.CopyMeshInfoVertexData();

				if ( tmproObj.textInfo.meshInfo[0].mesh != null )
				{
					tmproObj.textInfo.meshInfo[0].mesh.SetVertices(_verts);
					tmproObj.UpdateGeometry(tmproObj.textInfo.meshInfo[0].mesh, 0);
				}
			}
			else
			{
				if ( mesh == null || !_verts.IsCreated )
					return;

				if ( (dirtyChannels & MegaModChannel.Verts) != 0 )
				{
					BlendVerts(_verts);

					if ( UpdateMesh == 1 )
						mesh.SetVertices(_verts);
					else
						mesh.SetVertices(_verts);
				}

				if ( (dirtyChannels & MegaModChannel.UV) != 0 )
					mesh.uv = suvs;

				RecalcNormals();
				RecalcTangents();

				if ( recalcbounds )
				{
					//CalcBoundsBurst(mesh);
					mesh.RecalculateBounds();

					// TODO: cache the sr if we use it for setting bounds
					//SkinnedMeshRenderer sr = GetComponent<SkinnedMeshRenderer>();
					if ( sr )
						sr.localBounds = mesh.bounds;
				}

				if ( recalcCollider )
				{
					if ( meshCol == null )
						meshCol = GetComponent<MeshCollider>();

					if ( meshCol != null )
					{
						if ( colliderObject && !colliderObject.recalcCollider )
						{
							meshCol.sharedMesh = null;
							meshCol.sharedMesh = colliderObject.mesh;
						}
						else
						{
							meshCol.sharedMesh = null;
							meshCol.sharedMesh = mesh;
						}
					}
				}
			}
		}

		public void BlendVerts(NativeArray<Vector3> _verts)
		{
			if ( startverts == _verts )
				return;

			if ( mask )
			{
				vertexBlendBoneJob.origverts = startverts;
				vertexBlendBoneJob.verts = _verts;
				vertexBlendBoneJob.weights = weights;
				vertexBlendBoneJob.weight = selectionWeight;
				//vertexBlendBoneJob.channel = (int)selectChannel;

				vertexBlendBoneHandle = vertexBlendBoneJob.Schedule(_verts.Length, batchCount);
				vertexBlendBoneHandle.Complete();
			}
			else
			{
				if ( defSkin && defSkin.useBoneWeights && weights.IsCreated && weights.Length > 0 )
				{
					vertexBlendBoneJob.origverts = startverts;
					vertexBlendBoneJob.verts = _verts;
					vertexBlendBoneJob.weights = weights;
					vertexBlendBoneJob.weight = selectionWeight;
					//vertexBlendBoneJob.channel = (int)selectChannel;

					vertexBlendBoneHandle = vertexBlendBoneJob.Schedule(_verts.Length, batchCount);
					vertexBlendBoneHandle.Complete();
				}
				else
				{
					if ( useColSection && cols.IsCreated && cols.Length > 0 )
					{
						vertexBlendColJob.origverts	= startverts;
						vertexBlendColJob.verts		= _verts;
						vertexBlendColJob.cols		= cols;
						vertexBlendColJob.weight	= selectionWeight;
						vertexBlendColJob.channel	= (int)selectChannel;

						vertexBlendColHandle = vertexBlendColJob.Schedule(_verts.Length, batchCount);
						vertexBlendColHandle.Complete();
					}
					else
					{
						if ( selectionWeight < 0.999f )
						{
							vertexBlendJob.origverts	= startverts;
							vertexBlendJob.verts		= _verts;
							vertexBlendJob.weight		= selectionWeight;

							vertexBlendHandle = vertexBlendJob.Schedule(_verts.Length, batchCount);
							vertexBlendHandle.Complete();
						}
					}
				}
			}
		}

		#region NormalsTangents
		public void RecalcNormals()
		{
			if ( NormalMethod == MegaNormalMethod.Unity || dynamicMesh || noJobs )
				mesh.RecalculateNormals();
			else
			{
				if ( NormalMethod == MegaNormalMethod.Mega )
					CalcNormalsBurst(mesh);
			}
		}

		public void RecalcTangents()
		{
			if ( TangentMethod == MegaTangentMethod.Unity )
				mesh.RecalculateTangents();
			else
			{
				if ( TangentMethod == MegaTangentMethod.Mega )
					BuildTangents();
			}
		}

		void BuildTangents()
		{
			if ( uvs == null )
				return;

			BuildTangents(mesh, jsverts);
		}

		void BuildTangents(Mesh ms, NativeArray<Vector3> _verts)
		{
			int triangleCount = ms.triangles.Length;
			int vertexCount = _verts.Length;

			if ( tan1 == null || tan1.Length != vertexCount )
				tan1 = new Vector3[vertexCount];

			if ( tan2 == null || tan2.Length != vertexCount )
				tan2 = new Vector3[vertexCount];

			if ( tangents == null || tangents.Length != vertexCount )
				tangents = new Vector4[vertexCount];

			Vector3[] norms = ms.normals;
			int[] tris = ms.triangles;

			for ( int a = 0; a < triangleCount; a += 3 )
			{
				int i1 = tris[a];
				int i2 = tris[a + 1];
				int i3 = tris[a + 2];

				Vector3 v1 = _verts[i1];
				Vector3 v2 = _verts[i2];
				Vector3 v3 = _verts[i3];

				Vector2 w1 = uvs[i1];
				Vector2 w2 = uvs[i2];
				Vector2 w3 = uvs[i3];

				float x1 = v2.x - v1.x;
				float x2 = v3.x - v1.x;
				float y1 = v2.y - v1.y;
				float y2 = v3.y - v1.y;
				float z1 = v2.z - v1.z;
				float z2 = v3.z - v1.z;

				float s1 = w2.x - w1.x;
				float s2 = w3.x - w1.x;
				float t1 = w2.y - w1.y;
				float t2 = w3.y - w1.y;

				float r = 1.0f / (s1 * t2 - s2 * t1);

				Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
				Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

				tan1[i1].x += sdir.x;
				tan1[i1].y += sdir.y;
				tan1[i1].z += sdir.z;
				tan1[i2].x += sdir.x;
				tan1[i2].y += sdir.y;
				tan1[i2].z += sdir.z;
				tan1[i3].x += sdir.x;
				tan1[i3].y += sdir.y;
				tan1[i3].z += sdir.z;

				tan2[i1].x += tdir.x;
				tan2[i1].y += tdir.y;
				tan2[i1].z += tdir.z;
				tan2[i2].x += tdir.x;
				tan2[i2].y += tdir.y;
				tan2[i2].z += tdir.z;
				tan2[i3].x += tdir.x;
				tan2[i3].y += tdir.y;
				tan2[i3].z += tdir.z;
			}

			for ( int a = 0; a < _verts.Length; a++ )
			{
				Vector3 n = norms[a];
				Vector3 t = tan1[a];

				Vector3.OrthoNormalize(ref n, ref t);
				tangents[a].x = t.x;
				tangents[a].y = t.y;
				tangents[a].z = t.z;
				tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
			}

			ms.tangents = tangents;
		}

#if false
		int[] FindFacesUsing(Vector3[] verts, List<int> f, Vector3 p, Vector3 n)
		{
			List<int> faces = new List<int>();
			Vector3 v = Vector3.zero;

			for ( int i = 0; i < f.Count; i++ )
			{
				int fi = f[i] * 3;

				v = verts[tris[fi]];
				if ( v.x == p.x && v.y == p.y && v.z == p.z )
				{
					if ( n.Equals(norms[tris[fi]]) )
						faces.Add(fi / 3);
				}
				else
				{
					v = verts[tris[fi + 1]];
					if ( v.x == p.x && v.y == p.y && v.z == p.z )
					{
						if ( n.Equals(norms[tris[fi + 1]]) )
							faces.Add(fi / 3);
					}
					else
					{
						v = verts[tris[fi + 2]];
						if ( v.x == p.x && v.y == p.y && v.z == p.z )
						{
							if ( n.Equals(norms[tris[fi + 2]]) )
								faces.Add(fi / 3);
						}
					}
				}
			}

			return faces.ToArray();
		}
#else
		bool Approx(Vector3 v1, Vector3 v2)
		{
			if ( !Mathf.Approximately(v1.x, v2.x) )
				return false;

			if ( !Mathf.Approximately(v1.y, v2.y) )
				return false;

			if ( !Mathf.Approximately(v1.z, v2.z) )
				return false;

			return true;
		}

		int[] FindFacesUsing(Vector3[] verts, List<int> f, Vector3 p, Vector3 n)
		{
			List<int> faces = new List<int>();
			Vector3 v = Vector3.zero;

			for ( int i = 0; i < f.Count; i++ )
			{
				int fi = f[i] * 3;

				v = verts[tris[fi]];
				if ( v.x == p.x && v.y == p.y && v.z == p.z )
				{
					if ( Approx(n, norms[tris[fi]]) )
						faces.Add(fi / 3);
				}
				else
				{
					v = verts[tris[fi + 1]];
					if ( v.x == p.x && v.y == p.y && v.z == p.z )
					{
						if ( Approx(n, norms[tris[fi + 1]]) )
							faces.Add(fi / 3);
					}
					else
					{
						v = verts[tris[fi + 2]];
						if ( v.x == p.x && v.y == p.y && v.z == p.z )
						{
							if ( n.Equals(norms[tris[fi + 2]]) )
								faces.Add(fi / 3);
						}
					}
				}
			}

			return faces.ToArray();
		}
#endif
		public void BuildNormalMappingNew(Mesh mesh, bool force)
		{
			if ( indexmap.Count == 0 || newmap.Count == 0 || force )
			{
				indexmap.Clear();
				newmap.Clear();

				if ( newMapping == null || newMapping.Length == 0 || force )
				{
					Vector3[] _verts = mesh.vertices;

					float unit;
					MegaVoxelNormals.Voxelize(mesh.vertices, mesh.triangles, mesh.bounds, 32, out unit);

					tris	= mesh.triangles;
					norms	= mesh.normals;
					if ( norms.Length == 0 )
					{
						mesh.RecalculateNormals();
						norms = mesh.normals;
					}

					for ( int i = 0; i < _verts.Length; i++ )
					{
						List<int> faces = MegaVoxelNormals.GetFaces(_verts[i], unit);

						int[] ff = FindFacesUsing(_verts, faces, _verts[i], norms[i]);

						indexmap.Add(newmap.Count); // index into the mapping data to find the count

						newmap.Add(ff.Length);
						for ( int f = 0; f < ff.Length; f++ )
							newmap.Add(ff[f]);
					}
				}
			}
			else
				tris = mesh.triangles;

			int tricount = mesh.triangles.Length;

			faceCount	= new NativeArray<int>(indexmap.ToArray(), Allocator.Persistent);
			newMapping	= new NativeArray<int>(newmap.ToArray(), Allocator.Persistent);
			faceTris	= new NativeArray<int>(tris, Allocator.Persistent);
			if ( !facenormals.IsCreated )
				facenormals	= new NativeArray<Vector3>(tricount / 3, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}


		public void CalcBoundsBurst(Mesh mesh)
		{
			NativeArray<Vector3> verts;

			if ( UpdateMesh == 0 )
				verts = startverts;
			else
			{
				if ( JVertsSource )
					verts = jverts;
				else
					verts = jsverts;
			}

			if ( noJobs )
				mesh.RecalculateBounds();
			else
			{
				calcBoundsJob.verts = verts;
				calcBoundsJob.Execute();
				Bounds b = mesh.bounds;
				b.size = calcBoundsJob.size;
				b.center = calcBoundsJob.pos;
				mesh.bounds = b;
			}
		}

		public void CalcNormalsBurst(Mesh mesh)
		{
			NativeArray<Vector3> verts;

			if ( UpdateMesh == 0 )
				verts = startverts;
			else
			{
				if ( JVertsSource )
					verts = jverts;
				else
					verts = jsverts;
			}

			if ( noJobs )
			{
				CalcNormalsNoJobs(verts, faceTris, facenormals, faceCount, newMapping);
			}
			else
			{
				faceNormsJob.verts		= verts;
				faceNormsJob.tris		= faceTris;
				faceNormsJob.facenorms	= facenormals;
				faceNormsJobHandle		= faceNormsJob.Schedule(tris.Length / 3, batchCount);

				calcNormsJob.indexmap	= faceCount;
				calcNormsJob.normals	= this.normals;
				calcNormsJob.faces		= faceTris;
				calcNormsJob.mapping	= newMapping;
				calcNormsJob.facenorms	= facenormals;

				calcNormsJobHandle		= calcNormsJob.Schedule(jverts.Length, batchCount, faceNormsJobHandle);
				calcNormsJobHandle.Complete();
			}

			mesh.SetNormals(normals);
		}

		public void CalcNormalsNoJobs(NativeArray<Vector3> verts, NativeArray<int> tris, NativeArray<Vector3> facenorms, NativeArray<int> indexmap, NativeArray<int> mapping)
		{
			for ( int f = 0; f < tris.Length / 3; f++ )
			{
				int fi = f * 3;
				Vector3 v30 = verts[tris[fi]];
				Vector3 v31 = verts[tris[fi + 1]];
				Vector3 v32 = verts[tris[fi + 2]];

				float vax = v31.x - v30.x;
				float vay = v31.y - v30.y;
				float vaz = v31.z - v30.z;

				float vbx = v32.x - v31.x;
				float vby = v32.y - v31.y;
				float vbz = v32.z - v31.z;

				v30.x = vay * vbz - vaz * vby;
				v30.y = vaz * vbx - vax * vbz;
				v30.z = vax * vby - vay * vbx;

				// Uncomment this if you dont want normals weighted by poly size
				//float l = v30.x * v30.x + v30.y * v30.y + v30.z * v30.z;
				//l = 1.0f / mathf.sqrt(l);
				//v30.x *= l;
				//v30.y *= l;
				//v30.z *= l;

				facenorms[f] = v30;
			}

			for ( int i = 0; i < tris.Length / 3; i++ )
			{
				int fi = indexmap[i];   // offset into faces array that holds the facenorm indices 

				int count = mapping[fi];    // how many faces are used by this normal

				if ( count > 0 )
				{
					Vector3 norm = facenorms[mapping[fi + 1]];

					for ( int f = 1; f < count; f++ )
						norm += facenorms[mapping[fi + f + 1]];

					normals[i] = norm.normalized;
				}
				else
					normals[i] = Vector3.up;
			}
		}
		#endregion

		#region ContextMenu
#if false	// Set to false if probuilder not in your project
		[ContextMenu("Remove Probuilder")]
		public void RemoveProBuilder()
		{
			MeshFilter mf = GetComponent<MeshFilter>();
			if ( mf )
			{
				Mesh m = MegaUtils.DupMesh(mf.sharedMesh, "");

				UnityEngine.ProBuilder.ProBuilderMesh promesh = GetComponent<UnityEngine.ProBuilder.ProBuilderMesh>();
				DestroyImmediate(promesh);
				mf.sharedMesh = m;
				dynamicMesh = true;
			}
		}
#endif
		[ContextMenu("Resort")]
		public virtual void Resort()
		{
			BuildList();
		}

		[ContextMenu("Help")]
		public virtual void Help()
		{
			Application.OpenURL("http://www.west-racing.com/mf/?page_id=444");
		}

		[ContextMenu("Remove Modify Object (Keep deformed mesh)")]
		public virtual void RemoveKeep()
		{
			MegaModifier[] mods = GetComponents<MegaModifier>();

			for ( int i = 0; i < mods.Length; i++ )
			{
				if ( Application.isEditor )
					DestroyImmediate(mods[i]);
				else
					Destroy(mods[i]);
			}

			restorekeep = 1;
			if ( Application.isEditor )
				DestroyImmediate(this);
			else
				Destroy(this);
		}

		[ContextMenu("Remove Modify Object (Restore Mesh)")]
		public virtual void RemoveRestore()
		{
			MegaModifier[] mods = GetComponents<MegaModifier>();

			for ( int i = 0; i < mods.Length; i++ )
			{
				if ( Application.isEditor )
					DestroyImmediate(mods[i]);
				else
					Destroy(mods[i]);
			}

			restorekeep = 2;
			if ( Application.isEditor )
				DestroyImmediate(this);
			else
				Destroy(this);
		}

		[ContextMenu("Reset")]
		public void Reset()
		{
			if ( originalMesh )
			{
				if ( mods != null && mods.Length > 0 )
					mesh.vertices = originalMesh.vertices;

				mesh.uv = originalMesh.uv;
				mesh.normals = originalMesh.normals;
				mesh.tangents = originalMesh.tangents;

				if ( recalcbounds )
					mesh.bounds = originalMesh.bounds;
			}
		}
#endregion

		public void CalcMaskWeights()
		{
			if ( mask && Uvs.Length > 0 )
			{
				weights = new NativeArray<float>(originalMesh.vertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);

				int chan = (int)selectChannel;
				for ( int i = 0; i < originalMesh.vertexCount; i++ )
				{
					weights[i] = mask.GetPixelBilinear(Uvs[i].x, Uvs[i].y)[chan];
				}
			}
		}

		public void CalcBoneWeigths()
		{
			if ( defSkin && defSkin.useBoneWeights )
			{
				weights = new NativeArray<float>(originalMesh.vertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);

				//for ( int i = 0; i < weights.Length; i++ )
				//{
					//weights[i] = 0.0f;
				//}


				SkinnedMeshRenderer dsr = defSkin.smr;

				if ( dsr )
				{
					Mesh dm = dsr.sharedMesh;
					BoneWeight[] bw = dm.boneWeights;
#if false
					for ( int i = 0; i < dm.vertexCount; i++ )
					{
						if ( weights[i] < bw[i].weight0 * defSkin.bones[bw[i].boneIndex0].weight )
							weights[i] = bw[i].weight0 * defSkin.bones[bw[i].boneIndex0].weight;

						if ( weights[i] < bw[i].weight1 * defSkin.bones[bw[i].boneIndex1].weight )
							weights[i] = bw[i].weight1 * defSkin.bones[bw[i].boneIndex1].weight;

						if ( weights[i] < bw[i].weight2 * defSkin.bones[bw[i].boneIndex2].weight )
							weights[i] = bw[i].weight2 * defSkin.bones[bw[i].boneIndex2].weight;

						if ( weights[i] < bw[i].weight3 * defSkin.bones[bw[i].boneIndex3].weight )
							weights[i] = bw[i].weight3 * defSkin.bones[bw[i].boneIndex3].weight;
					}
#else
					for ( int i = 0; i < dm.vertexCount; i++ )
					{
						weights[i] += bw[i].weight0 * defSkin.bones[bw[i].boneIndex0].weight;
						weights[i] += bw[i].weight1 * defSkin.bones[bw[i].boneIndex1].weight;
						weights[i] += bw[i].weight2 * defSkin.bones[bw[i].boneIndex2].weight;
						weights[i] += bw[i].weight3 * defSkin.bones[bw[i].boneIndex3].weight;
#if false
						if ( weights[i] < bw[i].weight0 * defSkin.bones[bw[i].boneIndex0].weight )
							weights[i] = bw[i].weight0 * defSkin.bones[bw[i].boneIndex0].weight;

						if ( weights[i] < bw[i].weight1 * defSkin.bones[bw[i].boneIndex1].weight )
							weights[i] = bw[i].weight1 * defSkin.bones[bw[i].boneIndex1].weight;

						if ( weights[i] < bw[i].weight2 * defSkin.bones[bw[i].boneIndex2].weight )
							weights[i] = bw[i].weight2 * defSkin.bones[bw[i].boneIndex2].weight;

						if ( weights[i] < bw[i].weight3 * defSkin.bones[bw[i].boneIndex3].weight )
							weights[i] = bw[i].weight3 * defSkin.bones[bw[i].boneIndex3].weight;
#endif
					}
#endif
				}
			}
		}

		public void GetMesh(bool force)
		{
			if ( tmproObj )
				GetTMProMesh(force);
			else
			{
				if ( originalMesh )
				{
					if ( force || jsverts.Length == 0 || mesh == null )
					{
						if ( originalMesh.isReadable )
						{
							mesh = MegaUtils.DupMesh(originalMesh, "");

							cachedMesh = originalMesh;	// Lose cachedmesh

							SetMeshData();
							DisposeArrays();
							startverts	= new NativeArray<Vector3>(originalMesh.vertices, Allocator.Persistent);
							jverts		= new NativeArray<Vector3>(originalMesh.vertices, Allocator.Persistent);
							jsverts		= new NativeArray<Vector3>(originalMesh.vertexCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);	// may not need
							normals		= new NativeArray<Vector3>(mesh.normals, Allocator.Persistent);
							if ( mesh.colors.Length > 0 )
								cols		= new NativeArray<Color>(originalMesh.colors, Allocator.Persistent);

							mesh.MarkDynamic();

							BuildNormalMappingNew(mesh, force);	//false);

							MegaUtils.SetMesh(gameObject, mesh);
						}
					}
				}
			}
		}

		public void GetTMProMesh(bool force)
		{
			if ( tmproObj )
			{
				if ( force || jsverts.Length == 0 )
				{
					DisposeArrays();

					tmproObj.ForceMeshUpdate(true, true);
					tmpMeshInfo = tmproObj.textInfo.CopyMeshInfoVertexData();

					bbox = tmproObj.textInfo.meshInfo[0].mesh.bounds;
					mods = GetComponents<MegaModifier>();

					for ( int i = 0; i < mods.Length; i++ )
					{
						if ( mods[i] != null )
						{
							mods[i].SetModMesh(tmproObj.textInfo.meshInfo[0].mesh);
							mods[i].ModStart(this);
						}
					}
					mapping = null;
					UpdateMesh = -1;

					startverts = new NativeArray<Vector3>(tmpMeshInfo[0].vertices, Allocator.Persistent);
					jverts = new NativeArray<Vector3>(tmpMeshInfo[0].vertices, Allocator.Persistent);
					jsverts = new NativeArray<Vector3>(tmpMeshInfo[0].vertices.Length, Allocator.Persistent); // may not need
				}
			}
		}

		public void DisposeArrays()
		{
			if ( startverts.IsCreated )		startverts.Dispose();
			if ( jverts.IsCreated )			jverts.Dispose();
			if ( jsverts.IsCreated )		jsverts.Dispose();
			if ( normals.IsCreated )		normals.Dispose();
			if ( facenormals.IsCreated )	facenormals.Dispose();
			if ( faceTris.IsCreated )		faceTris.Dispose();
			if ( faceCount.IsCreated )		faceCount.Dispose();
			if ( newMapping.IsCreated )		newMapping.Dispose();
			if ( cols.IsCreated )			cols.Dispose();

			startverts	= default;
			jverts		= default;
			jsverts		= default;
			normals		= default;
			facenormals	= default;
			faceTris	= default;
			faceCount	= default;
			newMapping	= default;
			cols		= default;

			if ( mods != null )
			{
				for ( int i = 0; i < mods.Length; i++ )
					mods[i].Dispose();
			}
		}

		void SetMeshData()
		{
			bbox = originalMesh.bounds;

			uvs = originalMesh.uv;
			suvs = new Vector2[originalMesh.uv.Length];
			//cols = originalMesh.colors;

			//for ( int i = 0; i < group.Count; i++ )
			//{
				//if ( group[i] )
					//bbox = MegaUtils.AddBounds(transform, bbox, group[i].transform, group[i].bbox);
			//}

			mods = GetComponents<MegaModifier>();

			for ( int i = 0; i < mods.Length; i++ )
			{
				if ( mods[i] != null )
				{
					mods[i].SetModMesh(originalMesh);	//mesh);
					mods[i].ModStart(this);
				}
			}
			mapping = null;
			UpdateMesh = -1;
		}

		public void ModReset(MegaModifier m)
		{
			if ( m != null )
			{
				m.SetModMesh(cachedMesh);
				BuildList();
			}
		}

		public void MeshChanged()
		{
			originalMesh = null;
			cachedMesh = null;

			GameObject sourceObj;
			originalMesh = MegaUtils.FindMesh(gameObject, out sourceObj);

			GetMesh(true);
		}

		public void MeshUpdated()
		{
			GetMesh(true);

			if ( mods != null )
			{
				for ( int i = 0; i < mods.Length; i++ )
					mods[i].SetModMesh(cachedMesh);
			}
		}

		public void AddToGroup(MegaModifyObject modobj)
		{
			if ( modobj && !group.Contains(modobj) )
			{
				group.Add(modobj);

				Vector3 offset = transform.InverseTransformPoint(modobj.transform.position);
				for ( int m = 0; m < modobj.mods.Length; m++ )
					modobj.mods[m].Offset = offset;
			}
		}

		public void RemoveFromGroup(MegaModifyObject modobj)
		{
			group.Remove(modobj);
		}

		// new runtime group making
		public void ApplyModsToGroup()
		{
			for ( int g = 0; g < group.Count; g++ )
			{
				MegaModifyObject tobj = group[g];
				ApplyModsToObject(tobj);
			}

			if ( colliderObject )
			{
				ApplyModsToObject(colliderObject);
			}
		}

		public void ApplyModsToObject(MegaModifyObject tobj)
		{
			if ( tobj )
			{
				Vector3 offset = transform.InverseTransformPoint(tobj.transform.position);

				tobj.inGroup = true;

				// Not ideal as may have added extras?
				MegaModifier[] mods = tobj.GetComponents<MegaModifier>();
				for ( int i = 0; i < mods.Length; i++ )
					DestroyImmediate(mods[i]);

				for ( int i = 0; i < mods.Length; i++ )
				{
					MegaModifier mod = mods[i];
					MegaModifier m = (MegaModifier)tobj.GetComponent(mod.GetType());

					if ( !m )
						m = (MegaModifier)tobj.gameObject.AddComponent(mod.GetType());

					if ( m )
					{
						m.hideFlags = HideFlags.HideInInspector;
						CopyMod(mod, m);
						m.Offset = offset;
					}
				}

				if ( tobj.tmproObj )
					tobj.dynamicMesh = true;
			}
		}

		void CopyMod(MegaModifier from, MegaModifier to)
		{
			System.Type type = from.GetType();
			if ( type != to.GetType() )
				return; // type mis-match

			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
			PropertyInfo[] pinfos = type.GetProperties(flags);
			foreach ( var pinfo in pinfos )
			{
				if ( pinfo.CanWrite )
				{
					try
					{
						pinfo.SetValue(to, pinfo.GetValue(from, null), null);
					}
					catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
				}
			}
			FieldInfo[] finfos = type.GetFields(flags);
			foreach ( var finfo in finfos )
			{
				finfo.SetValue(to, finfo.GetValue(from));
			}
		}
		// End

		public T FindMod<T>(string label = "") where T : MegaModifier
		{
			for ( int i = 0; i < mods.Length; i++ )
			{
				if ( mods[i].GetType() == typeof(T) )
				{
					if ( label.Length > 0 )
					{
						if ( label == mods[i].Label )
							return (T)mods[i];
					}
					else
						return (T)mods[i];
				}
			}

			return null;
		}

		void OnDrawGizmosSelected()
		{
			modContext.mod = this;
			modContext.go = gameObject;

			if ( GlobalDisplay && DrawGizmos && Enabled && mods != null )
			{
				for ( int i = 0; i < mods.Length; i++ )
				{
					MegaModifier mod = mods[i];

					if ( mod != null )
					{
						if ( mod.ModEnabled && mod.DisplayGizmo && (mod.hideFlags & HideFlags.HideInInspector) == 0 )
						{
							modContext.Offset = mod.Offset;
							modContext.bbox = mod.bbox;
							modContext.gizmoPrepare = true;
							mod.DrawGizmo(modContext);
						}
					}
				}
			}
		}

		public void AttachChildren()
		{
			for ( int i = 0; i < transform.childCount; i++ )
			{
				GameObject obj = transform.GetChild(i).gameObject;

				MegaAttach attach = obj.GetComponent<MegaAttach>();

				if ( !attach )
					attach = obj.AddComponent<MegaAttach>();

				attach.target = this;
				attach.AttachIt();
				attach.enabled = true;
			}
		}

		public void DetachChildren()
		{
			for ( int i = 0; i < transform.childCount; i++ )
			{
				GameObject obj = transform.GetChild(i).gameObject;

				MegaAttach attach = obj.GetComponent<MegaAttach>();

				if ( attach )
				{
					attach.attached = false;
					attach.enabled = false;
				}
			}
		}

		public void SetUpdate()
		{
			if ( mods != null )
			{
				for ( int i = 0; i < mods.Length; i++ )
				{
					mods[i].ResetCorners();
				}
			}
		}

#if false
		class BlendShapeFrame
		{
			public float		weight;
			public Vector3[]	deltaverts;
			public Vector3[]	deltanorms;
			public Vector3[]	deltatans;
		}

		class BlendShapeChannel
		{
			public string					name;
			public int						index;
			public List<BlendShapeFrame>	frames = new List<BlendShapeFrame>();
		}

		List<BlendShapeChannel> GrabBlendShapes(Mesh mesh)
		{
			int bcount = mesh.blendShapeCount;

			List<BlendShapeChannel> channels = new List<BlendShapeChannel>();

			for ( int j = 0; j < bcount; j++ )
			{
				BlendShapeChannel channel = new BlendShapeChannel();

				int frames = mesh.GetBlendShapeFrameCount(j);

				channel.name = mesh.GetBlendShapeName(j);
				channel.index = j;

				channel.frames = new List<BlendShapeFrame>();

				string bname = mesh.GetBlendShapeName(j);

				for ( int f = 0; f < frames; f++ )
				{
					BlendShapeFrame frame = new BlendShapeFrame();
					frame.weight = mesh.GetBlendShapeFrameWeight(j, f);
					frame.deltaverts = new Vector3[mesh.vertexCount];
					frame.deltanorms = new Vector3[mesh.vertexCount];
					frame.deltatans = new Vector3[mesh.vertexCount];

					channel.frames.Add(frame);
					mesh.GetBlendShapeFrameVertices(j, f, frame.deltaverts, frame.deltanorms, frame.deltatans);
				}

				channels.Add(channel);
			}

			return channels;
		}

		void SetBlendShapes(Mesh mesh, List<BlendShapeChannel> channels)
		{
			mesh.ClearBlendShapes();

			for ( int j = 0; j < channels.Count; j++ )
			{
				int frames = channels[j].frames.Count;
				string bname = channels[j].name;

				for ( int f = 0; f < frames; f++ )
				{
					float weight = channels[j].frames[f].weight;
					mesh.AddBlendShapeFrame(bname, weight, channels[j].frames[f].deltaverts, channels[j].frames[f].deltanorms, channels[j].frames[f].deltatans);
				}
			}

			originalMesh.ClearBlendShapes();
			MegaUtils.CopyBlendShapes(mesh, originalMesh);
		}

		public void RemoveChannel(string name)
		{
			List<BlendShapeChannel> channels = GrabBlendShapes(mesh);

			for ( int i = 0; i < channels.Count; i++ )
			{
				if ( channels[i].name == name )
				{
					channels.RemoveAt(i);
					break;
				}
			}

			// Set the blendshapes back
			SetBlendShapes(mesh, channels);
		}

		BlendShapeChannel GetChannel(string name, List<BlendShapeChannel> channels)
		{
			for ( int i = 0; i < channels.Count; i++ )
			{
				if ( channels[i].name == name )
					return channels[i];
			}

			return null;
		}

		static int CompareFrames(BlendShapeFrame f1, BlendShapeFrame f2)
		{
			if ( f1.weight < f2.weight )
				return -1;

			if ( f1.weight > f2.weight )
				return 1;

			return 0;
		}

		public void AddBlendShapeFrame(string name, float weight)
		{
			List<BlendShapeChannel> channels = GrabBlendShapes(mesh);

			BlendShapeChannel channel = GetChannel(name, channels);

			if ( channel == null )
			{
				channel = new BlendShapeChannel();
				channel.name = name;
				channels.Add(channel);
			}

			BlendShapeFrame frame = new BlendShapeFrame();
			frame.weight = weight;
			frame.deltaverts	= new Vector3[startverts.Length];
			frame.deltanorms	= new Vector3[startverts.Length];
			frame.deltatans		= new Vector3[startverts.Length];

			channel.frames.Add(frame);

			NativeArray<Vector3> verts = GetVerts();
			Vector3[] origVerts = originalMesh.vertices;
			Vector3[] origNorms = originalMesh.normals;
			Vector4[] origTans = originalMesh.tangents;

			Vector4[] tans = mesh.tangents;

			for ( int i = 0; i < verts.Length; i++ )
			{
				frame.deltaverts[i] = verts[i] - origVerts[i];
				frame.deltanorms[i] = normals[i] - origNorms[i];
				if ( tans.Length > 0 )
					frame.deltatans[i] = tans[i] - origTans[i];
			}

			channel.frames.Sort(CompareFrames);

			SetBlendShapes(mesh, channels);
		}

		public void ChangeFrameWeight(string name, int f, float weight)
		{
			List<BlendShapeChannel> channels = GrabBlendShapes(mesh);

			BlendShapeChannel channel = GetChannel(name, channels);

			if ( channel != null )
				channel.frames[f].weight = weight;

			SetBlendShapes(mesh, channels);
		}

		public void RemoveBlendShapeFrame(string name, int frame)
		{
			List<BlendShapeChannel> channels = GrabBlendShapes(mesh);

			BlendShapeChannel channel = GetChannel(name, channels);

			if ( channel != null )
				channel.frames.RemoveAt(frame);

			SetBlendShapes(mesh, channels);
		}
#endif
	}
}