using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyKamikazeInitializeState : FSMState
    {
        private Enemy _enemy;
        private IEnemyAnimation _animation;

        [Construct]
        private void Construct(Enemy enemy, IEnemyKamikazeView view)
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