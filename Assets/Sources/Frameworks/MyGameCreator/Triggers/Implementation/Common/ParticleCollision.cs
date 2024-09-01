using System;
using UnityEngine;

namespace Sources.BoundedContexts.Triggers.Presentation.Common
{
    public class ParticleCollision : TriggerBase
    {
        public event Action Entered;

        private void OnParticleCollision(GameObject other) =>
            GetComponent(other, Entered);
    }
}