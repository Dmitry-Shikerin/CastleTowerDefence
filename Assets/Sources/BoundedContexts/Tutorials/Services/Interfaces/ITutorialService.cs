using Sources.BoundedContexts.Tutorials.Domain;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.BoundedContexts.Tutorials.Services.Interfaces
{
    public interface ITutorialService : IInitialize
    {
        void Complete();

        void Construct(Tutorial tutorial);
    }
}