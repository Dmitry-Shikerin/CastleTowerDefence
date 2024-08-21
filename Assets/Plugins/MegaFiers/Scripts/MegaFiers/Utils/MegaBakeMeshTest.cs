using System.Collections.Generic;
using UnityEngine;

// This script is an example script, it will bake modifier animations to blendshapes on the object, then it will create a prefab
// from that object and place them in the scene with randomized blends. This could be used to bake loads of different deformations
// to one object and on pipelines that support GPU instanced Skinned meshes you could add 1000's of the same object with big differences
// all rendered with one draw call, and you could change the blends at any time.
// You add the script to a Mega Deforming object, that object should also have an Animator attached and the abinmator have states that animate
// modifier values to create the shapes. You then add channels here providing the state name and how many frames you want that to bake to
// for example for a 90 bend you would want 3 or frames to get a proper bend in the blendshape, but for a stretch you may only need one frame
namespace MegaFiers
{
	public class MegaBakeMeshTest : MonoBehaviour
	{
		[System.Serializable]
		public class BlendChannel
		{
			public string			name;
			public string			stateName;
			public int				frames	= 1;
		}

		public MegaModifyObject		target;
		public List<BlendChannel>	channels = new List<BlendChannel>();
		public Animator				animator;

		[ContextMenu("Create BlendShapes")]
		public void BuildBlendshapes()
		{
			animator = GetComponent<Animator>();

			if ( target && animator && target.mesh && channels.Count > 0 )
			{
				bool auto = target.autoDisable;
				target.autoDisable = false;
				MegaBend bend = target.FindMod<MegaBend>();
				Bounds bounds = new Bounds();

				for ( int i = 0; i < channels.Count; i++ )
				{
					BlendChannel channel = channels[i];

					MegaBlendshapeWorkshop.RemoveChannel(target, channel.name);

					int id = Animator.StringToHash(channel.stateName);

					if ( animator.HasState(0, id) )
					{
						for ( int f = 0; f < channel.frames; f++ )
						{
							float t = (float)(f + 1) / (float)channel.frames;
							if ( t > 0.999f )
								t = 0.999f;

							animator.Play(id, 0, t);
							animator.Update(0.0f);
							target.ForceUpdate();
							target.ModifyObject();

							bounds.Encapsulate(target.mesh.bounds);
							// Keep track of bounds so we can set the end bounds value

							MegaBlendshapeWorkshop.AddBlendShapeFrame(target, channel.name, t * 100.0f);
						}

						animator.Play(id, 0, 0.0f);
						animator.Update(0.0f);
					}
				}

				target.autoDisable = auto;
				//GameObject obj = CreatePrefab();

				int index = 0;

				Mesh newmesh = MegaUtils.DupMesh(target.originalMesh, "");
				newmesh.bounds = bounds;
				Material[] mats = target.GetComponent<SkinnedMeshRenderer>().sharedMaterials;

				// Now we place a load in a grid with each one having different blends
				for ( int x = 0; x < 10; x++ )
				{
					for ( int y = 0; y < 10; y++ )
					{
						//GameObject o = Instantiate(obj);
						GameObject o = new GameObject();
						o.name = name + index++;

						o.transform.position = new Vector3(x * 12.0f, 0.0f, y * 12.0f);
						o.transform.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f);

						SkinnedMeshRenderer sr = o.AddComponent<SkinnedMeshRenderer>();

						sr.sharedMesh = newmesh;
						sr.sharedMaterials = mats;
						sr.localBounds = bounds;

						for ( int i = 0; i < newmesh.blendShapeCount; i++ )
							sr.SetBlendShapeWeight(i, Random.value * 100.0f);
					}
				}
			}
		}

#if false
		GameObject CreatePrefab()
		{
			GameObject newobj = Object.Instantiate(gameObject);

			if ( !System.IO.Directory.Exists("Assets/MegaPrefabs") )
				AssetDatabase.CreateFolder("Assets", "MegaPrefabs");

			MegaModifyObject mod = newobj.GetComponent<MegaModifyObject>();

			MegaModifier[] mods = newobj.GetComponents<MegaModifier>();

			for ( int i = 0; i < mods.Length; i++ )
			{
				DestroyImmediate(mods[i]);
				mods[i] = null;
			}

			Mesh mesh = target.originalMesh;
			mesh.name = mesh.name + " Baked";
			DestroyImmediate(mod);

			SkinnedMeshRenderer oldsr = GetComponent<SkinnedMeshRenderer>();

			SkinnedMeshRenderer sr = newobj.GetComponent<SkinnedMeshRenderer>();
			sr.sharedMesh = mesh;
			sr.sharedMaterials = oldsr.sharedMaterials;

			Animator animator = newobj.GetComponent<Animator>();
			if ( animator )
				DestroyImmediate(animator);

			CustomTreesTest ctt = newobj.GetComponent<CustomTreesTest>();
			if ( ctt )
				DestroyImmediate(ctt);

			if ( mesh )
			{
				string mname = mesh.name;
				int ix = mname.IndexOf("Instance");
				if ( ix != -1 )
					mname = mname.Remove(ix);

				string meshpath = "Assets/MegaPrefabs/" + mname + "_baked.asset";
				//AssetDatabase.CreateAsset(mesh, meshpath);
				//AssetDatabase.SaveAssets();
				//AssetDatabase.Refresh();
			}

			Object prefab = PrefabUtility.SaveAsPrefabAsset(newobj, "Assets/MegaPrefabs/" + newobj.name + "_Prefab.prefab");

			return newobj;
		}
#endif
	}
}