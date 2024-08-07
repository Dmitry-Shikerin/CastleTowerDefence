using System;

namespace MyAudios.Soundy.Editor.Presenters.Controllers
{
    public interface IPresenter : IDisposable
    {
        void Initialize();
    }
}