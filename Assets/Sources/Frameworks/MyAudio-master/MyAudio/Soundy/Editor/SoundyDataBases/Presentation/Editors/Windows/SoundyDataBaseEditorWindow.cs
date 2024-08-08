using System.Linq;
using Doozy.Editor.EditorUI.Windows.Internal;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.DataBases.Editors;
using MyAudios.Soundy.Editor.SoundyDataBases.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Editors;
using MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using MyAudios.Soundy.Sources.Settings.Domain.Configs;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.DataBases.Windows
{
    public class SoundyDataBaseEditorWindow : FluidWindow<SoundyDataBaseEditorWindow>
    {
        private SoundyDatabase _database;
        private AudioClip _plugAudio;

        [MenuItem("Tools/SoundyDataBase")]
        public static void ShowWindow() =>
            GetWindow<SoundyDataBaseEditorWindow>("Soundy Data Base");

        protected override void CreateGUI()
        {
            root.Clear();

            _database = SoundySettings.Database;
            _plugAudio = Resources.Load<AudioClip>("MyAudios/Soundy/Resources/Soundy/Plugs/Christmas Villain Loop");
            SoundyDataBaseEditor editor = 
                (SoundyDataBaseEditor)UnityEditor.Editor.CreateEditor(_database);
            VisualElement editorRoot = editor.CreateInspectorGUI();
            editorRoot
                .Bind(editor.serializedObject);

            ISoundyDataBaseView view = new SoundyDataBaseViewFactory().Create(
                _database, SoundySettings.Instance);
            
            root
                .Add(view.Root);
        }
    }
}