using Doozy.Runtime.Reactor.Animators;
using HighlightPlus;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Bunkers.Controllers;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.Triggers.Presentation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.Bunkers.Presentation.Implementation
{
    public class BunkerView : PresentableView<BunkerPresenter>, IBunkerView
    {
        [Required] [SerializeField] private EnemyTrigger _enemyTrigger;
        [Required] [SerializeField] private UIAnimator _damageAnimator;
        [Required] [SerializeField] private HighlightEffect _highlightEffect;
        [Range(0.1f, 2)]
        [SerializeField] private float _highlightDelta = 0.13f;

        public float HighlightDelta => _highlightDelta;
        public Vector3 Position => transform.position;
        public UIAnimator DamageAnimator => _damageAnimator;
        public HighlightEffect HighlightEffect => _highlightEffect;

        protected override void OnAfterEnable() =>
            _enemyTrigger.Entered += OnEntered;

        protected override void OnAfterDisable() =>
            _enemyTrigger.Entered -= OnEntered;

        private void OnEntered(IEnemyViewBase enemyView) =>
            Presenter.TakeDamage(enemyView);
    }
}