using UnityEditor;
using UnityEngine.UIElements;

namespace Sources.Frameworks.MyGameCreator.Core.Editor.Common
{
    public class TypeSelectorValueFancy : TTypeSelectorValue<Button>
    {
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        protected TypeSelectorValueFancy(SerializedProperty property, Button element) 
            : base(property, element)
        { }

        protected override void SetupActivator(Button element)
        {
            element.clicked += () => TypeSelectorFancyPopup.Open(
                element, 
                this.m_Type, 
                this.OnSelectType
            );
        }
    }
}