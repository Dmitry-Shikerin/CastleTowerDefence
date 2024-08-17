using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.ConfigCollectors;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers
{
    public class PoolManagerConfig : Config
    {
        [ValueDropdown(nameof(GetFilteredNames))]
        [SerializeReference] private string _type;
        [SerializeField] private bool _isWarmUp;
        [Range(0, 5)]
        [SerializeField] private int _warmUpTime;
        [Range(0, 100)]
        [SerializeField] private int _warmUpCount;
        [MinMaxSlider(-1, 100, true)]
        [SerializeField] private Vector2Int _minMaxPoolCount = new Vector2Int(-1, 0);
        [Range(0, 20)]
        [SerializeField] private float _deleteAfterTime;
        
        public bool IsWarmUp => _isWarmUp;
        public int WarmUpCount => _warmUpCount;
        public int MinPoolCount => _minMaxPoolCount.y;
        public int MaxPoolCount => _minMaxPoolCount.x;
        public float WarmUpTime => _warmUpTime;
        public Type Type => GetTypeByName();
        
        private Type GetTypeByName() =>
            GetFilteredTypeList().FirstOrDefault(type => type.Name == _type)
            ?? throw new NullReferenceException($"Type {nameof(_type)} not found");

        private List<string> GetFilteredNames()
        {
            return GetFilteredTypeList()
                .Select(type => type.Name)
                .ToList();
        }
        
        private IEnumerable<Type> GetFilteredTypeList()
        {
            return typeof(View).Assembly.GetTypes()
                .Where(type => type.IsAbstract == false)
                .Where(type => type.IsGenericTypeDefinition == false)
                .Where(type => typeof(View).IsAssignableFrom(type));
        }
    }
}