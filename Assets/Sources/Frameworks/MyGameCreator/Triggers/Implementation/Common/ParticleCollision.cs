using System;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Triggers.Implementation.Common
{
    public class ParticleCollision : TriggerBase
    {
        public event Action Entered;

        private void OnParticleCollision(GameObject other) =>
            GetComponent(other, Entered);
    }
}