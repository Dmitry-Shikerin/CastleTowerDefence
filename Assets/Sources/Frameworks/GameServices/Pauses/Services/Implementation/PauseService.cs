using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Domain.Models.Constants;
using Sources.InfrastructureInterfaces.Services.PauseServices;
using UnityEngine;

namespace Sources.Infrastructure.Services.PauseServices
{
    public class PauseService : IPauseService
    {
        public event Action PauseActivated;
        public event Action ContinueActivated;
        public event Action PauseSoundActivated;
        public event Action ContinueSoundActivated;
        
        public int PauseListenersCount { get; private set; }
        public int SoundPauseListenersCount { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsSoundPaused { get; private set; }
        
        public void ContinueSound()
        {
            SoundPauseListenersCount--;
            // Debug.Log($"PauseService continue sound: {SoundPauseListenersCount}");

            if (SoundPauseListenersCount > 0)
                return;

            if (SoundPauseListenersCount < 0)
                throw new IndexOutOfRangeException(nameof(SoundPauseListenersCount));

            IsSoundPaused = false;
            ContinueSoundActivated?.Invoke();
        }

        public void Continue()
        {
            PauseListenersCount--;
            // Debug.Log($"PauseService continue: {PauseListenersCount}");

            if (PauseListenersCount > 0)
                return;

            if (PauseListenersCount < 0)
                throw new IndexOutOfRangeException(nameof(PauseListenersCount));

            IsPaused = false;
            ContinueActivated?.Invoke();
            Time.timeScale = TimeScaleConst.Max;
        }

        public void PauseSound()
        {
            SoundPauseListenersCount++;
            // Debug.Log($"PauseService pause sound: {SoundPauseListenersCount}");
            
            if (SoundPauseListenersCount < 0)
                throw new IndexOutOfRangeException(nameof(SoundPauseListenersCount));

            IsSoundPaused = true;
            PauseSoundActivated?.Invoke();
        }

        public void Pause()
        {
            PauseListenersCount++;
            // Debug.Log($"PauseService pause: {PauseListenersCount}");

            if (PauseListenersCount < 0)
                throw new IndexOutOfRangeException(nameof(PauseListenersCount));

            IsPaused = true;
            PauseActivated?.Invoke();
            Time.timeScale = TimeScaleConst.Min;
        }

        public async UniTask PauseYield(CancellationToken cancellationToken)
        {
            do
            {
                await UniTask.Yield(cancellationToken);
            }
            while (IsPaused);
        }
        
        public async UniTask SoundPauseYield(CancellationToken cancellationToken)
        {
            do
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(0.05f), ignoreTimeScale: true, cancellationToken: cancellationToken);
                // await UniTask.Yield(cancellationToken);
            }
            while (IsSoundPaused);
        }
    }
}