using Sources.BoundedContexts.Animations.Presentations;
using Sources.BoundedContexts.Characters.PresentationInterfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Characters.Presentation
{
    public class CharacterAnimation : AnimationViewBase, ICharacterAnimation
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