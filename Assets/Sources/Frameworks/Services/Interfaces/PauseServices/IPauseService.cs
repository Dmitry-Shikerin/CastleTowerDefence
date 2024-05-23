using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sources.InfrastructureInterfaces.Services.PauseServices
{
    public interface IPauseService
    {
        event Action PauseActivated;
        event Action ContinueActivated;
        event Action PauseSoundActivated;
        event Action ContinueSoundActivated;
        
        bool IsPaused { get; }
        bool IsSoundPaused { get; }

        void ContinueSound();
        void Continue();
        void PauseSound();
        void Pause();
        UniTask PauseYield(CancellationToken cancellationToken);
        UniTask SoundPauseYield(CancellationToken cancellationToken);
    }
}