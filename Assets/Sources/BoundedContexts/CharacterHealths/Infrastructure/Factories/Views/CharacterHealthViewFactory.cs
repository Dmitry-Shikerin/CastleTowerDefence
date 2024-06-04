﻿using System;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.CharacterHealths.Controllers;
using Sources.BoundedContexts.CharacterHealths.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterHealths.Presentation;

namespace Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Views
{
    public class CharacterHealthViewFactory
    {
        private readonly CharacterHealthPresenterFactory _presenterFactory;

        public CharacterHealthViewFactory(CharacterHealthPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }

        public ICharacterHealthView Create(CharacterHealths.Domain.CharacterHealth characterHealth, CharacterHealthView view)
        {
            CharacterHealthPresenter presenter = _presenterFactory.Create(characterHealth, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}