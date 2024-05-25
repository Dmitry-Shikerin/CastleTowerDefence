﻿using JetBrains.Annotations;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Characters.PresentationInterfaces;
using Sources.BoundedContexts.Enemies.Extensions;

namespace Sources.BoundedContexts.Characters.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterIdleState : FSMState
    {
        private ICharacterView _view;
        private ICharacterAnimation _animation;

        protected override void OnInit()
        {
            CharacterDependencyProvider provider = 
                graphBlackboard.GetDependencyProvider<CharacterDependencyProvider>();
            _view = provider.View;
            _animation = _view.Animation;
        }

        protected override void OnEnter()
        {
            _animation.PlayIdle();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }
}