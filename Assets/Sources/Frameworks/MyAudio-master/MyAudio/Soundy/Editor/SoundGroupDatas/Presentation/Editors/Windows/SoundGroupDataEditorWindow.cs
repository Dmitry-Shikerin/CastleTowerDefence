using Doozy.Editor.EditorUI.Windows.Internal;
using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.DataBases.Editors;
using MyAudios.Soundy.Editor.SoundGroupDatas.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Editors.Windows
{
    public class SoundGroupDataEditorWindow : FluidWindow<SoundGroupDataEditorWindow>
    {
        private static SoundGroupData SoundGroupData { get; set; }
        
        public static void Open(SoundGroupData soundGroupData)
        {
            SoundGroupData = soundGroupData;
            SoundGroupDataEditorWindow window = GetWindow<SoundGroupDataEditorWindow>();
            window.titleContent = new GUIContent("Sound Group");
            window.Show();
        }
        
        protected override void CreateGUI()
        {
            root.Clear();
            
            SoundGroupDataEditor editor = (SoundGroupDataEditor)UnityEditor.Editor.CreateEditor(SoundGroupData);
            VisualElement editorRoot = editor.CreateInspectorGUI();
            editorRoot
                .Bind(editor.serializedObject);
            
            root
                .AddChild(editorRoot)
                .SetStylePadding(15, 15, 15, 15)
                ;
        }
    }
}