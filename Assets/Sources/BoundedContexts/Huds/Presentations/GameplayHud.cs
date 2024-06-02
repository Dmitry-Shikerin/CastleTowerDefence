using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Huds;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.Huds.Presentations
{
    public class GameplayHud : MonoBehaviour, IHud
    {
        public UiCollector UiCollector { get; }
    }
}