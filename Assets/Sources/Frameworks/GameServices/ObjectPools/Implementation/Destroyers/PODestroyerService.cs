using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Destroyers;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Destroyers
{
    public class PODestroyerService : IPODestroyerService
    {
        public void Destroy<T>(T view) where T : View
        {
            if (view.TryGetComponent(out PoolableObject poolableObject) == false)
            {
                Object.Destroy(view.gameObject);
                Debug.Log($"PoolableObject not found: {view.gameObject.name}");

                return;
            }

            Debug.Log($"PoolableObject found: {view.gameObject.name}");
            poolableObject.ReturnToPool();
            view.Hide();
        }
    }
}