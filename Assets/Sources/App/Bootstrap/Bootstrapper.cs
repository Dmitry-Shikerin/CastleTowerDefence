using System;
using Cysharp.Threading.Tasks;
using Sources.App;
using Sources.App.Factories;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private AppCore _appCore;

    private void Awake()
    {
        _appCore = FindObjectOfType<AppCore>() ?? new AppCoreFactory().Create();
    }
}
