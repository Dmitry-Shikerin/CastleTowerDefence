using Sirenix.OdinInspector;
using Sources.BoundedContexts.NavMeshAgents.PresentationInterfaces;
using Sources.ControllersInterfaces;
using Sources.Presentations.Views;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Sources.BoundedContexts.NavMeshAgents.Presentation
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