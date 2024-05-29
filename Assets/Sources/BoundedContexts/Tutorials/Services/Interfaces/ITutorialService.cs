using Sources.BoundedContexts.Tutorials.Domain;
using Sources.InfrastructureInterfaces.Services.StatesLifetimes;

namespace Sources.BoundedContexts.Tutorials.Services.Interfaces
{
    public interface ITutorialService : IEnable
    {
        void Complete();

        void Construct(Tutorial tutorial);
    }
}