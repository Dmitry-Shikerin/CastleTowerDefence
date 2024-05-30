using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.PresentationsInterfaces.Views;

namespace Sources.Presentations.Views
{
    public class ContainerView : View
    {
        public void AppendChild(IView view) =>
            view.SetParent(transform);
    }
}
