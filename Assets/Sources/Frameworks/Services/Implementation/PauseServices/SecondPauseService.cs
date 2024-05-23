using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.InfrastructureInterfaces.Services.PauseServices;

namespace Sources.Infrastructure.Services.PauseServices
{
    public class SecondPauseService : IPauseService
    {
        public event Action PauseActivated;
        public event Action ContinueActivated;
        public event Action PauseSoundActivated;
        public event Action ContinueSoundActivated;
        public bool IsPaused { get; private set; }
        public bool IsSoundPaused { get; private set; }
        
        public void ContinueSound()
        {
            throw new NotImplementedException();
        }

        public void Continue()
        {
            throw new NotImplementedException();
        }

        public void PauseSound()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public UniTask PauseYield(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask SoundPauseYield(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}