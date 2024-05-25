using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Presentation
{
    public class EnemyView : EnemyViewBase, IEnemyView
    {
        [Required] [SerializeField] private EnemyDependencyProvider _provider;
        [Required] [SerializeField] private FSMOwner _fsmOwner;
        
        public FSMOwner FsmOwner => _fsmOwner;
        public EnemyDependencyProvider Provider => _provider;
    }
}