using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

// 0.41ms for wrap before jobbing, seems to be 0.1ms after
namespace MegaFiers
{
	public struct MegaCloseFace
	{
		public int		face;
		public float	dist;
	}

	[System.Serializable]
	public struct megabindinf
	{
		public float	dist;
		public int		face;	// not sued
		public int		i0;
		public int		i1;
		public int		i2;
		public Vector3	bary;
		public float	weight;
		public float	area;	// not used
	}

	[ExecuteInEditMode]
	public class MegaWrap : MonoBehaviour
	{
		public float[]				weight;
		public float[]				weights;
		public Vector3[]			barys;
		public float[]				dist;
		public int[]				ixs;
		public int[]				indexes;
		public List<int>			neededVerts		= new List<int>();	// verts that need skinning first
		public Matrix4x4[]			bindposes;						// poses for skinning
		public BoneWeight[]			boneweights;					// bone weights for skinning
		public Transform[]			bones;							// bones
		public Vector3[]			freeverts;						// position for any vert with no attachments
		public Vector3[]			startverts;						// meshes before any wrapping
		public int[]				tris;							// tris for mesh
		public Vector3[]			norms;
		public List<int>			indexmap		= new List<int>();
		public List<int>			newmap			= new List<int>();
		[Unity.Collections.ReadOnly]
		public NativeArray<int>		faceTris;						// used by normal calc jobs
		[Unity.Collections.ReadOnly]
		public NativeArray<int>		faceCount;						// used by normal calc jobs
		[Unity.Collections.ReadOnly]
		public NativeArray<int>		newMapping;						// byte only needed here
		public NativeArray<Vector3> normals;						// used by normal calc
		public NativeArray<Vector3> facenormals;					// used by normal calc, filled by first job
		[Unity.Collections.ReadOnly]
		public NativeArray<Vector3>	skinnedVerts;
		[Unity.Collections.ReadOnly]
		public NativeArray<float>	jweight;
		[Unity.Collections.ReadOnly]
		public NativeArray<float>	jweights;
		[Unity.Collections.ReadOnly]
		public NativeArray<float>	jdist;
		[Unity.Collections.ReadOnly]
		public NativeArray<Vector3>	jbarys;
		[Unity.Collections.ReadOnly]
		public NativeArray<int>		jixs;
		[Unity.Collections.ReadOnly]
		public NativeArray<int>		jindexes;
		public NativeArray<Vector3> targetVerts;
		public NativeArray<Vector3>	verts;
		public Mesh					originalMesh;
		public float				gap				= 0.0f;
		public float				shrink			= 1.0f;
		public Mesh					mesh			= null;
		public Vector3				offset			= Vector3.zero;
		public bool					targetIsSkin	= false;
		public bool					sourceIsSkin	= false;
		public int					nomapcount		= 0;
		public float				size			= 0.01f;
		public int					vertindex		= 0;
		public MegaModifyObject		target;
		public bool					useMaxDist		= false;
		public float				maxdist			= 0.25f;
		public int					maxpoints		= 1;
		public int					voxelRes		= 32;
		public bool					WrapEnabled		= true;
		public MegaNormalMethod		NormalMethod	= MegaNormalMethod.Mega;
		public bool					UseBakedMesh	= true;
		public bool					recalcBounds	= true;
		public bool					localSpace		= true;
		public bool					autoDisable		= true;
		public SkinnedMeshRenderer	tmesh;
		Mesh						bakedmesh		= null;
		Job							job;
		JobHandle					jobHandle;
		FaceNormalsJob				faceNormsJob;
		JobHandle					faceNormsJobHandle;
		CalcNormalsJob				calcNormsJob;
		JobHandle					calcNormsJobHandle;
		public bool					canGetMesh		= false;
		public int					targetVertexCount;
		public static bool			IsPlaying		= false;
		public float				lastGap;
		float						lastShrink;
		public bool					forceUpdate		= false;
		public float[]				lastWeights;
		public int					blendShapeCount;

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			float3						gcp;
			float3						e11;
			float3						e22;
			float3						cr;
			float3						p;
			public Vector3				offset;
			public float				gap;
			public float				shrink;
			public NativeArray<Vector3>	verts;
			public Matrix4x4			stm;
			//public Matrix4x4			invstm;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>	skinnedVerts;  // count and index into other arrays pairs
			[Unity.Collections.ReadOnly]
			public NativeArray<int>		index;	// count and index into other arrays pairs
			[Unity.Collections.ReadOnly]
			public NativeArray<float>	weight;
			[Unity.Collections.ReadOnly]
			public NativeArray<float>	weights;
			[Unity.Collections.ReadOnly]
			public NativeArray<float>	dist;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>	barys;
			[Unity.Collections.ReadOnly]
			public NativeArray<int>		ixs;

