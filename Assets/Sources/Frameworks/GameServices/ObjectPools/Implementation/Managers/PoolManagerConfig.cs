using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.ConfigCollectors.Domain.ScriptableObjects;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.UiFramework.Texts.Extensions;
using Sources.Frameworks.Utils.Reflections;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers
{
    public class PoolManagerConfig : Config
    {
        [OnValueChanged(nameof(GetPrefab))]
        [ValueDropdown(nameof(GetFilteredNames))] 
        [SerializeReference] private string _type;
        [SerializeField] private bool _isWarmUp;
        [EnableIf(nameof(_isWarmUp))] 
        [Range(0, 5)] 
        [SerializeField] private int _warmUpTime;
        [EnableIf(nameof(_isWarmUp))] 
        [Range(0, 100)] 
        [SerializeField] private int _warmUpCount;
        [SerializeField] private bool _isCountLimit;
        [EnableIf(nameof(_isCountLimit))] 
        [MinMaxSlider(-1, 100, true)] 
        [SerializeField] private Vector2Int _minMaxPoolCount = new Vector2Int(-1, 0);
        [EnableIf(nameof(_isCountLimit))] 
        [Range(0, 20)] 
        [SerializeField] private float _deleteAfterTime;
        [HideLabel] 
        [PreviewField(300, ObjectFieldAlignment.Center)] 
        [SerializeField] private GameObject _prefab;

        public bool IsWarmUp => _isWarmUp;
        public int WarmUpCount => _warmUpCount;
        public bool IsCountLimit => _isCountLimit;
        public int MinPoolCount => _minMaxPoolCount.x;
        public int MaxPoolCount => _minMaxPoolCount.y;
        public float WarmUpTime => _warmUpTime;
        public float DeleteAfterTime => _deleteAfterTime;
        public Type Type => GetTypeByName();
        public GameObject Prefab => _prefab;

        private Type GetTypeByName() =>
            ReflectionUtils.GetFilteredTypeList<View>()
                .FirstOrDefault(type => type.Name == _type)
            ?? throw new NullReferenceException($"Type {nameof(_type)} not found");

        private List<string> GetFilteredNames()
        {
            return ReflectionUtils.GetFilteredTypeList<View>()
                .Select(type => type.Name)
                .ToList();
        }
        
        
        private void GetPrefab()
        {
            _prefab = LocalizationExtension
                .FindAssets<GameObject>($"t:GameObject")
            .FirstOrDefault(prefab => prefab.GetComponent(Type) != null);
        }
    }
}