using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.GameServices.ServerTimes.Services;
using Sources.Frameworks.MyGameCreator.DailyRewards.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Sources.Frameworks.GameServices.DailyRewards.Infrastructure.Services.Implementation
{
    public class DailyRewardService : IDailyRewardService
    {
        private readonly IServerTimeService _serverTimeService;
        private CancellationTokenSource _tokenSource;
        private DateTime _lastRewardTime;
        private TimeSpan _currentTime;
        private DateTime _targetRewardTime;
        private TimeSpan _delay;

        public DailyRewardService(IServerTimeService serverTimeService)
        {
            _serverTimeService = serverTimeService 
                                 ?? throw new ArgumentNullException(nameof(serverTimeService));
        }

        public void Initialize()
        {
            _delay = TimeSpan.FromSeconds(1);
            _tokenSource = new CancellationTokenSource();
            _lastRewardTime = DateTime.Now;
            _targetRewardTime = _lastRewardTime.AddDays(1);
            StartTimer();
        }

        public void Destroy()
        {
            _tokenSource.Cancel();
        }

        private async void StartTimer()
        {
            try
            {
                while (_tokenSource.Token.IsCancellationRequested == false)
                {
                    DateTime serverTime = _serverTimeService.GetNetworkTime();
                    _currentTime = _targetRewardTime - serverTime;
                    Debug.Log($"{serverTime}");

                    await UniTask.Delay(_delay, cancellationToken: _tokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}