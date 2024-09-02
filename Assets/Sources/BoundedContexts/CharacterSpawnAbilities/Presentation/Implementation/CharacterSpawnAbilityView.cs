using System.Collections.Generic;
using Doozy.Runtime.Reactor.Animators;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterSpawnAbilities.Controllers;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Extensions;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation.Types;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Implementation
{
    public class CharacterSpawnAbilityView : PresentableView<CharacterSpawnAbilityPresenter>, 
        ICharacterSpawnAbilityView, ISelfValidator
    {
        [SerializeField] private List<CharacterSpawnPoint> _meleeSpawnPoints;
        [SerializeField] private List<CharacterSpawnPoint> _rangeSpawnPoints;
        [SerializeField] private UIAnimator _healAnimator;

        public IReadOnlyList<ICharacterSpawnPoint> MeleeSpawnPoints => _meleeSpawnPoints;
        public IReadOnlyList<ICharacterSpawnPoint> RangeSpawnPoints => _rangeSpawnPoints;
        public UIAnimator HealAnimator => _healAnimator;

        public void Validate(SelfValidationResult result)
        {
            _meleeSpawnPoints.ValidateSpawnPoints(SpawnPointType.CharacterMelee, result);
            _rangeSpawnPoints.ValidateSpawnPoints(SpawnPointType.CharacterRanged, result);
        }

        [Button]
        public void AddSpawnPoints()
        {
            _meleeSpawnPoints = this.GetSpawnPoints(SpawnPointType.CharacterMelee);
            _rangeSpawnPoints = this.GetSpawnPoints(SpawnPointType.CharacterRanged);
        }
    }
}