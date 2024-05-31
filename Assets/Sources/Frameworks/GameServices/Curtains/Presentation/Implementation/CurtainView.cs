using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sources.Frameworks.Domain.Implementation.Constants;
using Sources.Frameworks.GameServices.Curtains.Domain;
using Sources.Frameworks.GameServices.Curtains.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Curtains.Presentation.Implementation
{
    public class CurtainView : View, ICurtainView
    {
        [Required] [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _duration = 1f;
        
        private CancellationTokenSource _cancellationTokenSource;
        private TimeSpan _timeSpan => TimeSpan.FromMilliseconds(1);
        
        public bool IsInProgress { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _canvasGroup.alpha = 0;
        }

        private void OnEnable() =>
            _cancellationTokenSource = new CancellationTokenSource();
        
        private void OnDisable() =>
            _cancellationTokenSource.Cancel();

        public async UniTask ShowAsync()
        {
            IsInProgress = true;
            Show();
            await Fade(0, CurtainConst.Max, _cancellationTokenSource.Token);
        }

        public async UniTask HideAsync()
        {
            await Fade(CurtainConst.Max, 0, _cancellationTokenSource.Token);
            Hide();
            IsInProgress = false;
        }

        private async UniTask Fade(float start, float end, CancellationToken cancellationToken)
        {
            try
            {
                _canvasGroup.alpha = start;

                while (Mathf.Abs(_canvasGroup.alpha - end) > MathConst.Epsilon)
                {
                    _canvasGroup.alpha = Mathf.MoveTowards(
                        _canvasGroup.alpha, end, Time.deltaTime / _duration);

                    await UniTask.Delay(_timeSpan, ignoreTimeScale:true, cancellationToken: cancellationToken);
                }

                _canvasGroup.alpha = end;
            }
            catch (OperationCanceledException)
            {
                if (_canvasGroup == null)
                    return;

                Hide();
            }
            catch (MissingReferenceException)
            {
            }
        }
    }
}