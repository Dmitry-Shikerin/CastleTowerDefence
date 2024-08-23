﻿using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    public class EnemyFromMoveToMoveToBunkerTransition : ConditionTask
    {
        private EnemyViewBase _view;

        [Construct]
        private void Construct(EnemyViewBase view) =>
            _view = view;

        protected override bool OnCheck() =>
            _view.CharacterHealthView == null;
    }
}