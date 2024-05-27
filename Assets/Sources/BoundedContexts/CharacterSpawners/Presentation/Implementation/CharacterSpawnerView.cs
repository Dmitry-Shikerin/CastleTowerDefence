using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterSpawners.Controllers;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Extensions;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Types;
using Sources.BoundedContexts.SpawnPoints.PresentationInterfaces;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterSpawners.Presentation.Implementation
{
    public class CharacterSpawnerView : PresentableView<CharacterSpawnerPresenter>, 
        ICharacterSpawnerView, ISelfValidator
    {
        
        [SerializeField] private List<SpawnPoint> _charactersMeleeSpawnPoints;
        [SerializeField] private List<SpawnPoint> _charactersRangedSpawnPoints;
        
        public IReadOnlyList<ISpawnPoint> MeleeSpawnPoints => _charactersMeleeSpawnPoints;
        public IReadOnlyList<ISpawnPoint> RangeSpawnPoints => _charactersRangedSpawnPoints;
        
        public void Validate(SelfValidationResult result)
        {
            _charactersMeleeSpawnPoints.ValidateSpawnPoints(SpawnPointType.CharacterMelee,  result);
            _charactersRangedSpawnPoints.ValidateSpawnPoints(SpawnPointType.CharacterRanged,  result);
        }
        
        [Button]
        private void AddCharacterMeleeSpawnPoints() =>
            _charactersMeleeSpawnPoints = gameObject.GetSpawnPoints(SpawnPointType.CharacterMelee);
        
        [Button]
        private void AddCharacterRangeSpawnPoints() =>
            _charactersRangedSpawnPoints = gameObject.GetSpawnPoints(SpawnPointType.CharacterRanged);
    }
}