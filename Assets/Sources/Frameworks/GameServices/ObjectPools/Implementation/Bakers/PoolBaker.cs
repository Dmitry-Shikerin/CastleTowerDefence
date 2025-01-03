﻿using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Bakers.Generic;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using TeoGames.Mesh_Combiner.Scripts.Combine;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Bakers
{
    public class PoolBaker<T> : IPoolBaker<T>
        where T : IView
    {
        private MeshCombiner _meshCombiner;
        private MeshRenderer _meshRenderer;
        private SkinnedMeshRenderer _skinnedMeshRenderer;
        private bool _isInitialize;

        public PoolBaker(Transform parent)
        {
            _meshCombiner = new GameObject($"MeshCombiner of {typeof(T).Name}")
                .AddComponent<MeshCombiner>();
            _meshCombiner.bakeMaterials = true;
            _meshCombiner.transform.SetParent(parent);
        }

        public void Add(IView view)
        {
            view.SetParent(_meshCombiner.transform);
            InitializeMeshRenderer();
        }

        private void InitializeMeshRenderer()
        {
            if (_meshRenderer != null)
                return;
            
            _meshRenderer = _meshCombiner.GetComponentInChildren<MeshRenderer>();
            _skinnedMeshRenderer = _meshCombiner.GetComponentInChildren<SkinnedMeshRenderer>();
        }
    }
}