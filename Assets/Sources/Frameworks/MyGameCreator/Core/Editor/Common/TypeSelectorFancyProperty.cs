using UnityEditor;
using UnityEngine.UIElements;

namespace Sources.Frameworks.MyGameCreator.Core.Editor.Common
{
    public class TypeSelectorFancyProperty : TypeSelectorValueFancy
    {
        public TypeSelectorFancyProperty(SerializedProperty property, Button element)
            : base(property, element)
        { }
    }
}