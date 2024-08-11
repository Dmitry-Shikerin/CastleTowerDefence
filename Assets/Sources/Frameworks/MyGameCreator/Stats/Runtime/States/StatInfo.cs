using System;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using Sources.Frameworks.MyGameCreator.Stats.Runtime.StatusEffects;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [Serializable]
    public class StatInfo : TInfo
    {
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public StatInfo() : base()
        {
            this.m_Acronym = new PropertyGetString("STA");
            this.m_Name = new PropertyGetString("Stat Name");
            this.m_Description = new PropertyGetString("Description...");
        }
    }
}