using System;
using Sources.BoundedContexts.FlamethrowerAbilities.Controllers;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Implementation;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.Soundies.Infrastructure.Interfaces;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Infrastructure.Factories.Views
{
    public class FlamethrowerAbilityViewFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ISoundyService _soundyService;

        public FlamethrowerAbilityViewFactory(
            IEntityRepository entityRepository,
            ISoundyService soundyService)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _soundyService = soundyService ?? 
                             throw new ArgumentNullException(nameof(soundyService));
        }

        public IFlamethrowerAbilityView Create(FlamethrowerAbilityView view)
        {
            FlamethrowerAbilityPresenter presenter = new FlamethrowerAbilityPresenter(
                _entityRepository, _soundyService, view);
            view.Construct(presenter);

            return view;
        }
    }
}