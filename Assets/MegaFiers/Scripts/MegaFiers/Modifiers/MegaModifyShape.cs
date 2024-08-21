using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

#if UNITY_EDITOR
using UnityEditor;
#endif

// We need to either do all in megamodobject, or modshape derives from modobject
// prob best to have all in meg mod obj
#if false
// TODO: Add Spline Morphing
namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Modify Shape")]
	[ExecuteInEditMode]
	public class MegaModifyShape : MonoBehaviour
	{
		public Mesh						originalMesh;
		[HideInInspector]
		public Mesh						cachedMesh;		// The original mesh for this object
		public bool						InvisibleUpdate	= false;
		[HideInInspector]
		public Bounds					bbox			= new Bounds();
		public bool						recalcbounds	= false;
		public bool						Enabled			= true;
		public MegaUpdateMode			UpdateMode		= MegaUpdateMode.LateUpdate;
		public bool						DrawGizmos		= true;
		public MegaModifier[]			mods			= null;
		[HideInInspector]
		public int						UpdateMesh		= 0;
		[HideInInspector]
		public GameObject				sourceObj;
		public static bool				GlobalDisplay	= true;
		public MegaModContext			modContext		= new MegaModContext();
		[Unity.Collections.ReadOnly][System.NonSerialized]
		public NativeArray<Vector3>		startverts;
		[System.NonSerialized]
		public NativeArray<Vector3>		jverts;
		[System.NonSerialized]
		public NativeArray<Vector3>		jsverts;
		bool							visible			= true;
		public int						restorekeep		= 0;
		bool							JVertsSource	= false;
		bool playing;
		public bool						autoDisable		= true;
		public bool						noUpdate;
		public bool						showGroup;
		public bool						groupEnabled	= true;
		public List<MegaModifyObject>	group			= new List<MegaModifyObject>();
		public bool						showParams;
		public bool						quickEdit		= true;
		public int						quickEditSize	= 200;
		int								lastUpdateFrame;
		public bool						inGroup			= false;
		public int						batchCount		= 64;
		public Vector2					scrollpos;
		public bool						noJobs			= false;
		public bool						changed;
		public int						subDivide		= 0;

		private void Awake()
		{
			DisposeArrays();
		}

		void Start()
		{
			if ( !originalMesh )
			{
				GameObject sourceObj;

					originalMesh = MegaUtils.FindMesh(gameObject, out sourceObj);
			}

			if ( Application.platform == RuntimePlatform.WebGLPlayer && !Application.isEditor )
				noJobs = true;	// Turn off jobs in built webgl as has no support for jobs or threading
		}

		public void ForceUpdate()
		{
			lastUpdateFrame = Time.frameCount - 1;
		}

		void Update()
		{
			if ( noUpdate )
				return;

			if ( groupEnabled && group.Count > 0 )
			{
				for ( int g = 0; g < group.Count; g++ )
				{
					MegaModifyShape modobj = group[g];

					if ( modobj )
					{
						modobj.InvisibleUpdate	= InvisibleUpdate;
						modobj.recalcbounds		= recalcbounds;
						modobj.UpdateMode		= UpdateMode;
						modobj.DrawGizmos		= DrawGizmos;

						for ( int m = 0; m < mods.Length; m++ )
						{
							MegaModifier mod = mods[m];
							modobj.mods[m].ModEnabled = mod.ModEnabled;
							modobj.mods[m].GroupParams(mod);
						}
					}
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
				JVertsSource	= true;
				//modContext.mod	= this;

				bool firstmod	= true;

				for ( int i = 0; i < mods.Length; i++ )
				{
					modContext.Offset = mods[i].Offset;
					modContext.bbox = mods[i].bbox;

					mods[i].SetTM();

					mods[i].prepared = mods[i].Prepare(modContext);
				}

				changed = true;

				if ( autoDisable )
				{
					changed = false;

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

							//if ( UpdateMesh < 1 )
							//{
								//if ( noJobs )
									//mod.ModifyNoJobs(this);
								//else
									//mod.Modify(this);
								//UpdateMesh = 1;
							//}
							//else
							//{
								//if ( noJobs )
									//mod.ModifyNoJobs(this);
								//else
									//mod.Modify(this);
							//}

							//mod.ModEnd(this);
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
					//if ( mod.valid )
						//mod.ModEnd(this);
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

		public void SetMesh(ref NativeArray<Vector3> _verts)
		{
#if false
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
#endif
		}

		public void BlendVerts(NativeArray<Vector3> _verts)
		{
			if ( startverts == _verts )
				return;
#if false
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
#endif
		}

		#region NormalsTangents
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
		}
#endregion

		public void GetMesh(bool force)
		{
			if ( originalMesh )
			{
				if ( force || jsverts.Length == 0 )	//|| mesh == null )
				{
					if ( originalMesh.isReadable )
					{
						//mesh = MegaUtils.DupMesh(originalMesh, "");

						cachedMesh = originalMesh;	// Lose cachedmesh

						SetMeshData();
						DisposeArrays();
						startverts	= new NativeArray<Vector3>(originalMesh.vertices, Allocator.Persistent);
						jverts		= new NativeArray<Vector3>(originalMesh.vertices, Allocator.Persistent);
						jsverts		= new NativeArray<Vector3>(originalMesh.vertexCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);	// may not need

						//MegaUtils.SetMesh(gameObject, mesh);
					}
				}
			}
		}

		public void DisposeArrays()
		{
			if ( startverts.IsCreated )		startverts.Dispose();
			if ( jverts.IsCreated )			jverts.Dispose();
			if ( jsverts.IsCreated )		jsverts.Dispose();

			startverts	= default;
			jverts		= default;
			jsverts		= default;

			if ( mods != null )
			{
				for ( int i = 0; i < mods.Length; i++ )
					mods[i].Dispose();
			}
		}

		void SetMeshData()
		{
			bbox = originalMesh.bounds;

			mods = GetComponents<MegaModifier>();

			for ( int i = 0; i < mods.Length; i++ )
			{
				if ( mods[i] != null )
				{
					mods[i].SetModMesh(originalMesh);	//mesh);
					//mods[i].ModStart(this);
				}
			}
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
			//modContext.mod = this;
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

				//attach.target = this;
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
	}
}
	#endif
// 1647