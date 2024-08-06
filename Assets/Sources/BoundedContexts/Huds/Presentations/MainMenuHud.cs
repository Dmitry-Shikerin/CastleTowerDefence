using System.Collections.Generic;
using Sources.Frameworks.GameServices.Volumes.Presentations;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Huds;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using Sources.Frameworks.YandexSdcFramework.Presentations.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.BoundedContexts.Huds.Presentations
{
    public class MainMenuHud : MonoBehaviour, IHud
    {
        [SerializeField] private List<LeaderBoardElementView> _leaderBoardElementViews;
        [SerializeField] private VolumeView _musicVolumeView;
        [SerializeField] private VolumeView _soundVolumeView;
        [SerializeField] private UiCollector _uiCollector;
        
        public IReadOnlyList<LeaderBoardElementView> LeaderBoardElementViews => _leaderBoardElementViews;
        public VolumeView MusicVolumeView => _musicVolumeView;
        public VolumeView SoundVolumeView => _soundVolumeView;
        public UiCollector UiCollector => _uiCollector;
    }
}