using Cysharp.Threading.Tasks;

namespace Sources.Frameworks.GameServices.Curtains.Presentation.Interfaces
{
    public interface ICurtainView
    {
        public UniTask ShowAsync();
        public UniTask HideAsync();
    }
}