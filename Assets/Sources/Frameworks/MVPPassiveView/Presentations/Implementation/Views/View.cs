using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views
{
    public abstract class View : MonoBehaviour, IView
    {
        public IView Show()
        {
            gameObject.SetActive(true);
            return this;
        }

        public void Hide() =>
            gameObject.SetActive(false);

        public void SetParent(Transform parent) =>
            gameObject.transform.SetParent(parent);

        public void SetLocalePosition(Vector3 position) =>
            transform.localPosition = position;

        public void SetPosition(Vector3 position) =>
            transform.position = position;

        public void SetRotation(Vector3 rotation) =>
            transform.rotation = Quaternion.Euler(rotation);
        
        public void SetRotation(Quaternion rotation) =>
            transform.rotation = rotation;

        public virtual void Destroy() =>
            Destroy(gameObject);
    }
}