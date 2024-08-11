using System;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using Sources.Frameworks.MyGameCreator.Stats.Runtime.StatusEffects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [Serializable]
    public class AttributeInfo
    {
        [SerializeField] private string _acronym;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        
        public string Acronym => _acronym;
        public string Name => _name;
        public string Description => _description;
    }
}