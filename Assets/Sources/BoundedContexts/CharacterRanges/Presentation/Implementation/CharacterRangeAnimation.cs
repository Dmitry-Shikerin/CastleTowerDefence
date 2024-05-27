using UnityEngine;
using Sources.BoundedContexts.Animations.Presentations;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterRanges.Presentation.Implementation
{
    public class CharacterRangeAnimation : AnimationViewBase, ICharacterRangeAnimation
    {
        private static int s_isIdle = Animator.StringToHash("IsIdle");
        private static int s_isAttack = Animator.StringToHash("IsAttack");

        private void Awake()
        {
            StoppingAnimations.Add(StopPlayIdle);
            StoppingAnimations.Add(StopPlayAttack);
        }

        public void PlayIdle()
        {
            ExceptAnimation(StopPlayIdle);
            Animator.SetBool(s_isIdle, true);
        }

        public void PlayAttack()
        {
            ExceptAnimation(StopPlayAttack);
            Animator.SetBool(s_isAttack, true);
        }

        private void StopPlayIdle() =>
            Animator.SetBool(s_isIdle, false);
        
        private void StopPlayAttack() =>
            Animator.SetBool(s_isAttack, false);
    }
}