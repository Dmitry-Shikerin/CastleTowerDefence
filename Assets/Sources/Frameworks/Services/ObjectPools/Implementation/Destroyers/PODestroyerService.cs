using Sources.Frameworks.Services.ObjectPools.Interfaces.Destroyers;
using Sources.Presentations.Views;

namespace Sources.Frameworks.Services.ObjectPools.Implementation.Destroyers
{
    public class PODestroyerService : IPODestroyerService
    {
        public void Destroy(View view)
        {
            if (view.TryGetComponent(out PoolableObject poolableObject) == false)
            {
                view.Destroy();

                return;
            }

            poolableObject.ReturnToPool();
            view.Hide();
        }
    }
}