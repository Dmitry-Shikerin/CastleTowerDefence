using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.Presentation.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Presentation.Implementation
{
    public class CharacterMeleeView : CharacterView, ICharacterMeleeView
    {
        [SerializeField] private Transform _massAttackPoint;
        [Range(1,5)]
        [SerializeField] private float _massAttackRange;

        public float MassAttackRange => _massAttackRange;
        public Vector3 MassAttackPoint => _massAttackPoint.position;
    }
}