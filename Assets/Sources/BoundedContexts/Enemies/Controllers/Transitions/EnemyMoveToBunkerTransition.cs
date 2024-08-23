using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    public class EnemyMoveToBunkerTransition : ConditionTask
    {
        private IEnemyView _view;
        
        [Construct]
        private void Construct(EnemyView view) =>
            _view = view;
        
        protected override bool OnCheck() =>
            Vector3.Distance(_view.Position, _view.CharacterMeleePoint.Position)
            <= _view.StoppingDistance && _view.CharacterHealthView == null;
    }
}