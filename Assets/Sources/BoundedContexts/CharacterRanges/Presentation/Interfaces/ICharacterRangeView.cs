using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces
{
    public interface ICharacterRangeView : IView
    {
        float FindRange { get; }
        Vector3 Position { get; }
        IEnemyHealthView EnemyHealth { get; }

        public void SetEnemyHealth(IEnemyHealthView enemyHealthView);
        void SetLookRotation(float angle);
        void PlayShootParticle();
    }
}