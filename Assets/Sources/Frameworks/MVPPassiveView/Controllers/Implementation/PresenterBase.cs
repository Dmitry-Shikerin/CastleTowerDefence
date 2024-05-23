using Sources.ControllersInterfaces;

namespace Sources.Controllers.Common
{
    public abstract class PresenterBase : IPresenter
    {
        public virtual void Enable()
        {
        }

        public virtual void Disable()
        {
        }
    }
}