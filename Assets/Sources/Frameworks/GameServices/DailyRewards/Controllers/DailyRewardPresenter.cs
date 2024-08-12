using System;
using System.Net.Sockets;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.GameServices.DailyRewards.Domain;
using Sources.Frameworks.GameServices.DailyRewards.Presentation;
using Sources.Frameworks.GameServices.ServerTimes.Services;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;

namespace Sources.Frameworks.GameServices.DailyRewards.Controllers
{
    public class DailyRewardPresenter : PresenterBase
    {
        private readonly DailyRewardView _view;
        private readonly IServerTimeService _serverTimeService;
        private readonly DailyReward _dailyReward;
        
        private CancellationTokenSource _tokenSource;

        public DailyRewardPresenter(
            IEntityRepository entityRepository, 
            DailyRewardView view,
            IServerTimeService serverTimeService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _serverTimeService = serverTimeService ?? throw new ArgumentNullException(nameof(serverTimeService));
            _dailyReward = entityRepository.Get<DailyReward>(ModelId.DailyReward);
        }

        public override void Enable()
        {
            _view.Button.onClickEvent.AddListener(OnClick);
            StartTimer();
        }

        public override void Disable()
        {
            _tokenSource.Cancel();
            _view.Button.onClickEvent.RemoveListener(OnClick);
        }

        private async void StartTimer()
        {
            try
            {
                _tokenSource = new CancellationTokenSource();
                _dailyReward.ServerTime = _serverTimeService.GetNetworkTime();
                await UniTask.Delay(
                    _dailyReward.Delay, 
                    cancellationToken: _tokenSource.Token, 
                    ignoreTimeScale: true);
                
                while (_tokenSource.Token.IsCancellationRequested == false)
                {
                    _dailyReward.ServerTime += TimeSpan.FromSeconds(1);
                    _dailyReward.SetCurrentTime();
                    _view.TimerText.SetText(_dailyReward.TimerText);

                    await UniTask.Delay(
                        _dailyReward.Delay, 
                        cancellationToken: _tokenSource.Token, 
                        ignoreTimeScale: true);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (SocketException)
            {
                Debug.Log($"SocketException: {_dailyReward.ServerTime}");
                _tokenSource.Cancel();
                StartTimer();
            }
        }
        

        private void OnClick()
        {
            _dailyReward.SetTargetRewardTime();
        }
    }
}