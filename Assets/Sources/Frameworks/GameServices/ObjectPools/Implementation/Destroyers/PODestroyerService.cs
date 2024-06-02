using System;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Destroyers;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Destroyers
{
    public class PODestroyerService : IPODestroyerService
    {
        public void Destroy<T>(T view) where T : View
        {
            try
            {
                var poolableObject = CheckPoolableObject(view);
                poolableObject.ReturnToPool();
                CheckPoolableObject(view);
                view.Hide();
            }
            catch (NullReferenceException)
            {
            }
        }

        private PoolableObject CheckPoolableObject<T>(T view) where T : View
        {
            if (view.TryGetComponent(out PoolableObject poolableObject))
                return poolableObject;

            Object.Destroy(view.gameObject);

            throw new NullReferenceException(nameof(poolableObject));
        }
    }
}