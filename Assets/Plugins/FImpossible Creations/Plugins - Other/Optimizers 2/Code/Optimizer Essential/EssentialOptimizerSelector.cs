using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    //[CreateAssetMenu(menuName = "Essential Types Setup")]
    public class EssentialOptimizerSelector : ScriptableObject
    {

        #region Getting Setup File     

        public static EssentialOptimizerSelector GetSelector
        {
            get
            {
                if (_settingsRef != null) return _settingsRef;

                _settingsRef = Resources.Load<EssentialOptimizerSelector>("Optimizers/EssentialSelector");

                if (_settingsRef == null)
                {
#if UNITY_EDITOR
                    UnityEngine.Object resourcesDir = Resources.Load<UnityEngine.Object>("Optimizers/Optimizers Settings Info");

                    if (resourcesDir == null)
                    {
                        UnityEngine.Debug.Log("[Optimizers ERROR] Not found file under 'resources/Optimizers/Optimizers Settings Info'. It's required to generate project settings files properly!");
                        _settingsRef = CreateInstance<EssentialOptimizerSelector>();
                    }
                    else
                    {
                        string path = UnityEditor.AssetDatabase.GetAssetPath(resourcesDir);
                        path = System.IO.Path.GetDirectoryName(path);
                        EssentialOptimizerSelector settingsFile = CreateInstance<EssentialOptimizerSelector>();
                        UnityEditor.AssetDatabase.CreateAsset(settingsFile, path + "/EssentialSelector.asset");
                        UnityEditor.AssetDatabase.SaveAssets();
                        UnityEditor.AssetDatabase.Refresh();
                        //_settingsRef = Resources.Load<EssentialOptimizerSelector>("Optimizers/EssentialSelector");
                        _settingsRef = settingsFile;

                        if (_settingsRef == null)
                        {
                            UnityEngine.Debug.Log("[Optimizers ERROR] Failed to generate settings file under 'resources/Optimizers/EssentialSelector'!");
                        }
                    }
#else
                    UnityEngine.Debug.Log("[ERROR] No Optimizers EssentialSelector file has been found! Generating new with default settings.");
                    _settingsRef = CreateInstance<EssentialOptimizerSelector>();
#endif
                }

                return _settingsRef;
            }
        }

        static EssentialOptimizerSelector _settingsRef = null;

        #endregion

        public bool Light = true;
        public bool Particle = true;
        public bool Renderer = true;
        public bool MonoBehaviour = true;
        public bool AudioSource = false;
        public bool NavMeshAgent = false;
        public bool Rigidbody = false;
        //public bool LODGroup = false;

        //private void OnValidate()
        //{
        //    Optimizer_Base._HandleUnityLODWithReload = LODGroup;
        //}

        public bool IsTypeAllowed(EssentialLODsController.EEssType type)
        {
            switch (type)
            {
                case EssentialLODsController.EEssType.Particle: if (Particle) return true; break;
                case EssentialLODsController.EEssType.Light: if (Light) return true; break;
                case EssentialLODsController.EEssType.MonoBehaviour: if (MonoBehaviour) return true; break;
                case EssentialLODsController.EEssType.Renderer: if (Renderer) return true; break;
                case EssentialLODsController.EEssType.NavMeshAgent: if (NavMeshAgent) return true; break;
                case EssentialLODsController.EEssType.AudioSource: if (AudioSource) return true; break;
                case EssentialLODsController.EEssType.Rigidbody: if (Rigidbody) return true; break;
                //case EEssType.LODGroup: if (t.lo) return true; break;
            }

            return false;
        }

    }


#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(EssentialOptimizerSelector))]
    public class EssentialOptimizerSelectorEditor : UnityEditor.Editor
    {
        public EssentialOptimizerSelector Get { get { if (_get == null) _get = (EssentialOptimizerSelector)target; return _get; } }
        private EssentialOptimizerSelector _get;

        //private void OnEnable()
        //{
        //    Get.LODGroup = Optimizer_Base._HandleUnityLODWithReload;
        //}

        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("Choose Components to be automatically detected by Essential Optimizer", UnityEditor.MessageType.Info);
            GUILayout.Space(4f);
            DrawDefaultInspector();
            GUILayout.Space(4f);
        }
    }
#endif

}