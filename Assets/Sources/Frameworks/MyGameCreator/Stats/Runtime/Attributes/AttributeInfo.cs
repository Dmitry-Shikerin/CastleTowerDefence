using System;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using Sources.Frameworks.MyGameCreator.Stats.Runtime.StatusEffects;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [Serializable]
    public class AttributeInfo : TInfo
    {
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public AttributeInfo() : base()
        {
            this.m_Acronym = new PropertyGetString("ATT");
            this.m_Name = new PropertyGetString("Attribute Name");
            this.m_Description = new PropertyGetString("Description...");
        }
    }
}