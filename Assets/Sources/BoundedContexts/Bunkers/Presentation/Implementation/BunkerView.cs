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
        
        public Vector3 Position => transform.position;

        protected override void OnAfterEnable() =>
            _enemyTrigger.Entered += OnEntered;

        protected override void OnAfterDisable() =>
            _enemyTrigger.Entered -= OnEntered;

        private void OnEntered(IEnemyView enemyView)
        {
            Presenter.TakeDamage(enemyView);
            Debug.Log($"EenmyEntered: {enemyView}");
        }
    }
}