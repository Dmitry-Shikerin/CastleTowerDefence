using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyMoveToCharacterState : FSMState
    {
        private IEnemyView _view;
        private IEnemyAnimation _animation;
        
        [Construct]
        private void Construct(EnemyView enemyView)
        {
            _view = enemyView;
            _animation = _view.Animation;
        }
        
        protected override void OnEnter() =>
            _animation.PlayWalk();

        protected override void OnUpdate()
        {
            if (_view.CharacterHealthView == null)
                return;
            
            if (_view.CharacterHealthView.CurrentHealth <= 0)
            {
                _view.SetCharacterHealth(null);
                
                return;
            }
            
            _view.Move(_view.CharacterHealthView.Position);
        }
    }
}