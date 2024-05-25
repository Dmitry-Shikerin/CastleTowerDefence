using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Frameworks.UiFramework.Presentation.Animations;
using Sources.InfrastructureInterfaces.Services.StatesLifetimes;
using Sources.PresentationsInterfaces.Views.Constructors;

namespace Sources.Frameworks.UiFramework.ServicesInterfaces
{
    public interface IViewService : IAwake, IEnable, IDisable, IDestroy, IConstruct<UiAnimator>
    {
    }
}