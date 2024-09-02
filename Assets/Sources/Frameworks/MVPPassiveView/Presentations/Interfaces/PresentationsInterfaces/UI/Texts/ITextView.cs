using System.Threading;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using UnityEngine;

namespace Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts
{
    public interface ITextView : IEnable, IDisable
    {
        bool IsHide { get; }
        
        void SetText(string text);
        void SetIsHide(bool isHide);
        void SetTextColor(Color color);
        void SetClearColorAsync(CancellationToken cancellationToken);
    }
}