using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterSpawners.Controllers;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Extensions;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation.Types;
using Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterSpawners.Presentation.Implementation
{
    public class CharacterSpawnerView : PresentableView<CharacterSpawnerPresenter>, 
        ICharacterSpawnerView, ISelfValidator
    {
        
        [SerializeField] private List<CharacterSpawnPoint> _charactersMeleeSpawnPoints;
        [SerializeField] private List<CharacterSpawnPoint> _charactersRangedSpawnPoints;
        
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