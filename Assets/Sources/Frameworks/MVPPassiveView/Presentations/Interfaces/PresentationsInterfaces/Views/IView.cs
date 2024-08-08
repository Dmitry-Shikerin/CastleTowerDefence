using UnityEngine;

namespace Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views
{
    public interface IView
    {
        IView Show();
        void Hide();
        void SetParent(Transform parent);
        void SetLocalePosition(Vector3 position);
        void SetPosition(Vector3 position);
        void SetRotation(Vector3 rotation);
        public void SetRotation(Quaternion rotation);
        void Destroy();
    }
}