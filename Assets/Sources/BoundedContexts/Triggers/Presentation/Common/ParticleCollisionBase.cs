using System;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.PresentationsInterfaces.Triggers;
using UnityEngine;

namespace Sources.BoundedContexts.Triggers.Presentation.Common
{
    public class ParticleCollisionBase<T> : View, IEnteredTrigger<T>
    {
        public event Action<T> Entered;

        private void OnParticleCollision(GameObject other)
        {
            if (other.TryGetComponent(out T component))
            {
                Entered?.Invoke(component);
                
                return;
            }

            T nextComponent = other.GetComponentInChildren<T>();
            
            if (nextComponent != null)
                Entered?.Invoke(nextComponent);
        }
    }
}