using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions.Conditions
{
    [Category("Custom/Enemy")]
    public class CharacterHealthViewNotEqualsNull : ConditionTask
    {
        private EnemyViewBase _view;

        [Construct]
        private void Construct(EnemyViewBase view) =>
            _view = view;

        protected override bool OnCheck() =>
            _view.CharacterHealthView != null;
    }
}