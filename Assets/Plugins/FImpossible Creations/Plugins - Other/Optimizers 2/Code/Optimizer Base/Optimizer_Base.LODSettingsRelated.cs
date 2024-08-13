
namespace FIMSpace.FOptimizing
{
    public abstract partial class Optimizer_Base
    {
        public virtual int GetToOptimizeCount() { return 0; }

        /// <summary> Reassigning auto settings </summary>
        public abstract void RefreshLODSettings();

        /// <summary> You can use it if you want to disable optimization on some object to prevent it from disappearing etc.
        /// Switching all optimized component states to initial values and disabling optimizer actions </summary>
        public virtual void SwitchOFFOptimizer()
        {
            ChangeLODLevelTo(0);
            SetCulled(false);
            enabled = false;
        }

        /// <summary> If optimizer was OFF, Switching ON optimized component states to currrent distance LOD </summary>
        public virtual void SwitchONOptimizer()
        {
            enabled = true;
            ChangeLODLevelTo(CurrentDistanceLODLevel);
            if (CullIfNotSee) if (OutOfCameraView) SetCulled(true);
        }

        /// <summary> Calling SwitchONOptimizer or SwitchOFFOptimizer methods </summary>
        public void SwitchOptimizerEnabled(bool enable)
        {
            if (enabled == enable) return;

            if (enable) SwitchONOptimizer();
            else SwitchOFFOptimizer();
        }

    }
}
