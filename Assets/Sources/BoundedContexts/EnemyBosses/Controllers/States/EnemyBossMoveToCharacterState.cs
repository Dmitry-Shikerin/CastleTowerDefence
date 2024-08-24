using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyBossMoveToCharacterState : FSMState
    {
        private IEnemyBossView _view;
        private IEnemyBossAnimation _animation;

        [Construct]
        private void Construct(IEnemyBossView view)
        {
            _view = view;
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