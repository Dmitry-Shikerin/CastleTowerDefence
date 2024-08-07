using System;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.Views
{
    public interface IView : IDisposable
    {
        VisualElement Root { get; }
        
        void Initialize();
        void CreateView();
    }
}