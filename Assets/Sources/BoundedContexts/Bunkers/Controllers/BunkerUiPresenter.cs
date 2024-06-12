﻿using System;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.Bunkers.Controllers
{
    public class BunkerUiPresenter : PresenterBase
    {
        private readonly Bunker _bunker;
        private readonly IBunkerUi _view;

        public BunkerUiPresenter(IEntityRepository entityRepository, IBunkerUi view)
        {
            if (entityRepository == null)
                throw new ArgumentNullException(nameof(entityRepository));
            
            _bunker = entityRepository.Get<Bunker>(ModelId.Bunker);
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            OnHealthChanged();
            _bunker.OnHealthChanged += OnHealthChanged;
        }

        public override void Disable() =>
            _bunker.OnHealthChanged += OnHealthChanged;

        private void OnHealthChanged() =>
            _view.HealthText.SetText(_bunker.Health.ToString());
    }
}