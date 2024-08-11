
using System;
using Sources.Frameworks.MyGameCreator.Core.Editor.Common;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [Serializable]
    public class OverrideAttributeData
    {
#pragma warning disable 414
        [SerializeField] [HideInInspector] 
        private bool m_IsExpanded = false;
#pragma warning restore 414
        
        [SerializeField] private EnablerRatio m_ChangeStartPercent = new EnablerRatio(false, 1f);

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool ChangeStartPercent => this.m_ChangeStartPercent.IsEnabled;
        public double StartPercent => this.m_ChangeStartPercent.Value;
    }
}