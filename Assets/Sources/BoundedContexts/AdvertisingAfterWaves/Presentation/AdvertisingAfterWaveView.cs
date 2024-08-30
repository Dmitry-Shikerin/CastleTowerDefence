using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Reactor.Animators;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.YandexSdcFramework.Advertisings.Domain.Constant;
using TMPro;
using UnityEngine;

namespace Sources.BoundedContexts.AdvertisingAfterWaves.Presentation
{
    public class AdvertisingAfterWaveView : View
    {
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private UIAnimator _animator;
        
        private TimeSpan _timerTimeSpan = TimeSpan.FromSeconds(AdvertisingConst.Delay);

        public void Show()
        {
            
        }
        
        
        private async UniTask ShowTimerAsync(CancellationToken cancellationToken)
        {
            for (int i = 3; i > 0 ; i--)
            {
                _timerText.SetText($"{i}");
                await UniTask.Delay(_timerTimeSpan, cancellationToken: cancellationToken);
            }
        }
    }
}