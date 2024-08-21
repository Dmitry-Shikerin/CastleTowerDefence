#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	public class MegaSubDivideWindow : EditorWindow
	{
		public int	subDivides = 2;

		[MenuItem("MegaFiers/Sub Divide Mesh")]
		static public void Init()
		{
			MegaSubDivideWindow window = ScriptableObject.CreateInstance<MegaSubDivideWindow>();
			window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
			window.ShowUtility();

			EditorWindow.GetWindow(typeof(MegaSubDivideWindow), false, "Sub Divide Mesh");
		}

		void OnGUI()
		{
			if ( Selection.activeGameObject == null )
				return;

			subDivides = EditorGUILayout.IntSlider("Sub Divide Levels", subDivides, 1, 8);

			if ( GUILayout.Button("Sub Divide") )
			{
				GameObject obj = Selection.activeGameObject;

				if ( obj )
				{
					MeshFilter mf = obj.GetComponent<MeshFilter>();

					if ( mf && mf.sharedMesh )
					{
						Mesh newmesh = MegaUtils.DupMesh(mf.sharedMesh, "");

						MeshHelper.Subdivide(newmesh, subDivides);

						mf.sharedMesh = newmesh;
					}
				}
			}
		}
	}
}
#endif