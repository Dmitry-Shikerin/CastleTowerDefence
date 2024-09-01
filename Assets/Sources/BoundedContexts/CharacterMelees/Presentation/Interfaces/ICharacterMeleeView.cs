using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.Characters.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces
{
    public interface ICharacterMeleeView : ICharacterView
    {
        public float MassAttackRange { get; }
        public Vector3 MassAttackPoint { get; }
        public ICharacterAnimation Animation { get; }
        public CharacterHealthView HealthView { get; }
    }
}