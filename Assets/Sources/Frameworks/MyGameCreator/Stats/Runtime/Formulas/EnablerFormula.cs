using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using Unity.VisualScripting;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime.Formulas
{
    public class EnablerFormula : TEnablerValue<Unity.VisualScripting.Formula>
    {
        public EnablerFormula()
        { }

        public EnablerFormula(Unity.VisualScripting.Formula value) : base(false, value)
        { }
        
        public EnablerFormula(bool isEnabled, Unity.VisualScripting.Formula value) : base(isEnabled, value)
        { }
    }
}