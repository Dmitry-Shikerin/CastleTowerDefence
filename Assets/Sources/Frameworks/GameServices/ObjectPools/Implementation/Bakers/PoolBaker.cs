using DevDunk.AutoLOD;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using TeoGames.Mesh_Combiner.Scripts.Combine;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Bakers
{
    public class PoolBaker<T> : IPoolBaker<T> where T : IView
    {
        private MeshCombiner _meshCombiner;
        private MeshRenderer _meshRenderer;
        private SkinnedMeshRenderer _skinnedMeshRenderer;
        private bool _isInitialize;

        public PoolBaker()
        {
            _meshCombiner = new GameObject($"MeshCombiner of {typeof(T).Name}")
                .AddComponent<MeshCombiner>();
        }

        public IPoolBaker<T> SetRootParent(Transform parent)
        {
            if (_isInitialize)
                return this;
            
            _meshCombiner.transform.SetParent(parent.transform);
            _meshCombiner.bakeMaterials = true;
            _isInitialize = true;
            
            return this;
        }

        public void Bake()
        {
            
        }

        public void Add<T>(T gameObject)
            where T : IView
        {
            gameObject.SetParent(_meshCombiner.transform);
            InitializeMeshRenderer();
            SetSkinnedMesh(gameObject);
        }

        private void InitializeMeshRenderer()
        {
            if (_meshRenderer != null)
                return;
            
            _meshRenderer = _meshCombiner.GetComponentInChildren<MeshRenderer>();
            _skinnedMeshRenderer = _meshCombiner.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        private void SetSkinnedMesh<T>(T gameObject)
        {
            if((gameObject as View).TryGetComponent(out AnimatorLODObject animatorLOD) == false)
                return;

            if (_skinnedMeshRenderer == null)
                return;

            animatorLOD.SkinnedMeshRenderers.Clear();
            animatorLOD.SkinnedMeshRenderers.Add(_skinnedMeshRenderer);
        }
    }
}