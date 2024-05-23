using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sources.Domain.Models.Constants;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.Presentations.UI.Curtains
{
    public class CurtainView : View
    {
        [Required] [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _duration = 1f;
        
        private CancellationTokenSource _cancellationTokenSource;
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

        public async UniTask ShowCurtain()
        {
            IsInProgress = true;
            Show();
            await Fade(0, CurtainConst.Max, _cancellationTokenSource.Token);
        }

        public async UniTask HideCurtain()
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

                    //await UniTask.Yield(cancellationToken);
                    await UniTask.Delay(TimeSpan.FromMilliseconds(1), ignoreTimeScale:true, cancellationToken: cancellationToken);
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