using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyAudios.MyUiFramework.Utils;
using MyAudios.Soundy.Sources.AudioControllers.Controllers;
using MyAudios.Soundy.Sources.Managers.Controllers;
using MyAudios.Soundy.Sources.Settings.Domain.Configs;
using UnityEngine;

namespace Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.AudioPoolers.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Dynamic sound pool manager for SoundyControllers
    /// </summary>
    [DefaultExecutionOrder(SoundyExecutionOrder.SoundyPooler)]
    public class SoundyPooler : MonoBehaviour
    {
        #region Static Properties

        /// <summary> Returns a reference to the SoundyPooler component that should be attached to the SoundyManager GameObject. If one does not exist, it gets added. </summary>
        public static SoundyPooler Instance => SoundyManager.Pooler;

        /// <summary> Auto kill any SoundyControllers that are have been unused for the set idle kill duration </summary>
        public static bool AutoKillIdleControllers => SoundySettings.Instance.AutoKillIdleControllers;

        /// <summary> The duration a SoundyController needs to be idle to be considered killable </summary>
        public static float ControllerIdleKillDuration => SoundySettings.Instance.ControllerIdleKillDuration;

        /// <summary> Time interval to check for idle SoundyControllers to kill them </summary>
        public static float IdleCheckInterval => SoundySettings.Instance.IdleCheckInterval;

        /// <summary> The minimum number of SoundyControllers that should be available in the pool, that will not get automatically killed even if they are killable </summary>
        public static int MinimumNumberOfControllers => SoundySettings.Instance.MinimumNumberOfControllers;

        /// <summary> Internal variable that holds a list of available SoundyControllers </summary>
        private static List<SoundyController> s_pool;

        /// <summary> The list of available SoundyControllers that make up the pool </summary>
        private static List<SoundyController> Pool 
        { 
            get => s_pool ?? (s_pool = new List<SoundyController>());
            set => s_pool = value;
        }

        #endregion
        
        #region Private Variables

        /// <summary> Internal variable that holds a reference to the coroutine that performs the check for idle controllers </summary>
        private Coroutine _idleCheckCoroutine;

        /// <summary> Wait interval between used by the coroutine that performs the check for idle controllers </summary>
        private WaitForSecondsRealtime _idleCheckIntervalWaitForSecondsRealtime;

        /// <summary> Internal variable used as a reference holder to minimise memory allocations </summary>
        private SoundyController _tempController;

        #endregion

        #region Unity Methods

        private void Reset() =>
            SoundySettings.Instance.ResetComponent(this);

        private void OnEnable()
        {
            if (!AutoKillIdleControllers) 
                return;
            
            StartIdleCheckInterval();
        }

        private void OnDisable() =>
            StopIdleCheckInterval();

        #endregion

        #region Static Methods

        /// <summary> Stop all SoundyControllers from playing, destroy the GameObjects they are attached to and clear the Pool </summary>
        /// <param name="keepMinimumNumberOfControllers"> Should there be a minimum set number of controllers in the pool left after clearing? </param>
        public static void ClearPool(bool keepMinimumNumberOfControllers = false)
        {
            if (keepMinimumNumberOfControllers)
            {
                RemoveNullControllersFromThePool();           //remove any null controllers (sanity check)
                
                if (Pool.Count <= MinimumNumberOfControllers) //make sure the minimum number of controllers are in the pool before killing them
                    return;

                int killedControllersCount = 0;
                
                for (int i = Pool.Count - 1; i >= MinimumNumberOfControllers; i--) //go through the pool
                {
                    SoundyController controller = Pool[i];
                    Pool.Remove(controller); //remove controller from the pool
                    controller.Kill();       //kill the controller
                    killedControllersCount++;
                }

                return;
            }

            SoundyController.KillAll();
            Pool.Clear();
        }

        /// <summary> Get a SoundyController from the Pool, or create a new one if all the available controllers are in use </summary>
        public static SoundyController GetControllerFromPool()
        {
            RemoveNullControllersFromThePool();
            if (Pool.Count <= 0)
                PutControllerInPool(SoundyController.CreateController()); //the pool does not have any controllers in it -> create and return a new controller
            
            SoundyController controller = Pool[0];                        //assign the first found controller
            Pool.Remove(controller);                                      //remove the assigned controller from the pool
            controller.gameObject.SetActive(true);
            
            return controller; //return a reference to the controller
        }

        /// <summary> Create a set number of SoundyControllers and add them to the Pool </summary>
        /// <param name="numberOfControllers"> How many controllers should be created </param>
        public static void PopulatePool(int numberOfControllers)
        {
            RemoveNullControllersFromThePool();
            
            if (numberOfControllers < 1) 
                return; //sanity check
            
            for (int i = 0; i < numberOfControllers; i++)
                PutControllerInPool(SoundyController.CreateController());
            // if (Instance.DebugComponent) DDebug.Log("Populate Pool - Added " + numberOfControllers + " Controllers to the Pool - " + Pool.Count + " Controllers Available", Instance);
        }

        /// <summary> Put a SoundyController back in the Pool </summary>
        /// <param name="controller"> The controller </param>
        public static void PutControllerInPool(SoundyController controller)
        {
            if (controller == null) 
                return;
            
            if (Pool.Contains(controller) == false) 
                Pool.Add(controller);
            
            controller.gameObject.SetActive(false);
            controller.transform.SetParent(Instance.transform);
        }

        #endregion

        #region Private Methods

        private void StartIdleCheckInterval()
        {
            _idleCheckIntervalWaitForSecondsRealtime = 
                new WaitForSecondsRealtime(IdleCheckInterval < 0 ? 0 : IdleCheckInterval);
            _idleCheckCoroutine = StartCoroutine(KillIdleControllersEnumerator());
        }

        private void StopIdleCheckInterval()
        {
            if (_idleCheckCoroutine == null) 
                return;
            
            StopCoroutine(_idleCheckCoroutine);
            _idleCheckCoroutine = null;
        }

        /// <summary> Removes any null controllers from the pool </summary>
        private static void RemoveNullControllersFromThePool() =>
            Pool = Pool.Where(p => p != null).ToList();

        #endregion

        #region Enumerators

        private IEnumerator KillIdleControllersEnumerator()
        {
            while (AutoKillIdleControllers)
            {
                yield return _idleCheckIntervalWaitForSecondsRealtime; //check idle wait interval
                
                RemoveNullControllersFromThePool();                     //remove any null controllers (sanity check)
                int minimumNumberOfControllers = MinimumNumberOfControllers > 0 ? MinimumNumberOfControllers : 0;
                float controllerIdleKillDuration = ControllerIdleKillDuration > 0 ? ControllerIdleKillDuration : 0;
                
                if (Pool.Count <= minimumNumberOfControllers) 
                    continue;            //make sure the minimum number of controllers are in the pool before killing any more of them
                
                for (int i = Pool.Count - 1; i >= minimumNumberOfControllers; i--) //go through the pool
                {
                    _tempController = Pool[i];
                    
                    if (_tempController.gameObject.activeSelf) 
                        continue;                     //controller is active do not kill it
                    
                    if (_tempController.IdleDuration < controllerIdleKillDuration) 
                        continue; //controller is not killable as it has not been idle for long enough
                    
                    Pool.Remove(_tempController);                                            //remove controller from the pool
                    _tempController.Kill();                                                  //kill the controller
                }
            }

            _idleCheckCoroutine = null;
        }

        #endregion
    }
}