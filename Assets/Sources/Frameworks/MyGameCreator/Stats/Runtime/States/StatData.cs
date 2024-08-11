using System;
using Sources.Frameworks.MyGameCreator.Stats.Tables.Domain;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [Serializable]
    public class StatData
    {
       [SerializeField] private double _baseValue = 0;
       [SerializeField] private Table _table;
        
        public double BaseValue => _baseValue;
        public Table Table => _table != null ? _table : throw new NullReferenceException();
    }
}