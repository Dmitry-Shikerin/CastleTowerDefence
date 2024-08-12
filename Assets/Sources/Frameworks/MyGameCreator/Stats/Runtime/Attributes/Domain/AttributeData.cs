using System;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [Serializable]
    public class AttributeData
    {
        [SerializeField] private double _minValue;
        [SerializeField] private Stat _maxValue;
        [SerializeField] [Range(0f, 1f)] private float _startPercent = 1f;
        
        public double MinValue => _minValue;
        public Stat MaxValue => _maxValue;
        
        public float StartPercent => _startPercent;
    }
}