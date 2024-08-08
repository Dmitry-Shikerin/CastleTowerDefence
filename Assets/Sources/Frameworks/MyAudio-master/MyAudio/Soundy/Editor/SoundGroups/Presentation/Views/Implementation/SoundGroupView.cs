using System;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundGroups.Controllers;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Controls;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Interfaces;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Implementation
{
    public class SoundGroupView : ISoundGroupView
    {
        private SoundGroupPresenter _presenter;
        private SoundGroupVisualElement _visualElement;
        private ISoundDataBaseView _soundDataBaseView;
        
        public VisualElement Root { get; private set; }

        public void Construct(SoundGroupPresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            CreateView();
            Initialize();
        }

        public void CreateView()
        {
            _visualElement = new SoundGroupVisualElement();
            Root = _visualElement;
        }

        public void Initialize()
        {
            _visualElement.PlayButton
                .SetOnClick(() => _presenter.ChangeSoundGroupState());
            _visualElement.DeleteButton.SetOnClick(() => _presenter.RemoveSoundGroup());
            _visualElement.SoundGroupDataButton.SetOnClick(() => _presenter.ShowSoundGroupData());
            _visualElement.Slider.sliderTracker.RegisterCallback<MouseDownEvent>((mouse) => _presenter.MouseDown(mouse.button)); ;
            _presenter.Initialize();
        }

        public void Dispose()
        {
            Root.RemoveFromHierarchy();
            _presenter.Dispose();
        }

        public void SetPlayIcon() =>
            _visualElement.PlayButton.SetIcon(EditorSpriteSheets.EditorUI.Icons.Play);

        public void SetStopIcon() =>
            _visualElement.PlayButton.SetIcon(EditorSpriteSheets.EditorUI.Icons.Stop);

        public void SetSliderValue(float audioSourceTime) =>
            _visualElement.Slider.SetSliderValue(audioSourceTime);

        public void SetSliderMaxValue(float maxValue)
        {
            _visualElement.Slider.slider.value = 0;
            _visualElement.Slider.slider.lowValue = 0;
            _visualElement.Slider.slider.highValue = maxValue;
        }

        public void StopAllAudioGroup() =>
            _soundDataBaseView.StopAllSoundGroup();

        public void SetDataBase(ISoundDataBaseView soundDataBaseView) =>
            _soundDataBaseView = soundDataBaseView;

        public void StopPlaySound() =>
            _presenter.StopSound();

        public void SetSoundGroupName(string soundGroupName)
        {
            _visualElement
                .SoundGroupDataButton
                .SetLabelText(soundGroupName);
        }

        public void StopAllAudioData()
        {
            
        }
    }
}