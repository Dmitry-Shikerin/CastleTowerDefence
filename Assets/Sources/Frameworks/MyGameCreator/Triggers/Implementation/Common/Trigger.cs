using System;
using UnityEngine;

namespace Sources.BoundedContexts.Triggers.Presentation.Common
{
    public class Trigger : TriggerBase
    {
        public event Action Entered;
        public event Action Exited;

        private void OnTriggerEnter(Collider other) =>
            GetComponent(other, Entered);

        private void OnTriggerExit(Collider other) =>
            GetComponent(other, Exited);
    }
}