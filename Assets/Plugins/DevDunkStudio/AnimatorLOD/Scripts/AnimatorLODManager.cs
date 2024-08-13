using System.Collections.Generic;
using UnityEngine;

namespace DevDunk.AutoLOD
{
    public class AnimatorLODManager : MonoBehaviour
    {
        private static AnimatorLODManager instance;

        public static AnimatorLODManager Instance
        {
            get => instance;
            private set
            {
                if (instance)
                {
                    Debug.LogWarning($"Old instance of AnimatorLOD was found on {instance.name}", value.gameObject);

                    //Set animators from previous instance to this instance
                    value.Animators = instance.Animators;
                    value.LODs = instance.LODs;

                    Destroy(instance);
                }

                instance = value;
            }
        }

        public bool IsRunning { get; private set; }

        [Tooltip("Camera transform to use for LOD calculations. If not set will use Camera.main")]
        public Transform cameraTransform;

        [Tooltip("If true, the LOD system will run at start. If false, start manually with EnableAnimatorLOD")]
        public bool RunAtStart = true;

        [Tooltip("LOD settings for the animators")]
        public LODSettings[] LODs = new LODSettings[]
        {
            new LODSettings { Distance = 10f, frameCount = 0, MaxBoneWeight = SkinQuality.Bone4 },
            new LODSettings { Distance = 20f, frameCount = 1, MaxBoneWeight = SkinQuality.Bone2 },
            new LODSettings { Distance = 30f, frameCount = 2, MaxBoneWeight = SkinQuality.Bone2 },
            new LODSettings { Distance = 40f, frameCount = 4, MaxBoneWeight = SkinQuality.Bone1 },
        };

        private List<AnimatorLODObject> Animators = new List<AnimatorLODObject>();

        private void Awake()
        {
            Instance = this;
            IsRunning = RunAtStart;
        }

        private void Start() =>
            cameraTransform = Camera.main.transform;

        /// <summary>
        /// Add a new animator to the LOD System
        /// </summary>
        /// <param name="animator">Animator object to add</param>
        public void AddAnimator(AnimatorLODObject animator) =>
            Animators.Add(animator);

        /// <summary>
        /// Remove an animator to the LOD System
        /// </summary>
        /// <param name="animator">Animator object to remove</param>
        public void RemoveAnimator(AnimatorLODObject animator) =>
            Animators.Remove(animator);

        /// <summary>
        /// Disable Animator LOD System
        /// </summary>
        public void DisableAnimatorLOD()
        {
            if (IsRunning == false)
                return;

            foreach (AnimatorLODObject animator in Animators)
                animator.DisableLODSystem();

            IsRunning = false;
        }

        /// <summary>
        /// Enable Animator LOD System
        /// </summary>
        public void EnableAnimatorLOD()
        {
            if (IsRunning)
                return;

            foreach (AnimatorLODObject animator in Animators)
                animator.EnableLODSystem();

            IsRunning = true;
        }

        private void Update()
        {
            if (!IsRunning || !cameraTransform) return;

            Vector3 camPos = cameraTransform.position;

            //Loop through all animators and check if they need to be enabled or disabled
            foreach (AnimatorLODObject animator in Animators)
            {
                if (animator.FrameCountdown <= 0)
                {
                    int frameCount = GetClosestLODFrameCountBinaray(
                        animator.TrackedTransform.position, camPos, out SkinQuality quality);
                    animator.EnableAnimator(frameCount, frameCount + 1, quality);

                    continue;
                }

                animator.DisableAnimator();
            }
        }

        /// <summary>
        /// Get the required LOD for the animator (legacy)
        /// </summary>
        /// <param name="position">Animator position</param>
        /// <param name="cameraPosition">Camera position</param>
        /// <param name="quality">Quality for this mesh renderer</param>
        /// <returns>Returns which LOD needs to be used</returns>
        private int GetClosestLODFrameCount(Vector3 position, Vector3 cameraPosition, out SkinQuality quality)
        {
            float distanceToCamera = Vector3.Distance(position, cameraPosition);

            for (int i = 0; i < LODs.Length; i++)
            {
                if (distanceToCamera > LODs[i].Distance)
                {
                    quality = LODs[i].MaxBoneWeight;
                    return LODs[i].frameCount;
                }
            }

            quality = SkinQuality.Auto;
            
            return 0;
        }

        /// <summary>
        /// Get the required LOD for the animator
        /// </summary>
        /// <param name="position">Animator position</param>
        /// <param name="cameraPosition">Camera position</param>
        /// <param name="quality">Quality for this mesh renderer</param>
        /// <returns>Returns which LOD needs to be used</returns>
        private int GetClosestLODFrameCountBinaray(Vector3 position, Vector3 cameraPosition, out SkinQuality quality)
        {
            float distanceToCamera = Vector3.Distance(position, cameraPosition);
            
            return BinarySearchClosestLODIndex(distanceToCamera, out quality);
        }

        /// <summary>
        /// Binary search for the closest LOD for performance
        /// </summary>
        /// <param name="distance">Distance between camera and animator</param>
        /// <param name="quality">Quality for this mesh renderer</param>
        /// <returns>Returns which LOD needs to be used</returns>
        private int BinarySearchClosestLODIndex(float distance, out SkinQuality quality)
        {
            int low = 0;
            int high = LODs.Length - 1;
            int closestIndex = 0;

            while (low <= high)
            {
                int mid = (low + high) / 2;

                if (LODs[mid].Distance == distance)
                {
                    closestIndex = mid;
                    break;
                }
                else if (LODs[mid].Distance < distance)
                {
                    closestIndex = mid;
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            quality = LODs[closestIndex].MaxBoneWeight;
            return LODs[closestIndex].frameCount;
        }

        /// <summary>
        /// Set the camera transform to be used for LOD calculations
        /// </summary>
        /// <param name="camera">Camera component from which the transform is taken</param>
        public void SetCameraTransform(Camera camera) =>
            cameraTransform = camera.transform;

        /// <summary>
        /// Set the camera transform to be used for LOD calculations
        /// </summary>
        /// <param name="camera">Camera transform used</param>
        public void SetCameraTransform(Transform camera) =>
            cameraTransform = camera;
    }
}