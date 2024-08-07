using Doozy.Engine.Soundy;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.SoundGroupDatas.Infrastructure.Factories;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using MyAudios.Soundy.Sources.Settings.Domain.Configs;
using UnityEditor;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Editors
{
    [CustomEditor(typeof(SoundGroupData))]
    public class SoundGroupDataEditor : UnityEditor.Editor
    {
        private SoundDatabase _soundDatabase;
        private SoundGroupData _soundGroupData;
        private VisualElement _root;

        public override VisualElement CreateInspectorGUI()
        {
            FindProperties();
            ISoundGroupDataView view = new SoundGroupDataViewFactory().Create(_soundGroupData, _soundDatabase);
            _root = new VisualElement()
                .AddChild(view.Root);

            return _root;
        }

        private void FindProperties()
        {
            _soundGroupData = (SoundGroupData)serializedObject.targetObject;
            _soundDatabase = SoundySettings.Database.GetSoundDatabase(_soundGroupData.DatabaseName);
        }
    }
}