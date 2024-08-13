using UnityEngine;

namespace FIMSpace.FOptimizing
{
    //[CreateAssetMenu(menuName = "Optimizer 2019_4+ Types Setup")]
    public class Optimizer2020Selector : ScriptableObject
    {

        #region Getting Setup File     

        public static Optimizer2020Selector GetSelector
        {
            get
            {
                if (_settingsRef != null) return _settingsRef;

                _settingsRef = Resources.Load<Optimizer2020Selector>("Optimizers/Optimizer2020Selector");

                if (_settingsRef == null)
                {
#if UNITY_EDITOR
                    UnityEngine.Object resourcesDir = Resources.Load<UnityEngine.Object>("Optimizers/Optimizers Settings Info");

                    if (resourcesDir == null)
                    {
                        UnityEngine.Debug.Log("[Optimizers ERROR] Not found file under 'resources/Optimizers/Optimizers Settings Info'. It's required to generate project settings files properly!");
                        _settingsRef = CreateInstance<Optimizer2020Selector>();
                    }
                    else
                    {
                        string path = UnityEditor.AssetDatabase.GetAssetPath(resourcesDir);
                        path = System.IO.Path.GetDirectoryName(path);
                        Optimizer2020Selector settingsFile = CreateInstance<Optimizer2020Selector>();
                        UnityEditor.AssetDatabase.CreateAsset(settingsFile, path + "/Optimizer2020Selector.asset");
                        UnityEditor.AssetDatabase.Refresh();
                        UnityEditor.AssetDatabase.SaveAssets();
                        //_settingsRef = Resources.Load<Optimizer2020Selector>("Optimizers/Optimizer2020Selector");
                        _settingsRef = settingsFile;

                        if (_settingsRef == null)
                        {
                            UnityEngine.Debug.Log("[Optimizers ERROR] Failed to generate settings file under 'resources/Optimizers/Optimizer2020Selector'!");
                        }
                    }
#else
                    UnityEngine.Debug.Log("[ERROR] No Optimizers Optimizer2020Selector file has been found! Generating new with default settings.");
                    _settingsRef = CreateInstance<Optimizer2020Selector>();
#endif
                }

                return _settingsRef;
            }
        }

        static Optimizer2020Selector _settingsRef = null;

        #endregion


        public bool Light = true;
        public bool Particle = true;
        public bool Renderer = true;
        public bool SkinnedRenderer = true;
        public bool MonoBehaviour = true;
        public bool AudioSource = false;
        public bool NavMeshAgent = false;
        public bool Rigidbody = false;

        //public bool LODGroup = false;

        //private void OnValidate()
        //{
        //    Optimizer_Base._HandleUnityLODWithReload = LODGroup;
        //}

        public bool IsTypeAllowed(Component type)
        {
            if (type is ParticleSystem) return Particle;
            else if (type is Light) return Light;
            else if (type is SkinnedMeshRenderer) return SkinnedRenderer;
            else if (type is Renderer) return Renderer;
            else if (type is MonoBehaviour) return MonoBehaviour;
            else if (type is AudioSource) return AudioSource;
            else if (type is UnityEngine.AI.NavMeshAgent) return NavMeshAgent;
            else if (type is Rigidbody) return Rigidbody;

            return true; // Not mentioned, so allow
        }

        #region Editor Code

#if UNITY_EDITOR
        [UnityEditor.CanEditMultipleObjects]
        [UnityEditor.CustomEditor(typeof(Optimizer2020Selector))]
        public class Optimizer2020SelectorEditor : UnityEditor.Editor
        {
            public Optimizer2020Selector Get { get { if (_get == null) _get = (Optimizer2020Selector)target; return _get; } }
            private Optimizer2020Selector _get;

            public override void OnInspectorGUI()
            {
                UnityEditor.EditorGUILayout.HelpBox("Choose Components to be automatically detected by 'Optimizer 2019.4+'", UnityEditor.MessageType.Info);
                GUILayout.Space(4f);
                DrawDefaultInspector();
                GUILayout.Space(4f);
            }
        }
#endif

        #endregion

    }



}
