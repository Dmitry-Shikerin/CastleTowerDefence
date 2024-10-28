using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using HighlightPlus;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.Soundies.Infrastructure.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Bunkers.Controllers
{
    public class BunkerPresenter : PresenterBase
    {
        private readonly Bunker _bunker;
        private readonly IBunkerView _view;
        private readonly ISoundyService _soundyService;
        
        private CancellationTokenSource _tokenSource;

        public BunkerPresenter(IEntityRepository entityRepository, IBunkerView view, ISoundyService soundyService)
        {
            if (entityRepository == null)
                throw new ArgumentNullException(nameof(entityRepository));
            
            _bunker = entityRepository.Get<Bunker>(ModelId.Bunker);
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
        }

        public override void Enable()
        {
            _tokenSource = new CancellationTokenSource();
            _view.HighlightEffect.highlighted = true;
        }

        public override void Disable()
        {
            _tokenSource.Cancel();   
        }

        public void TakeDamage(IEnemyViewBase enemyView)
        {
            _bunker.TakeDamage();
            _view.DamageAnimator.Play();
            enemyView.Destroy();
            ShowHighlight();
            _soundyService.Play("Sounds", "Bunker");
        }

        private async void ShowHighlight()
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
            HighlightEffect highlight = _view.HighlightEffect;
            float highlightDelta = _view.HighlightDelta;
            
            try
            {
                highlight.glow = 5f;
                highlight.overlay = 1f;

                while (highlight.glow > 0f 
                       && highlight.overlay > 0f 
                       && _tokenSource.Token.IsCancellationRequested == false)
                {
                    highlight.glow = Mathf.MoveTowards(
                        highlight.glow, 0, highlightDelta * 5 * Time.deltaTime);
                    highlight.overlay = Mathf.MoveTowards(
                        highlight.overlay, 0, highlightDelta * Time.deltaTime);
                    
                    await UniTask.Yield(_tokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}