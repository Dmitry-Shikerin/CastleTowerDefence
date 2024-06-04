using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterSpawnAbilities.Controllers;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Extensions;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation.Types;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Implementation
{
    public class CharacterSpawnAbilityView : PresentableView<CharacterSpawnAbilityPresenter>, 
        ICharacterSpawnAbilityView, ISelfValidator
    {
        [Required] [SerializeField] private UIButton _abilityButton;
        [SerializeField] private List<CharacterSpawnPoint> _charactersMeleeSpawnPoints;
        [SerializeField] private List<CharacterSpawnPoint> _charactersRangedSpawnPoints;

        public UIButton SpawnButton => _abilityButton;
        public IReadOnlyList<ICharacterSpawnPoint> MeleeSpawnPoints => _charactersMeleeSpawnPoints;
        public IReadOnlyList<ICharacterSpawnPoint> RangeSpawnPoints => _charactersRangedSpawnPoints;
        
        public void Validate(SelfValidationResult result)
        {
            _charactersMeleeSpawnPoints.ValidateSpawnPoints(SpawnPointType.CharacterMelee,  result);
            _charactersRangedSpawnPoints.ValidateSpawnPoints(SpawnPointType.CharacterRanged,  result);
        }
        
        [Button]
        private void AddCharacterMeleeSpawnPoints() =>
            _charactersMeleeSpawnPoints = this.GetSpawnPoints(SpawnPointType.CharacterMelee);
        
        [Button]
        private void AddCharacterRangeSpawnPoints() =>
            _charactersRangedSpawnPoints = this.GetSpawnPoints(SpawnPointType.CharacterRanged);
    }
}