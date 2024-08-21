using System;
using System.Net.Sockets;
using System.Threading;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager;
using JetBrains.Annotations;
using MyAudios.MyUiFramework.Utils.Soundies.Infrastructure;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.GameServices.DailyRewards.Domain;
using Sources.Frameworks.GameServices.DailyRewards.Presentation;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
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
        private readonly ISoundyService _soundyService;
        private readonly ILoadService _loadService;
        private readonly DailyReward _dailyReward;
        
        private CancellationTokenSource _tokenSource;

        public DailyRewardPresenter(
            IEntityRepository entityRepository, 
            DailyRewardView view,
            IServerTimeService serverTimeService,
            ISoundyService soundyService,
            ILoadService loadService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _serverTimeService = serverTimeService ?? throw new ArgumentNullException(nameof(serverTimeService));
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _dailyReward = entityRepository.Get<DailyReward>(ModelId.DailyReward);
        }

        public override void Enable()
        {
            _view.Button.onClickEvent.AddListener(OnClick);
            StartTimer();
            ActivateButton();
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
                _tokenSource.Cancel();
                StartTimer();
            }
        }

        private void OnClick()
        {
            Debug.Log($"3daily reward {_dailyReward.IsAvailable}");
            ActivateButton();
            
            if (_dailyReward.TrySetTargetRewardTime() == false)
                return;
            
            Debug.Log($"4daily reward {_dailyReward.IsAvailable}");
            _view.Animator.Play();
            _soundyService.Play("Sounds","DailyReward");
            _loadService.Save(ModelId.DailyReward);
        }

        private void ActivateButton()
        {
            Debug.Log($"1daily reward {_dailyReward.IsAvailable}");
            if (_dailyReward.IsAvailable == false)
            {
                _view.LockImage.ShowImage();
                _view.Button.interactable = false;
                _view.Button.SetState(UISelectionState.Disabled);
                _view.TimerView.alpha = 1;
                
                return;
            }
            
            _view.LockImage.HideImage();
            _view.Button.interactable = true;
            _view.TimerView.alpha = 0;
            _view.Button.SetState(UISelectionState.Normal);
            
            Debug.Log($"2daily reward {_dailyReward.IsAvailable}");
        }
    }
}