using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [Serializable]
    public class StatData
    {
        [SerializeField] private double m_Base = 0;
        [SerializeField] private Formula m_Formula = null;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public double Base => this.m_Base;
        public Formula Formula => this.m_Formula;
    }
}