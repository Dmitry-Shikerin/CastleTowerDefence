using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Bakers;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using TeoGames.Mesh_Combiner.Scripts.Combine;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Bakers
{
    public interface IPoolBaker<T> : IPoolBaker
        where T : IView
    {
        IPoolBaker<T> SetRootParent(Transform parent);
    }
}