using UnityEngine;
using System.Collections;

namespace MegaFiers
{
	public class MegaMeshCombiner : MonoBehaviour
	{
		[ContextMenu("Combine Mesh")]
		public void Combine()
		{
			Combine(gameObject);
		}

		static public GameObject Combine(GameObject gameObject)
		{
			GameObject newobj = new GameObject();
			newobj.name = gameObject.name + " Combined";
			newobj.transform.position = gameObject.transform.position;
			newobj.transform.rotation = gameObject.transform.rotation;

			Vector3 basePosition = gameObject.transform.position;
			Quaternion baseRotation = gameObject.transform.rotation;
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;

			ArrayList materials = new ArrayList();
			ArrayList combineInstanceArrays = new ArrayList();
			MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

			foreach ( MeshFilter meshFilter in meshFilters )
			{
				MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();

				if ( !meshRenderer || !meshFilter.sharedMesh || meshRenderer.sharedMaterials.Length != meshFilter.sharedMesh.subMeshCount )
					continue;

				for ( int s = 0; s < meshFilter.sharedMesh.subMeshCount; s++ )
				{
					int materialArrayIndex = Contains(materials, meshRenderer.sharedMaterials[s].name);
					if ( materialArrayIndex == -1 )
					{
						materials.Add(meshRenderer.sharedMaterials[s]);
						materialArrayIndex = materials.Count - 1;
					}
					combineInstanceArrays.Add(new ArrayList());

					CombineInstance combineInstance = new CombineInstance();
					combineInstance.transform = meshRenderer.transform.localToWorldMatrix;
					combineInstance.subMeshIndex = s;
					combineInstance.mesh = meshFilter.sharedMesh;
					(combineInstanceArrays[materialArrayIndex] as ArrayList).Add(combineInstance);
				}
			}

			MeshFilter meshFilterCombine = newobj.GetComponent<MeshFilter>();
			if ( meshFilterCombine == null )
				meshFilterCombine = newobj.AddComponent<MeshFilter>();

			MeshRenderer meshRendererCombine = newobj.GetComponent<MeshRenderer>();
			if ( meshRendererCombine == null )
				meshRendererCombine = newobj.AddComponent<MeshRenderer>();

			Mesh[] meshes = new Mesh[materials.Count];
			CombineInstance[] combineInstances = new CombineInstance[materials.Count];

			for ( int m = 0; m < materials.Count; m++ )
			{
				CombineInstance[] combineInstanceArray = (combineInstanceArrays[m] as ArrayList).ToArray(typeof(CombineInstance)) as CombineInstance[];
				meshes[m] = new Mesh();
				meshes[m].CombineMeshes(combineInstanceArray, true, true);

				combineInstances[m] = new CombineInstance();
				combineInstances[m].mesh = meshes[m];
				combineInstances[m].subMeshIndex = 0;
			}

			meshFilterCombine.sharedMesh = new Mesh();
			meshFilterCombine.sharedMesh.CombineMeshes(combineInstances, false, false);

			Material[] materialsArray = materials.ToArray(typeof(Material)) as Material[];
			meshRendererCombine.materials = materialsArray;

			gameObject.transform.position = basePosition;
			gameObject.transform.rotation = baseRotation;

			return newobj;
		}

		static int Contains(ArrayList searchList, string searchName)
		{
			for ( int i = 0; i < searchList.Count; i++ )
			{
				if ( ((Material)searchList[i]).name == searchName )
					return i;
			}
			return -1;
		}
	}
}