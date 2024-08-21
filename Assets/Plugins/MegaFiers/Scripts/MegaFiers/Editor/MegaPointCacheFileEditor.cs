using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaPointCacheFile))]
	public class MegaPointCacheFileEditor : Editor
	{
		static public void DeleteMapping(MegaPointCacheFile file)
		{
			if ( file )
			{
				file.Verts = null;
				EditorUtility.SetDirty(file);
				AssetDatabase.SaveAssets();
			}
		}
	}
}