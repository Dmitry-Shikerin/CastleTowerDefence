using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Images;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.LoadingImages.Presentation
{
    public class RotationImageView : View, ISelfValidator
    {
        [Range(0, 2)]
        [SerializeField] private float _delay = 0.5f;
        [Required] [SerializeField] private ImageView _imageView;
        [SerializeField] private List<Sprite> _sprites;
        
        private CancellationTokenSource _tokenSource;
        private TimeSpan _timeSpanDelay;
        
        public void Validate(SelfValidationResult result)
        {
            if (_sprites.Count != 8)
                result.AddError("Sprites count should be 8");
        }

        private void OnEnable()
        {
            _tokenSource = new CancellationTokenSource();
            _timeSpanDelay = TimeSpan.FromSeconds(_delay);
            Rotate();
        }

        private void OnDisable()
        {
            _tokenSource.Cancel();
        }

        private async void Rotate()
        {
            try
            {
                while (_tokenSource.IsCancellationRequested == false)
                {
                    foreach (Sprite sprite in _sprites)
                    {
                        _imageView.SetSprite(sprite);
                        await UniTask.Delay(
                            _timeSpanDelay, 
                            cancellationToken: _tokenSource.Token);
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}