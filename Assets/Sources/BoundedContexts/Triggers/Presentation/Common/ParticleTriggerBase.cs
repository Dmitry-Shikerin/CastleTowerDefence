using System;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Triggers;
using UnityEngine;

namespace Sources.BoundedContexts.Triggers.Presentation.Common
{
    public abstract class ParticleTriggerBase<T> : View, ITrigger<T>
    {
        public event Action<T> Entered;
        public event Action<T> Exited;

        
    }
}