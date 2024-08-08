using System;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.AudioDatas.Infrastructure.Factories;
using MyAudios.Soundy.Editor.AudioDatas.Presentation.View.Interfaces;
using MyAudios.Soundy.Editor.Presenters.Controllers;
using MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Constants;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using UnityEngine;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Controllers
{
    public class SoundGroupDataPresenter : IPresenter
    {
        private readonly SoundGroupData _soundGroupData;
        private readonly SoundDatabase _soundDatabase;
        private readonly ISoundGroupDataView _view;
        private readonly AudioDataViewFactory _audioDataViewFactory;

        public SoundGroupDataPresenter(
            SoundGroupData soundGroupData,
            SoundDatabase soundDatabase,
            ISoundGroupDataView view,
            AudioDataViewFactory audioDataViewFactory)
        {
            _soundGroupData = soundGroupData ?? throw new ArgumentNullException(nameof(soundGroupData));
            _soundDatabase = soundDatabase ?? throw new ArgumentNullException(nameof(soundDatabase));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _audioDataViewFactory = audioDataViewFactory ?? throw new ArgumentNullException(nameof(audioDataViewFactory));
        }

        public void Initialize()
        {
            AddAudioDatas();
            _view.SetLoop(_soundGroupData.Loop);
            _view.SetSoundName(_soundGroupData.SoundName);
            _view.SetIsOnButtonTab(_soundGroupData.Mode);
            _view.SetVolume(
                new Vector2(_soundGroupData.Volume.MinValue, _soundGroupData.Volume.MaxValue),
                new Vector2(SoundGroupDataConst.MinVolume, SoundGroupDataConst.MaxVolume));
            _view.SetPitch(
                new Vector2(_soundGroupData.Pitch.MinValue, _soundGroupData.Pitch.MaxValue),
                new Vector2(SoundGroupDataConst.MinPitch, SoundGroupDataConst.MaxPitch));
            _view.SetSpatialBlend(
                _soundGroupData.SpatialBlend,
                new Vector2(SoundGroupDataConst.MinSpatialBlend, SoundGroupDataConst.MaxSpatialBlend));
        }

        public void Dispose()
        {
            
        }

        private void AddAudioDatas()
        {
            foreach (AudioData audioData in _soundGroupData.Sounds)
            {
                IAudioDataView view = _audioDataViewFactory.Create(audioData, _soundGroupData);
                view.SetSoundGroupData(_view);
                _view.AddAudioData(view);
            }
        }
        
        public void SetPlayMode(SoundGroupData.PlayMode playMode) =>
            _soundGroupData.Mode = playMode;

        public void CreateAudioData()
        {
            AudioData audioData = _soundGroupData.AddAudioData();
            IAudioDataView view = _audioDataViewFactory.Create(audioData, _soundGroupData);
            _view.AddAudioData(view);
        }

        public void ChangeVolume(Vector2 volume)
        {
            _soundGroupData.Volume.MinValue = volume.x;
            _soundGroupData.Volume.MaxValue = volume.y;
        }

        public void ChangePitch(Vector2 pitch)
        {
            _soundGroupData.Pitch.MinValue = pitch.x;
            _soundGroupData.Pitch.MaxValue = pitch.y;
        }

        public void ChangeSpatialBlend(float value) =>
            _soundGroupData.SpatialBlend = value;

        public SoundGroupData GetSoundGroupData() =>
            _soundGroupData;

        public void ChangeLoopState(bool value) =>
            _soundGroupData.Loop = value;
    }
}