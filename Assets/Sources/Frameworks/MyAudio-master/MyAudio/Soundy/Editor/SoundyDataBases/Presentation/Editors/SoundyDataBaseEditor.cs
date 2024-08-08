using System.Collections.Generic;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Editors
{
    [CustomEditor(typeof(SoundyDatabase))]
    public class SoundyDataBaseEditor : UnityEditor.Editor
    {
        public SoundyDatabase Target;

        private SerializedProperty DatabaseNames { get; set; }
        private SerializedProperty SoundDatabases { get; set; }

        private VisualElement Root { get; set; }
        private FluidComponentHeader Header { get; set; }

        private FluidListView DatabaseNamesField { get; set; }
        private FluidListView SoundDatabasesField { get; set; }

        private FluidButton DatabasesButton { get; set; }

        private List<FluidFoldout> Foldouts { get; set; }

        private List<GameObject> _audioSources = new List<GameObject>();

        public override VisualElement CreateInspectorGUI()
        {
            FindProperties();
            InitializeEditor();
            Compose();

            return Root;
        }

        private void Refresh()
        {
            InitializeNames();
            InitializeDataBases();
        }

        private void FindProperties()
        {
            DatabaseNames = serializedObject.FindProperty(nameof(SoundyDatabase.DatabaseNames));
            SoundDatabases = serializedObject.FindProperty(nameof(SoundyDatabase.SoundDatabases));
        }

        private void InitializeEditor()
        {
            Root = new VisualElement();

            InitializeHeader();
            InitializeNames();
            InitializeDataBases();
        }

        private void Compose()
        {
            FluidFoldout namesFoldout =
                new FluidFoldout()
                    .AddContent(DatabaseNamesField)
                    .SetLabelText("Names");

            FluidFoldout dataBaseFoldout =
                new FluidFoldout()
                    .AddContent(SoundDatabasesField)
                    .SetLabelText("Databases");

            FluidButton databaseButton =
                FluidButton
                    .Get()
                    .SetName("Data Base")
                    .SetLabelText("Data Base")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Sound)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);

            FluidButton settingsButton =
                FluidButton
                    .Get()
                    .SetName("Settings")
                    .SetLabelText("Settings")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Settings)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);

            VisualElement buttonsRow = DesignUtils.row;
            buttonsRow
                .AddSpaceBlock(2)
                .AddChild(databaseButton)
                .AddSpaceBlock(5)
                .AddChild(settingsButton);

            TextField addDataBaseTextField = new TextField();
            addDataBaseTextField.SetStyleMinWidth(250);

            FluidButton addDataBaseButton =
                FluidButton
                    .Get()
                    .SetName("New Sound Data Base")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.CategoryPlus)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained)
                    .AddOnClick(() =>
                    {
                        if (string.IsNullOrEmpty(addDataBaseTextField.value))
                            return;

                        Target.CreateSoundDatabase(
                            "Assets/MyAudios/Resources/Soundy/DataBases",
                            addDataBaseTextField.value);
                    });

            FluidButton undoDataBaseButton =
                FluidButton
                    .Get()
                    .SetName("Undo Sound Data Base")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Close)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);


            VisualElement addNewSoundDataRow = DesignUtils.row;
            addNewSoundDataRow
                .AddChild(addDataBaseTextField)
                .AddChild(addDataBaseButton)
                .AddChild(undoDataBaseButton);

            FluidFoldout newSoundDataBaseFoldout =
                new FluidFoldout()
                    .AddContent(addNewSoundDataRow);

            FluidButton newSoundDatabaseButton =
                FluidButton
                    .Get()
                    .SetName("New Sound Data Base")
                    .SetLabelText("New Sound Data Base")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.CategoryPlus)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained)
                    .AddOnClick(() =>
                    {
                        if (newSoundDataBaseFoldout.isOn == false)
                            newSoundDataBaseFoldout.Open();
                        else
                            newSoundDataBaseFoldout.Close();
                    });

            FluidButton refreshButton =
                FluidButton
                    .Get()
                    .SetName("Refresh")
                    .SetLabelText("Refresh")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Refresh)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);

            VisualElement secondButtonsRow = DesignUtils.row;
            secondButtonsRow
                .AddChild(newSoundDatabaseButton)
                .AddSpaceBlock(5)
                .AddChild(refreshButton)
                .AddSpaceBlock(5);

            FluidButton saveButton =
                FluidButton
                    .Get()
                    .SetName("Save")
                    .SetLabelText("Save")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);

            FluidButton soundDatabasesButton =
                FluidButton
                    .Get()
                    .SetName("Sound Databases")
                    .SetLabelText("Sound Databases")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Settings)
                    .SetElementSize(ElementSize.Normal)
                    .SetButtonStyle(ButtonStyle.Contained);

            VisualElement thirdButtonsRow = DesignUtils.row;
            thirdButtonsRow
                .AddSpaceBlock(5)
                .AddChild(saveButton)
                .AddSpaceBlock(5)
                .AddChild(soundDatabasesButton);


            Root
                .AddChild(Header)
                .AddSpaceBlock(2)
                .AddChild(namesFoldout)
                .AddSpaceBlock()
                .AddChild(dataBaseFoldout)
                // .AddChild(dataBaseFoldout)
                // .AddSpaceBlock(2)
                // .AddChild(buttonsRow)
                // .AddChild(secondButtonsRow)
                // .AddChild(thirdButtonsRow)
                // .AddChild(newSoundDataBaseFoldout)
                ;
        }

        private void InitializeNames()
        {
            DatabaseNamesField =
                DesignUtils.NewStringListView(DatabaseNames, "Database Names", "");
        }

        private void InitializeDataBases()
        {
            SoundDatabasesField = DesignUtils.NewObjectListView(
                SoundDatabases, "Sound Databases", "", typeof(SoundDatabase));
        }

        private void InitializeHeader()
        {
            Header =
                FluidComponentHeader
                    .Get()
                    .SetComponentNameText("Soundy Database")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Sound)
                    .SetAccentColor(EditorColors.EditorUI.Orange)
                ;
        }
    }
}