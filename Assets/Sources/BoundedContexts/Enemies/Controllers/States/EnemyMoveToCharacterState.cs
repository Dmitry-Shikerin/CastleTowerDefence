using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyMoveToCharacterState : FSMState
    {
        private Enemy _enemy;
        private IEnemyView _view;
        private IEnemyAnimation _animation;

        protected override void OnInit()
        {
            EnemyDependencyProvider provider =
                graphBlackboard.parent.GetVariable<EnemyDependencyProvider>("_provider").value;

            _enemy = provider.Enemy;
            _view = provider.View;
            _animation = provider.Animation;
        }

        protected override void OnEnter() =>
            _animation.PlayWalk();

        protected override void OnUpdate()
        {
            if(_view.CharacterHealthView.CurrentHealth <= 0)
                _view.SetCharacterHealth(null);
            
            _view.Move(_view.CharacterHealthView.Position);
        }

        protected override void OnExit()
        {
        }
    }
}