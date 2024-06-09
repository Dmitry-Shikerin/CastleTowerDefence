using System;
using JetBrains.Annotations;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Presentation.Implementation
{
    public class EnemyBossAnimation : EnemyAnimation, IEnemyBossAnimation
    {
        private static int s_isRun = Animator.StringToHash("IsRun");
        
        public event Action ScreamAnimationEnded;

        protected override void OnAfterAwake() =>
            StoppingAnimations.Add(StopRun);

        public void PlayRun()
        {
            ExceptAnimation(StopRun);
            Animator.SetBool(s_isRun, true);
        }

        private void StopRun()
        {
            if(Animator.GetBool(s_isRun) == false)
                return;
            
            Animator.SetBool(s_isRun, false);
        }
        
        [UsedImplicitly]
        private void OnScreamAnimationEnded() =>
            ScreamAnimationEnded?.Invoke();
    }
}