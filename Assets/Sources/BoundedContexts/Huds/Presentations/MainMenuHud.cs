using System.Collections.Generic;
using Sources.Frameworks.GameServices.Volumes.Presentations;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Huds;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using Sources.Frameworks.YandexSdcFramework.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.Huds.Presentations
{
    public class MainMenuHud : MonoBehaviour, IHud
    {
        [SerializeField] private List<LeaderBoardElementView> _leaderBoardElementViews;
        [SerializeField] private MusicChangerView _musicChangerView;
        [SerializeField] private SoundsChangerView _soundsChangerView;
        [SerializeField] private UiCollector _uiCollector;
        
        public IReadOnlyList<LeaderBoardElementView> LeaderBoardElementViews => _leaderBoardElementViews;
        public MusicChangerView MusicChangerView => _musicChangerView;
        public SoundsChangerView SoundsChangerView => _soundsChangerView;
        public UiCollector UiCollector => _uiCollector;
    }
}