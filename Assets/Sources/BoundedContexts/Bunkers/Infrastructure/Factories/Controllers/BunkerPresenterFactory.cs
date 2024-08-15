﻿using System;
using MyAudios.MyUiFramework.Utils.Soundies.Infrastructure;
using Sources.BoundedContexts.Bunkers.Controllers;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Controllers
{
    public class BunkerPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ISoundyService _soundyService;

        public BunkerPresenterFactory(IEntityRepository entityRepository, ISoundyService soundyService)
        {
            _entityRepository = entityRepository ??
                                throw new ArgumentNullException(nameof(entityRepository));
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
        }

        public BunkerPresenter Create(IBunkerView bunkerView)
        {
            return new BunkerPresenter(_entityRepository, bunkerView, _soundyService);
        }
    }
}