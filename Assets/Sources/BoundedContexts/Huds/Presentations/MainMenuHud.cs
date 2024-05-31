using System.Collections.Generic;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Huds;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using Sources.Frameworks.YandexSdcFramework.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.Huds.Presentations
{
    public class MainMenuHud : MonoBehaviour, IHud
    {
        [SerializeField] private List<LeaderBoardElementView> _leaderBoardElementViews;
        [SerializeField] private Transform _leaderBoardContainer;
        [SerializeField] private UiCollector _uiCollector;
        
        public IReadOnlyList<LeaderBoardElementView> LeaderBoardElementViews => _leaderBoardElementViews;
        public Transform LeaderBoardContainer => _leaderBoardContainer;
        public UiCollector UiCollector => _uiCollector;
    }
}