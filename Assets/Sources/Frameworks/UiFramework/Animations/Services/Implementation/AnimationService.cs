using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.UiFramework.Presentation.Animations;
using Sources.Frameworks.UiFramework.Presentation.Animations.Types;
using Sources.Frameworks.UiFramework.Presentation.Buttons;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Animations;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Services.Animations
{
    public class AnimationService : IAnimationService
    {
        private UiAnimator _uiAnimator;
        private UiButton _uiUiUiButton;
        private CancellationTokenSource _cancellationTokenSource;

        public void Awake()
        {
            if (_uiAnimator.ReactionAnimationType == ReactionAnimationType.ButtonClick)
            {
                _uiUiUiButton = _uiAnimator.GetComponent<UiButton>();

                if (_uiUiUiButton == null)
                    throw new NullReferenceException(nameof(_uiUiUiButton));
            }
        }

        public void Enable()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            if (_uiAnimator.ReactionAnimationType == ReactionAnimationType.ButtonClick)
                _uiUiUiButton.AddClickListener(PlayAnimation);

            if (_uiAnimator.ReactionAnimationType == ReactionAnimationType.ShowView)
                PlayAnimation();
        }

        public void Disable()
        {
            if (_uiAnimator.ReactionAnimationType == ReactionAnimationType.ButtonClick)
                _uiUiUiButton.RemoveClickListener(PlayAnimation);

            _cancellationTokenSource.Cancel();
        }

        public void Construct(UiAnimator playerWallet)
        {
            _uiAnimator = playerWallet;
        }

        private async void PlayAnimation()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            if (_uiAnimator.AnimationType == AnimationType.Scale)
            {
                if (_uiAnimator.ScaleAnimationType == ScaleAnimationType.PopUp)
                    await PlayScaleAnimationAsync(_uiAnimator.transform, _cancellationTokenSource.Token);
                if (_uiAnimator.ScaleAnimationType == ScaleAnimationType.Infinitely)
                {
                    while (_cancellationTokenSource.Token.IsCancellationRequested == false)
                    {
                        await PlayScaleAnimationAsync(_uiAnimator.transform, _cancellationTokenSource.Token);
                    }
                }
                
                return;
            }

            if (_uiAnimator.AnimationType == AnimationType.Rotate)
                await PlayRotateAnimationAsync(_cancellationTokenSource.Token);
        }

        private async UniTask PlayScaleAnimationAsync(Transform transform, CancellationToken token)
        {
            try
            {
                while (transform != null && Vector3.Distance(transform.localScale, _uiAnimator.TargetScale) > 0.01f)
                {
                    transform.localScale = Vector3.MoveTowards(
                        transform.localScale,
                        _uiAnimator.TargetScale,
                        _uiAnimator.AnimationDuration);

                    await UniTask.Yield(token);
                }

                while ( transform != null && Vector3.Distance(transform.localScale, _uiAnimator.FromScale) > 0.01f)
                {
                    transform.localScale = Vector3.MoveTowards(
                        transform.localScale,
                        _uiAnimator.FromScale,
                        _uiAnimator.AnimationDuration);

                    await UniTask.Yield(token);
                }
            }
            catch (OperationCanceledException)
            {
                if(transform == null)
                    return;
                
                transform.localScale = _uiAnimator.FromScale;
            }
        }

        private async UniTask PlayRotateAnimationAsync(CancellationToken token)
        {
            try
            {
                while (_uiAnimator != null && token.IsCancellationRequested == false && _uiAnimator.enabled)
                {
                    if (_uiAnimator == null)
                        return;

                    _uiAnimator?.transform.Rotate(_uiAnimator.RotationVector);

                    await UniTask.Yield(token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        public void Destroy()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}