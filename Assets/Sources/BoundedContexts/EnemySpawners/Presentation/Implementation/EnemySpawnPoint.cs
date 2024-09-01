using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Implementation;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Implementation
{
    public class EnemySpawnPoint : SpawnPoint, IEnemySpawnPoint
    {
        [Required] [SerializeField] private CharacterSpawnPoint _characterMeleeSpawnPoint;
        [Required] [SerializeField] private CharacterSpawnPoint _characterRangedSpawnPoint;
        
        public ICharacterSpawnPoint CharacterMeleeSpawnPoint => _characterMeleeSpawnPoint;
        public ICharacterSpawnPoint CharacterRangedSpawnPoint => _characterRangedSpawnPoint;
    }
}