			public float3 FaceNormal(float3 p0, float3 p1, float3 p2)
			{
				e11.x = p1.x - p0.x;
				e11.y = p1.y - p0.y;
				e11.z = p1.z - p0.z;

				e22.x = p2.x - p0.x;
				e22.y = p2.y - p0.y;
				e22.z = p2.z - p0.z;

				cr.x = e11.y * e22.z - e22.y * e11.z;
				cr.y = -(e11.x * e22.z - e22.x * e11.z);
				cr.z = e11.x * e22.y - e22.x * e11.y;

				return cr;
			}

			public float3 GetCoordMine(float3 A, float3 B, float3 C, float3 bary)
			{
				gcp.x = (bary.x * A.x) + (bary.y * B.x) + (bary.z * C.x);
				gcp.y = (bary.x * A.y) + (bary.y * B.y) + (bary.z * C.y);
				gcp.z = (bary.x * A.z) + (bary.y * B.z) + (bary.z * C.z);

				return gcp;
			}

			public void Execute(int i)
			{
				p = Vector3.zero;

				int c = index[i * 2];
				if ( c > 0 )
				{
					int ix = index[(i * 2) + 1];
					int ixx = ix * 3;

					float oow = 1.0f / weight[i];

					for ( int j = 0; j < c; j++ )
					{
						int j1 = (j * 3) + ixx;
						float3 p0 = skinnedVerts[ixs[j1]];	//ixs[ixx + (j * 3)]];
						float3 p1 = skinnedVerts[ixs[j1 + 1]];	//ixs[ixx + 1 + (j * 3)]];
						float3 p2 = skinnedVerts[ixs[j1 + 2]];   //ixs[ixx + 2 + (j * 3)]];

						//float3 p0 = skinnedVerts[ixs[ixx + (j * 3)]];
						//float3 p1 = skinnedVerts[ixs[ixx + 1 + (j * 3)]];
						//float3 p2 = skinnedVerts[ixs[ixx + 2 + (j * 3)]];

						float3 cp = GetCoordMine(p0, p1, p2, barys[ix + j]);
						float3 norm = FaceNormal(p0, p1, p2);

						float sq = 1.0f / math.length(norm);    //math.sqrt(norm.x * norm.x + norm.y * norm.y + norm.z * norm.z);

						float d = (dist[ix + j] * shrink) + gap;

						cp.x += d * norm.x * sq;
						cp.y += d * norm.y * sq;
						cp.z += d * norm.z * sq;

						float bw = weights[ix + j] * oow;

						if ( j == 0 )
						{
							p.x = cp.x * bw;
							p.y = cp.y * bw;
							p.z = cp.z * bw;
						}
						else
						{
							p.x += cp.x * bw;
							p.y += cp.y * bw;
							p.z += cp.z * bw;
						}
					}
				}

				Vector3 pp = stm.MultiplyPoint3x4(p);
				verts[i] = pp + offset;
			}
		}

		private void Awake()
		{
			canGetMesh = true;
			DisposeArrays();
		}

		void Start()
		{
			MeshFilter mf = GetComponent<MeshFilter>();

			if ( mf != null )
				originalMesh = mf.sharedMesh;
			else
			{
				SkinnedMeshRenderer smesh = (SkinnedMeshRenderer)GetComponent(typeof(SkinnedMeshRenderer));

				if ( smesh != null )
				{
					originalMesh = smesh.sharedMesh;
					sourceIsSkin = true;
				}
			}

			if ( originalMesh )
			{
				startverts = originalMesh.vertices;

				mesh = MegaUtils.DupMesh(originalMesh, "");	//CloneMesh(originalMesh);

				if ( mf )
					mf.sharedMesh = mesh;
				else
				{
					SkinnedMeshRenderer smesh = (SkinnedMeshRenderer)GetComponent(typeof(SkinnedMeshRenderer));
					smesh.sharedMesh = mesh;
				}

				mesh.MarkDynamic();
				GetMesh(false);
			}
		}

		private void OnDestroy()
		{
			DisposeArrays();
		}

