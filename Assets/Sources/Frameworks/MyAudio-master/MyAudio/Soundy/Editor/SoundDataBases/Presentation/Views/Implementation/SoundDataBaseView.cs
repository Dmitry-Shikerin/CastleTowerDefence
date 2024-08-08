using System;
using System.Collections.Generic;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.NewSoundContents.Presentation.Controlls;
using MyAudios.Soundy.Editor.SoundDataBases.Controllers;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Controls;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Views.Interfaces;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Implementation
{
    public class SoundDataBaseView : ISoundDataBaseView
    {
        private SoundDataBasePresenter _presenter;
        private SoundDataBaseHeaderVisualElement _headerVisualElement;
        private NewSoundContentVisualElement _newSoundContentVisualElement;
        private SoundDataBaseAudioMixerVisualElement _audioMixerVisualElement;
        private VisualElement _spaseElement;
        private ScrollView _scrollView;
        private VisualElement _soundGroupsContainer;
        private List<ISoundGroupView> _soundGroups;
        private ISoundyDataBaseView _soundyDataBaseView;

        public VisualElement Root { get; private set; }

        public void Construct(SoundDataBasePresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            CreateView();
            Initialize();
        }

        public void CreateView()
        {
            _headerVisualElement = new SoundDataBaseHeaderVisualElement();
            _newSoundContentVisualElement = new NewSoundContentVisualElement();
            _audioMixerVisualElement = new SoundDataBaseAudioMixerVisualElement();
            VisualElement spaseLine = DesignUtils.row
                .SetStyleMaxHeight(4)
                .SetStyleMinHeight(4)
                .SetStyleBackgroundColor(EditorColors.Default.Background)
                .SetStyleBorderRadius(4,4,4,4);
            _spaseElement = DesignUtils.column
                .SetStyleMinWidth(25)
                .SetStyleMaxHeight(25)
                .AddChild(DesignUtils.row)
                .AddChild(spaseLine)
                .AddChild(DesignUtils.row);
            _scrollView = new ScrollView();
            _soundGroupsContainer = DesignUtils
                .column
                .SetStyleBackgroundColor(EditorColors.Default.Background)
                .AddSpace(4)
                .AddChild(_scrollView);
            
            Root = DesignUtils.column
                .AddChild(_headerVisualElement)
                .AddSpace(4)
                .AddChild(_audioMixerVisualElement)
                .AddSpace(4)
                .AddChild(_spaseElement)
                .AddSpace(4)
                .AddChild(_newSoundContentVisualElement)
                .AddChild(_soundGroupsContainer);
        }

        public void Initialize()
        {
            _newSoundContentVisualElement.CreateButton.SetOnClick(() 
                => _presenter.CreateSoundGroup(_newSoundContentVisualElement.SoundGroupTextField.value));
            _headerVisualElement.PingAssetButton.SetOnClick(() 
                => Selection.activeObject = _presenter.GetDataBase());
            _headerVisualElement.RenameButton.SetOnClick(() => 
                _presenter.RenameDataBase(_headerVisualElement.SoundGroupTextField.value));
            _headerVisualElement.RemoveButton.SetOnClick(() => _presenter.RemoveDataBase());
            _audioMixerVisualElement.ObjectField
                .RegisterCallback<ChangeEvent<AudioMixerGroup>>((value) 
                    => _presenter.ChangeAudioMixerGroup(value.newValue));
            _soundGroups = new List<ISoundGroupView>();
            _presenter.Initialize();
        }

        public void RemoveSoundGroup(ISoundGroupView soundGroupView)
        {
            _soundGroups.Remove(soundGroupView);
            Root.Remove(soundGroupView.Root);
        }
        
        public void StopAllSoundGroup()
        {
            foreach (ISoundGroupView soundGroup in _soundGroups)
                soundGroup.StopPlaySound();
        }

        public void SetName(string name) =>
            _headerVisualElement.SoundGroupTextField.value = name;

        public void SetAudioMixerGroup(AudioMixerGroup audioMixerGroup) =>
            _audioMixerVisualElement.ObjectField.SetValueWithoutNotify(audioMixerGroup);

        public void RenameDataBaseButtons() =>
            _soundyDataBaseView.RenameButtons();
        
        public void SetSoundyDataBaseView(ISoundyDataBaseView view) =>
            _soundyDataBaseView = view ?? throw new ArgumentNullException(nameof(view));

        public void AddSoundGroup(ISoundGroupView soundGroupView)
        {
            _scrollView
                .AddChild(soundGroupView.Root)
                .AddSpace(4);
            _soundGroups.Add(soundGroupView);
        }


        public void Dispose()
        {
            Root.RemoveFromHierarchy();
            _soundyDataBaseView.UpdateDataBase();
            _presenter.Dispose();
        }
    }
}