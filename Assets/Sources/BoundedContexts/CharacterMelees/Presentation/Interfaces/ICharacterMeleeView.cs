using Sources.BoundedContexts.AttackTargetFinders.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.Characters.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
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