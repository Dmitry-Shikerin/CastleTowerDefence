using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using Sources.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.BoundedContexts.NavMeshAgents.PresentationInterfaces
{
    public interface INavMeshAgent : IView
    {
        public Vector3 Position { get; }
        public float StoppingDistance { get; }
    
        void Move(Vector3 position);
        void SetStoppingDistance(float stoppingDistance);
    }
}