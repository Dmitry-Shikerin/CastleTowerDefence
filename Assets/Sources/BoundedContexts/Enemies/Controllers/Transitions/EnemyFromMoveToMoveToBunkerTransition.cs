using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    public class EnemyFromMoveToMoveToBunkerTransition : ConditionTask
    {
        private IEnemyView _view;

        [Construct]
        private void Construct(EnemyView view) =>
            _view = view;

        protected override bool OnCheck() =>
            _view.CharacterHealthView == null;
    }
}