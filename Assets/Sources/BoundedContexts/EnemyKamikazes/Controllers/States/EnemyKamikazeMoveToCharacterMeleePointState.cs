using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyKamikazeMoveToCharacterMeleePointState : FSMState
    {
        private IEnemyKamikazeView _view;
        private IEnemyAnimation _animation;

        [Construct]
        private void Construct(IEnemyKamikazeView view)
        {
            _view = view;
            _animation = _view.Animation;
        }
        
        protected override void OnEnter() =>
            _animation.PlayWalk();

        protected override void OnUpdate() =>
            _view.Move(_view.CharacterMeleePoint.Position);
    }
}