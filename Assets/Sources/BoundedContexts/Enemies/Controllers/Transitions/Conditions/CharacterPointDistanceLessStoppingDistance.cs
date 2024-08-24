using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions.Conditions
{
    [Category("Custom/Enemy")]
    public class CharacterPointDistanceLessStoppingDistance : ConditionTask
    {
        private EnemyViewBase _view;

        [Construct]
        private void Construct(EnemyViewBase view) =>
            _view = view;

        protected override bool OnCheck() =>
            Vector3.Distance(_view.Position, _view.CharacterMeleePoint.Position)
            <= _view.StoppingDistance + 0.15f;
    }
}