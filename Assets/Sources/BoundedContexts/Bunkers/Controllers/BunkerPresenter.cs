using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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
            
            try
            {
                _view.HighlightEffect.glow = 5f;
                _view.HighlightEffect.overlay = 1f;

                while (_view.HighlightEffect.glow > 0f &&
                       _view.HighlightEffect.overlay > 0f && 
                       _tokenSource.Token.IsCancellationRequested == false)
                {
                    _view.HighlightEffect.glow = Mathf.MoveTowards(
                        _view.HighlightEffect.glow, 
                        0, 
                        _view.HighlightDelta * 5 * Time.deltaTime);
                   _view.HighlightEffect.overlay = Mathf.MoveTowards(
                       _view.HighlightEffect.overlay, 
                       0, 
                       _view.HighlightDelta * Time.deltaTime);
                    
                    await UniTask.Yield(_tokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}