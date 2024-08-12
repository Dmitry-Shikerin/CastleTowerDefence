using UnityEngine.UIElements;

namespace Sources.Frameworks.MyGameCreator.Core.Editor.Common
{
    public class SpaceSmaller : VisualElement
    {
        public SpaceSmaller()
        {
            this.style.height = new StyleLength(5);
        }
    }
}