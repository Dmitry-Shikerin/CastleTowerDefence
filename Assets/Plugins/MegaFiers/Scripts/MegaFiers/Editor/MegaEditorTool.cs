#if false
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

namespace MegaFiers
{
	// Tagging a class with the EditorTool attribute and no target type registers a global tool. Global tools are valid for any selection, and are accessible through the top left toolbar in the editor.
	[EditorTool("Platform Tool")]
	class MegaEditorTool : EditorTool
	{
		// Serialize this value to set a default value in the Inspector.
		[SerializeField]
		Texture2D m_ToolIcon;

		GUIContent m_IconContent;

		void OnEnable()
		{
			m_IconContent = new GUIContent()
			{
				image = m_ToolIcon,
				text = "Deform Object",
				tooltip = "Add Modify Object Component To Object"
			};
		}

		public override GUIContent toolbarIcon
		{
			get { return m_IconContent; }
		}

		// This is called for each window that your tool is active in. Put the functionality of your tool here.
		public override void OnToolGUI(EditorWindow window)
		{
			EditorGUI.BeginChangeCheck();

			Vector3 position = Tools.handlePosition;

			using ( new Handles.DrawingScope(Color.green) )
			{
				position = Handles.Slider(position, Vector3.right);
			}

			if ( EditorGUI.EndChangeCheck() )
			{
				Vector3 delta = position - Tools.handlePosition;

				Undo.RecordObjects(Selection.transforms, "Add Modify Object");

				foreach ( var transform in Selection.transforms )
					transform.position += delta;
			}
		}
	}
}
#endif