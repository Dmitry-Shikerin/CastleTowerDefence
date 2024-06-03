using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Implementation;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces;
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