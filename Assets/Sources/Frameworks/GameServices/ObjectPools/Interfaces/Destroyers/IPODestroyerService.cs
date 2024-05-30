using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Presentations.Views;

namespace Sources.Frameworks.Services.ObjectPools.Interfaces.Destroyers
{
    public interface IPODestroyerService
    {
        void Destroy(View view);
    }
}