		public void GetMesh(bool force)
		{
			if ( originalMesh )
			{
				if ( force || !verts.IsCreated || !jweights.IsCreated )	//|| mesh == null )
				{
					DisposeArrays();

					if ( !verts.IsCreated )
						verts = new NativeArray<Vector3>(originalMesh.vertices, Allocator.Persistent);

					if ( !normals.IsCreated )
						normals = new NativeArray<Vector3>(mesh.normals, Allocator.Persistent);

					BuildNormalMappingNew(mesh, false);
					MegaUtils.SetMesh(gameObject, mesh);

					// Have a single value to say if we have saved data or not and that is correct
					if ( indexes != null && indexes.Length > 0 )
						jindexes = new NativeArray<int>(indexes, Allocator.Persistent);

					if ( weight != null && weight.Length > 0 )
						jweight = new NativeArray<float>(weight, Allocator.Persistent);

					if ( weights != null && weights.Length > 0 )
						jweights = new NativeArray<float>(weights, Allocator.Persistent);

					if ( barys != null && barys.Length > 0 )
						jbarys = new NativeArray<Vector3>(barys, Allocator.Persistent);

					if ( dist != null && dist.Length > 0 )
						jdist = new NativeArray<float>(dist, Allocator.Persistent);

					if ( ixs != null && ixs.Length > 0 )
						jixs = new NativeArray<int>(ixs, Allocator.Persistent);

					if ( targetIsSkin && !UseBakedMesh )
						skinnedVerts = new NativeArray<Vector3>(targetVertexCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				}
				else
				{
					if ( targetIsSkin && !UseBakedMesh )
					{
						if ( skinnedVerts.IsCreated )
							skinnedVerts.Dispose();

						if ( !skinnedVerts.IsCreated )
							skinnedVerts = new NativeArray<Vector3>(targetVertexCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					}
				}
			}
		}

		[ContextMenu("Help")]
		public void Help()
		{
			Application.OpenURL("http://www.west-racing.com/mf/?page_id=3709");
		}

		[ContextMenu("Reset Mesh")]
		public void ResetMesh()
		{
			DisposeArrays();

			if ( mesh )
			{
				mesh.vertices = originalMesh.vertices;
				mesh.normals = originalMesh.normals;
				mesh.bounds = originalMesh.bounds;
				BuildNormalMappingNew(mesh, false);
			}

			weight = null;
		}

		public void DisposeArrays()
		{
			if ( jindexes.IsCreated )		jindexes.Dispose();
			if ( jweight.IsCreated )		jweight.Dispose();
			if ( jweights.IsCreated )		jweights.Dispose();
			if ( jdist.IsCreated )			jdist.Dispose();
			if ( jbarys.IsCreated )			jbarys.Dispose();
			if ( jixs.IsCreated )			jixs.Dispose();
			if ( verts.IsCreated )			verts.Dispose();
			if ( faceTris.IsCreated )		faceTris.Dispose();
			if ( faceCount.IsCreated )		faceCount.Dispose();
			if ( newMapping.IsCreated )		newMapping.Dispose();
			if ( normals.IsCreated )		normals.Dispose();
			if ( facenormals.IsCreated )	facenormals.Dispose();
			if ( skinnedVerts.IsCreated )	skinnedVerts.Dispose();
		}

		void LateUpdate()
		{
			if ( target )
			{
				bool doupdate = true;

				if ( autoDisable )
				{
					if ( !target.changed )
						doupdate = false;
				}

				if ( gap != lastGap || shrink != lastShrink )
				{
					lastGap = gap;
					lastShrink = shrink;
					doupdate = true;
				}

				if ( BlendShapesChanged() )
					doupdate = true;

				if ( doupdate || forceUpdate )
				{
					forceUpdate = false;
					DoUpdate();
				}
			}
		}

		bool BlendShapesChanged()
		{
			if ( targetIsSkin && target.originalMesh.blendShapeCount > 0 )
			{
				if ( target.originalMesh.blendShapeCount != blendShapeCount )
				{
					blendShapeCount = target.originalMesh.blendShapeCount;
					lastWeights = new float[target.originalMesh.blendShapeCount];
				}

				for ( int i = 0; i < target.originalMesh.blendShapeCount; i++ )
				{
					if ( tmesh.GetBlendShapeWeight(i) != lastWeights[i] )
					{
						lastWeights[i] = tmesh.GetBlendShapeWeight(i);
						return true;
					}
				}
			}

			return false;
		}

		public Vector3 GetSkinPos(int i)
		{
			Vector3 pos = targetVerts[i];
			Vector3 bpos = bindposes[boneweights[i].boneIndex0].MultiplyPoint(pos);
			Vector3 p = bones[boneweights[i].boneIndex0].TransformPoint(bpos) * boneweights[i].weight0;

			bpos = bindposes[boneweights[i].boneIndex1].MultiplyPoint(pos);
			p += bones[boneweights[i].boneIndex1].TransformPoint(bpos) * boneweights[i].weight1;

			bpos = bindposes[boneweights[i].boneIndex2].MultiplyPoint(pos);
			p += bones[boneweights[i].boneIndex2].TransformPoint(bpos) * boneweights[i].weight2;

			bpos = bindposes[boneweights[i].boneIndex3].MultiplyPoint(pos);
			p += bones[boneweights[i].boneIndex3].TransformPoint(bpos) * boneweights[i].weight3;

			return p;
		}

		public void GetTargetVerts()
		{
			targetVerts = target.GetVerts();
		}

		void DoUpdate()
		{
			if ( WrapEnabled == false || target == null || weight == null || weight.Length == 0 )   //|| bindverts == null )
			{
				weight = null;
				return;
			}

#if UNITY_EDITOR
			if ( canGetMesh)	//Application.isPlaying )
				GetMesh(false);
#endif
			GetTargetVerts();

			if ( mesh == null || !targetVerts.IsCreated )
				return;

			if ( !verts.IsCreated )
				return;

			if ( targetIsSkin && neededVerts != null && neededVerts.Count > 0 )
			{
				//if ( boneweights == null || tmesh == null )
				//{
					//tmesh = (SkinnedMeshRenderer)target.GetComponent(typeof(SkinnedMeshRenderer));
					//if ( tmesh != null )
					//{
						//if ( !sourceIsSkin )
						//{
							//Mesh sm		= tmesh.sharedMesh;
							//bindposes	= sm.bindposes;
							//bones		= tmesh.bones;
							//boneweights	= sm.boneWeights;
						//}
					//}
				//}

				//if ( tmesh == null )
					//tmesh = (SkinnedMeshRenderer)target.GetComponent(typeof(SkinnedMeshRenderer));

				if ( UseBakedMesh || boneweights.Length == 0 )
				{
					if ( bakedmesh == null )
						bakedmesh = new Mesh();

					tmesh.BakeMesh(bakedmesh);
					skinnedVerts = new NativeArray<Vector3>(bakedmesh.vertices, Allocator.TempJob);
				}
				else
				{
					// job this as well
					skinnedVerts = new NativeArray<Vector3>(bakedmesh.vertexCount, Allocator.TempJob);

					for ( int i = 0; i < neededVerts.Count; i++ )
						skinnedVerts[neededVerts[i]] = GetSkinPos(neededVerts[i]);
				}
			}

			Matrix4x4 stm = Matrix4x4.identity;

			Vector3 p = Vector3.zero;
			if ( targetIsSkin && !sourceIsSkin )
			{
				if ( localSpace )
					stm = Matrix4x4.identity;
				else
				{
					//stm = transform.worldToLocalMatrix * target.transform.localToWorldMatrix;
					stm = target.transform.localToWorldMatrix * transform.worldToLocalMatrix;
					//stm = stm.inverse;	//transform.worldToLocalMatrix * target.transform.localToWorldMatrix;
				}

				job.offset			= offset;
				job.gap				= gap;
				job.shrink			= shrink;
				job.verts			= verts;
				job.stm				= stm;
				job.skinnedVerts	= skinnedVerts;
				job.index			= jindexes;
				job.weight			= jweight;
				job.weights			= jweights;
				job.dist			= jdist;
				job.barys			= jbarys;
				job.ixs				= jixs;

				jobHandle = job.Schedule(verts.Length, 64);
				jobHandle.Complete();

				if ( UseBakedMesh )
					skinnedVerts.Dispose();
			}
			else
			{
				if ( localSpace )
					stm = Matrix4x4.identity;
				else
				{
					//stm = transform.worldToLocalMatrix * target.transform.localToWorldMatrix;
					stm = target.transform.localToWorldMatrix * transform.worldToLocalMatrix;
				}

				job.offset			= offset;
				job.gap				= gap;
				job.shrink			= shrink;
				job.verts			= verts;
				job.stm				= stm;
				job.skinnedVerts	= targetVerts;
				job.index			= jindexes;
				job.weight			= jweight;
				job.weights			= jweights;
				job.dist			= jdist;
				job.barys			= jbarys;
				job.ixs				= jixs;

				jobHandle = job.Schedule(verts.Length, 64);
				jobHandle.Complete();
			}

			mesh.SetVertices(verts);
			RecalcNormals();
			if ( recalcBounds )
			{
				mesh.bounds = target.mesh.bounds;
				//mesh.RecalculateBounds();
			}
		}

		int[] FindFacesUsing(Vector3[] _verts, Vector3 p, Vector3 n)
		{
			List<int> faces = new List<int>();
			Vector3 v = Vector3.zero;

			for ( int i = 0; i < tris.Length; i += 3 )
			{
				v = _verts[tris[i]];
				if ( v.x == p.x && v.y == p.y && v.z == p.z )
				{
					if ( n.Equals(norms[tris[i]]) )
						faces.Add(i / 3);
				}
				else
				{
					v = _verts[tris[i + 1]];
					if ( v.x == p.x && v.y == p.y && v.z == p.z )
					{
						if ( n.Equals(norms[tris[i + 1]]) )
							faces.Add(i / 3);
					}
					else
					{
						v = _verts[tris[i + 2]];
						if ( v.x == p.x && v.y == p.y && v.z == p.z )
						{
							if ( n.Equals(norms[tris[i + 2]]) )
								faces.Add(i / 3);
						}
					}
				}
			}

			return faces.ToArray();
		}

		public void RecalcNormals()
		{
			if ( NormalMethod == MegaNormalMethod.Unity )
				mesh.RecalculateNormals();
			else
				CalcNormalsBurst(mesh);
		}

		public void BuildNormalMappingNew(Mesh mesh, bool force)
		{
			tris = mesh.triangles;
			int tricount = tris.Length;

			if ( indexmap.Count == 0 || newmap.Count == 0 || force )
			{
				indexmap.Clear();
				newmap.Clear();

				if ( newMapping == null || newMapping.Length == 0 || force )
				{
					norms = mesh.normals;
					MegaNormMap mapping = new MegaNormMap();

					Vector3[] _verts = mesh.vertices;

					for ( int i = 0; i < _verts.Length; i++ )
					{
						mapping.faces = FindFacesUsing(_verts, _verts[i], norms[i]);

						indexmap.Add(newmap.Count); // index into the mapping data to find the count

						newmap.Add(mapping.faces.Length);
						for ( int f = 0; f < mapping.faces.Length; f++ )
							newmap.Add(mapping.faces[f]);
					}
				}
			}

			faceCount	= new NativeArray<int>(indexmap.ToArray(), Allocator.Persistent);
			newMapping	= new NativeArray<int>(newmap.ToArray(), Allocator.Persistent);
			faceTris	= new NativeArray<int>(tris, Allocator.Persistent);
			facenormals	= new NativeArray<Vector3>(tricount / 3, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}

		public void CalcNormalsBurst(Mesh mesh)
		{
			if ( facenormals.IsCreated )
			{
				faceNormsJob.verts		= verts;
				faceNormsJob.tris		= faceTris;
				faceNormsJob.facenorms	= facenormals;
				faceNormsJobHandle		= faceNormsJob.Schedule(tris.Length / 3, 64);
				calcNormsJob.indexmap	= faceCount;
				calcNormsJob.normals	= normals;
				calcNormsJob.faces		= faceTris;
				calcNormsJob.mapping	= newMapping;
				calcNormsJob.facenorms	= facenormals;
				calcNormsJobHandle		= calcNormsJob.Schedule(verts.Length, 64, faceNormsJobHandle);
				calcNormsJobHandle.Complete();

				mesh.SetNormals(normals);
			}
		}

		public void AttachChildren()
		{
			for ( int i = 0; i < transform.childCount; i++ )
			{
				GameObject obj = transform.GetChild(i).gameObject;

				MegaAttachWrap attach = obj.GetComponent<MegaAttachWrap>();

				if ( !attach )
					attach = obj.AddComponent<MegaAttachWrap>();

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

				MegaAttachWrap attach = obj.GetComponent<MegaAttachWrap>();

				if ( attach )
					attach.enabled = false;
			}
		}
	}
}