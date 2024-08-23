using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    public class EnemyAttackTransition : ConditionTask
    {
        private EnemyViewBase _view;
        
        [Construct]
        private void Construct(EnemyViewBase view) =>
            _view = view;

        protected override bool OnCheck() =>
            _view.CharacterHealthView != null 
            && _view.CharacterHealthView.CurrentHealth > 0
            && Vector3.Distance(_view.Position, _view.CharacterHealthView.Position)
            <= _view.StoppingDistance + 0.15f;
    }
}