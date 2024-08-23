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
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.GameServices.ServerTimes.Services;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using UnityEngine;

namespace Sources.Frameworks.GameServices.DailyRewards.Controllers
{
    public class DailyRewardPresenter : PresenterBase
    {
        private readonly DailyRewardView _view;
        private readonly ITimeService _timeService;
        private readonly ILoadService _loadService;
        private readonly DailyReward _dailyReward;
        
        private CancellationTokenSource _tokenSource;

        public DailyRewardPresenter(
            IEntityRepository entityRepository, 
            DailyRewardView view,
            ITimeService timeService,
            ILoadService loadService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
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
                _dailyReward.ServerTime = _timeService.GetTime();
                
                await UniTask.Delay(
                    _dailyReward.Delay, 
                    cancellationToken: _tokenSource.Token, 
                    ignoreTimeScale: true);
                
                while (_tokenSource.Token.IsCancellationRequested == false)
                {
                    _dailyReward.ServerTime += TimeSpan.FromSeconds(1);
                    _dailyReward.SetCurrentTime();
                    _view.TimerText.SetText(_dailyReward.TimerText);
                    ActivateButton();

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
            ActivateButton();
            
            if (_dailyReward.TrySetTargetRewardTime() == false)
                return;
            
            _view.Animator.Play();
            _loadService.Save(ModelId.DailyReward);
        }

        private void ActivateButton()
        {
            Debug.Log($"daily reward {_dailyReward.IsAvailable}");
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
        }
    }
}