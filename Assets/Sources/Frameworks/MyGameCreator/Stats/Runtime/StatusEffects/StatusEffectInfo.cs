using System;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime.StatusEffects
{
    [Serializable]
    public class StatusEffectInfo : TInfo
    {
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public StatusEffectInfo() : base()
        {
            this.m_Acronym = new PropertyGetString("SE");
            this.m_Name = new PropertyGetString("Status Effect Name");
            this.m_Description = new PropertyGetString("Description...");
        }
    }
}