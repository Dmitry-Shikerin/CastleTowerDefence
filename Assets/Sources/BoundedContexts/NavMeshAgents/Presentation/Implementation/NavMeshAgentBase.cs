using Sirenix.OdinInspector;
using Sources.BoundedContexts.NavMeshAgents.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;
using UnityEngine.AI;

namespace Sources.BoundedContexts.NavMeshAgents.Presentation.Implementation
{
    public class NavMeshAgentBase : View, INavMeshAgent
    {
        [Required] [SerializeField] private NavMeshAgent _navMeshAgentBase;

        public Vector3 Position => transform.position;
        public float StoppingDistance => _navMeshAgentBase.stoppingDistance;
        protected NavMeshAgent NavMeshAgent => _navMeshAgentBase;
        
        public void Move(Vector3 position) =>
            _navMeshAgentBase.SetDestination(position);
        
        public void SetStoppingDistance(float stoppingDistance) =>
            _navMeshAgentBase.stoppingDistance = stoppingDistance;
    }
}