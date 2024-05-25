using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.Presentation;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyInitializeState : FSMState
    {
        private Enemy _enemy;
        private IEnemyView _view;

        protected override void OnInit()
        {
            //TODO как получить сюда зависимость
            EnemyDependencyProvider provider = 
                (graphBlackboard.parent as Blackboard).GetComponent<EnemyDependencyProvider>();

            _enemy = provider.Enemy;
            _view = provider.View;
        }

        protected override void OnEnter()
        {
            Debug.Log($"Enemy in InitializeState");
            _enemy.IsInitialized = true;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }
}