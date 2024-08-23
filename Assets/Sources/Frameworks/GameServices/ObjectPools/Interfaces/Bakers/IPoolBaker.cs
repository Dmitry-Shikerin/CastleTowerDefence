using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ObjectPools.Interfaces.Bakers
{
    public interface IPoolBaker
    {
        void Bake();
        void Add(IView view);
    }
}