using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public class OptimizersSetup : ScriptableObject
    {
        #region Settings File Prepare

        static OptimizersSetup SettingsReference
        {
            get
            {
                if (_settingsRef != null) return _settingsRef;

                _settingsRef = Resources.Load<OptimizersSetup>("Optimizers/Optimizers Setup");

                if (_settingsRef == null)
                {
#if UNITY_EDITOR
                    UnityEngine.Object resourcesDir = Resources.Load<UnityEngine.Object>("Optimizers/Optimizers Settings Info");
                    
                    if ( resourcesDir == null)
                    {
                        UnityEngine.Debug.Log("[Optimizers ERROR] Not found file under 'resources/Optimizers/Optimizers Settings Info'. It's required to generate project settings file properly!");
                        _settingsRef = CreateInstance<OptimizersSetup>();
                    }
                    else
                    {
                        string path = UnityEditor.AssetDatabase.GetAssetPath(resourcesDir);
                        path = System.IO.Path.GetDirectoryName(path);
                        OptimizersSetup settingsFile = CreateInstance<OptimizersSetup>();
                        UnityEditor.AssetDatabase.CreateAsset(settingsFile, path + "/Optimizers Setup.asset");
                        UnityEditor.AssetDatabase.Refresh();
                        UnityEditor.AssetDatabase.SaveAssets();
                        _settingsRef = Resources.Load<OptimizersSetup>("Optimizers/Optimizers Setup");

                        if (_settingsRef == null)
                        {
                            UnityEngine.Debug.Log("[Optimizers ERROR] Failed to generate settings file under 'resources/Optimizers/Optimizers Settings'!");
                            _settingsRef = CreateInstance<OptimizersSetup>();
                        }
                    }
#else
                    UnityEngine.Debug.Log("[ERROR] No Optimizers Project Settings file has been found! Generating new with default settings.");
                    _settingsRef = CreateInstance<OptimizersSetup>();
#endif
                }

                return _settingsRef;
            }
        }

        static OptimizersSetup _settingsRef = null;

#if UNITY_EDITOR

        static UnityEditor.SerializedObject _baseSO = null;

        public static UnityEditor.SerializedObject BaseSerializedObject
        {
            get
            {
                if (Settings == null) return null;

                if (_baseSO == null || _baseSO.targetObject != _settingsRef)
                {
                    _baseSO = new UnityEditor.SerializedObject(_settingsRef);
                }

                return _baseSO;
            }
        }

#endif

#endregion

        /// <summary> Access it for global optimizers system settings </summary>
        public static OptimizersSetup Settings { get { return SettingsReference; } }

        [FPD_Header("Main Settings", 10)]
        [Tooltip("Allow to search for components to optimize, which are inside deactivated game objects")]
        [FPD_Width(284)]
        public bool SearchForComponentsInDeactivatedObjects = false;

        [Tooltip("If you want to allow generating optimizers manager object in scenes during edit mode, for accessing more scene specific settings.")]
        public bool GenerateManagerInEditMode = true;


        [FPD_Header("Default Manager Settings", 20)]

        [Tooltip("If you use VR SDK or some auto main camera creation/assign logics, increase this value to for example 3 to let engine do main camera calculations and then assign new camera for optimizers automatically")]
        public int GetCameraAfter = 0;

        [Tooltip("(Check this parameter in the OptimizersManager object for more details)\nWhen you adding this component, algorithm is adapting this value as MainCamera Far Clipping planes are setted*\n\nAutomatic optimization distance values basing on main character size - Check human scale gizmo in scene view next to camera (It can need other adjustement anyway - depends of project needs)")]
        public float WorldScale = 2.0f;

        [Tooltip("You should use as many as you can optimizers with same LOD distances and LODs counts to get best from culling containers.\n\nThis number defines how many slots should pre define each container for target optimizers components.\n\nWhen you use many components with different LOD counts or different LOD distance settings and there is only few (for example 200) objects to optimize in each distance range / lod count set you should change this number to be lower to not prepare too much slots for target optimizers (tiny bit higer RAM usage if capacity size is too big)")]
        [FPD_Width(164)]
        public int SingleContainerCapacity = 300;


        #region DOTS Settings Related

#if OPTIMIZERS_DOTS_IMPORTED

        [FPD_Header("DOTS Related Features", 30)]
        [FPD_Width(164)]
        public bool UseProgressiveCulling = false;

        [Tooltip("Which Layers should be treated as obstacles in sight.\nIf it's the same like 'OptimizersCullingLayer' then only objects with optimizers will be able to cover other objects with optimizers.\nUse another layers to make them cover optimized objects (without optimizers will not be hidden - they will be just sight obstacles).")]
        [FPD_Width(164)]
        public LayerMask ProgressiveCullingMask = 1 << 4;

        [Tooltip("Layer for optimizers detection colliders, should be unique for better performance. (this layer will be applied for culling detection colliders generated when game starts)")]
        [FPD_Width(164)]
        [FPD_Layers] public int OptimizersCullingLayer = 4;

        [Tooltip("Allowing raycasts going through objects with 'Is Obstacle = false' under OptimizerReference component. Means it can be used for example on lights -> can be occluded but can't occlude others")]
        [FPD_Width(164)]
        public bool SupportNotObstacles = true;

        [Tooltip("Higher value = shorter time for disappearing objects outside camera frustum and higher precision but slightly bigger load on performance.\nIf your scene have a lot of small detail objects for culling you should put response quality higher but if your scenes have just many medium/sized objects with optimizers on then you can lower it.\nIf some objects starts to disappear and appear every few frames that means response quality is too low.")]
        //[FPD_Width(164)]
        [SerializeField][Range(250, 3000)] public int ProgressiveResponseQuality = 1000;

        [Tooltip("Auto Set progress delay to hide objects")]
        public bool ProgAutoDelay = true;

        [Tooltip("Target progress time delay in seconds to hide objects")]
        [FPD_Width(164)]
        [SerializeField] public float ProgCullDelay = 1.5f;

        [Tooltip("Automatically refresh progressive culling range when screen size changes or camera's FOV")]
        [FPD_Width(224)]
        public bool AutoDetectFOVAndScreenChange = false;

#endif

        #endregion

        /// <summary> Marking setting file as dirty to make unity save its changes </summary>
        public static void OnSettingsChange()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(Settings);
#endif
        }

    }
}