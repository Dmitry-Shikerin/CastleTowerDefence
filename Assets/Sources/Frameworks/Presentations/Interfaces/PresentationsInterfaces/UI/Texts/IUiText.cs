using System.Threading;
using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Domain.Models.TextViewTypes;
using Sources.InfrastructureInterfaces.Services.StatesLifetimes;
using UnityEngine;

namespace Sources.PresentationsInterfaces.UI.Texts
{
    public interface IUiText : IEnable, IDisable
    {
        TextViewType TextViewType { get; }
        bool IsHide { get; }
        string Id { get; }
        
        void SetText(string text);
        void SetIsHide(bool isHide);
        void SetTextColor(Color color);
        void SetClearColorAsync(CancellationToken cancellationToken);
    }
}