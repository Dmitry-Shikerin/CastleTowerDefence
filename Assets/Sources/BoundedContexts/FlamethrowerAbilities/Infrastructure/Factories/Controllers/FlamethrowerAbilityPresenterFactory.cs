using System;
using JetBrains.Annotations;
using MyAudios.MyUiFramework.Utils.Soundies.Infrastructure;
using Sources.BoundedContexts.FlamethrowerAbilities.Controllers;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Infrastructure.Factories.Controllers
{
    public class FlamethrowerAbilityPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ISoundyService _soundyService;

        public FlamethrowerAbilityPresenterFactory(IEntityRepository entityRepository, ISoundyService soundyService)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
        }

        public FlamethrowerAbilityPresenter Create(IFlamethrowerAbilityView view)
        {
            return new FlamethrowerAbilityPresenter(_entityRepository, _soundyService, view);
        }
    }
}