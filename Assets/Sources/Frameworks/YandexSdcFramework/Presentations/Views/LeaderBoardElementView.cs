using Sources.Controllers.YandexSDK;
using Sources.Presentations.Views;
using Sources.PresentationsInterfaces.Views.YandexSDK;
using TMPro;
using UnityEngine;

namespace Sources.Frameworks.YandexSdcFramework.Presentations.Views
{
    public class LeaderBoardElementView : PresentableView<LeaderBoardElementPresenter>, ILeaderBoardElementView
    {
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private TMP_Text _playerRank;
        [SerializeField] private TMP_Text _playerScore;

        public void SetName(string name) =>
            _playerName.text = name;

        public void SetRank(string rank) =>
            _playerRank.text = rank;

        public void SetScore(string score) =>
            _playerScore.text = score;
    }
}