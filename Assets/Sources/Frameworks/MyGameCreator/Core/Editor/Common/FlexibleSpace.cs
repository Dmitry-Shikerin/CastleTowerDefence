using UnityEngine.UIElements;

namespace Sources.Frameworks.MyGameCreator.Core.Editor.Common
{
    public sealed class FlexibleSpace : VisualElement
    {
        public FlexibleSpace()
        {
            this.style.flexGrow = 1;
        }
    }
}