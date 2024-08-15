using System;
using MyAudios.MyUiFramework.Utils.Soundies.Infrastructure;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Presentation.Implementation;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.Bunkers.Controllers
{
    public class BunkerPresenter : PresenterBase
    {
        private readonly Bunker _bunker;
        private readonly IBunkerView _view;
        private readonly ISoundyService _soundyService;

        public BunkerPresenter(IEntityRepository entityRepository, IBunkerView view, ISoundyService soundyService)
        {
            if (entityRepository == null)
                throw new ArgumentNullException(nameof(entityRepository));
            
            _bunker = entityRepository.Get<Bunker>(ModelId.Bunker);
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
        }

        public void TakeDamage(IEnemyViewBase enemyView)
        {
            _bunker.TakeDamage();
            enemyView.Destroy();
            _soundyService.Play("Sounds", "Bunker");
        }
    }
}