using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using TeoGames.Mesh_Combiner.Scripts.Combine;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Bakers
{
    public class PoolBaker<T> : IPoolBaker<T> where T : IView
    {
        private MeshCombiner _meshCombiner;

        public PoolBaker()
        {
            _meshCombiner = new GameObject($"MeshCombiner of {typeof(T).Name}")
                .AddComponent<MeshCombiner>();
            _meshCombiner.bakeMaterials = true;
        }

        public void Bake()
        {
            
        }

        public void Add<T>(T gameObject) 
            where T : IView
        {
            gameObject.SetParent(_meshCombiner.transform);
        }
    }
}