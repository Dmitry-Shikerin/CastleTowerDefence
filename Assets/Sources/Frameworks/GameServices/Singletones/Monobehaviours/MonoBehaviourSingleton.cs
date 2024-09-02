using UnityEngine;

namespace Sources.Frameworks.GameServices.Singletones.Monobehaviours
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = (T)FindObjectOfType(typeof(T));

                if (s_instance == null)
                    s_instance = new GameObject(typeof(T).Name).AddComponent<T>();

                return s_instance;
            }
        }
    }
}