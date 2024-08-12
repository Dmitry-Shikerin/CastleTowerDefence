using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.MyGameCreator.DailyRewards.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.DailyRewards.Infrastructure.Services.Implementation
{
    public class DailyRewardService : IDailyRewardService
    {
        private CancellationTokenSource _tokenSource;
        private DateTime _lastRewardTime;
        private TimeSpan _currentTime;
        private DateTime _targetRewardTime;
        private TimeSpan _delay;
        
        public void Initialize()
        {
            _delay = TimeSpan.FromDays(1);
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
                    _currentTime = _targetRewardTime - DateTime.Now;
                    Debug.Log($"{_currentTime}");

                    await UniTask.Delay(_delay, cancellationToken: _tokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}