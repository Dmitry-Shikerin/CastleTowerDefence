using Sources.App.Core;
using Sources.App.Factories;
using UnityEngine;

namespace Sources.App.Bootstrap
{
    public class Bootstrapper : MonoBehaviour
    {
        private AppCore _appCore;

        private void Awake()
        {
            _appCore = FindObjectOfType<AppCore>() ?? new AppCoreFactory().Create();
        }
    }
}
