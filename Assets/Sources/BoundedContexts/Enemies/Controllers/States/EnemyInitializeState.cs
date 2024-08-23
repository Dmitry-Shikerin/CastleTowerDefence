using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyInitializeState : FSMState
    {
        private Enemy _enemy;
        private IEnemyAnimation _animation;

        [Construct]
        private void Construct(Enemy enemy, EnemyView view)
        {
            _enemy = enemy;
            _animation = view.Animation;
        }
        
        protected override void OnEnter()
        {
            _enemy.IsInitialized = true;
            _animation.PlayIdle();
        }
    }
}