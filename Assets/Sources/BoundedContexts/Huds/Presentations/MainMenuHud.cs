using System.Collections.Generic;
using Sources.Frameworks.YandexSdcFramework.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.Huds.Presentations
{
    public class MainMenuHud : MonoBehaviour
    {
        [SerializeField] private List<LeaderBoardElementView> _leaderBoardElementViews;
        [SerializeField] private Transform _leaderBoardContainer;
        
        public IReadOnlyList<LeaderBoardElementView> LeaderBoardElementViews => _leaderBoardElementViews;
        public Transform LeaderBoardContainer => _leaderBoardContainer;
    }
}