using System;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime.StatusEffects
{
    [Serializable]
    public class TInfo
    {
        [SerializeField] public PropertyGetString m_Name;
        [SerializeField] public PropertyGetString m_Acronym;
        [SerializeField] public PropertyGetString m_Description;
        
        [SerializeField] public PropertyGetSprite m_Icon;
        [SerializeField] public Color m_Color;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TInfo()
        {
            this.m_Icon = GetSpriteNone.Create;
            this.m_Color = Color.white;
        }
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Color Color => this.m_Color;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public string GetName(Args args) => this.m_Name.Get(args);
        public string GetAcronym(Args args) => this.m_Acronym.Get(args);
        public string GetDescription(Args args) => this.m_Description.Get(args);
        public Sprite GetIcon(Args args) => this.m_Icon.Get(args);
    }
}