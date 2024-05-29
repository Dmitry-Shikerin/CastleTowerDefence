using System;
using System.Collections.Generic;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.Upgrades.Domain;
using Sources.Frameworks.Services.ObjectPools.Generic;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Implementation
{
    public class CharacterMeleeSpawnService : ICharacterMeleeSpawnService
    {
        private readonly IObjectPool<CharacterMeleeView> _enemyPool;
        private readonly ICharacterMeleeViewFactory _characterViewFactory;

        public CharacterMeleeSpawnService(
            IObjectPool<CharacterMeleeView> characterPool, 
            ICharacterMeleeViewFactory characterViewFactory)
        {
            _enemyPool = characterPool ?? throw new ArgumentNullException(nameof(characterPool));
            _characterViewFactory = characterViewFactory ?? throw new ArgumentNullException(nameof(characterViewFactory));
        }

        public ICharacterMeleeView Spawn(Vector3 position)
        {
            CharacterMelee characterMelee = new CharacterMelee(
                new CharacterHealth.Domain.CharacterHealth(
                    new Upgrade(
                        0,
                        0,
                        0, 
                        new List<int>( ), "CharacterHealthUpgrade"))
            );
            
            ICharacterMeleeView characterMeleeView = SpawnFromPool(characterMelee) ?? 
                                            _characterViewFactory.Create(characterMelee);
            characterMeleeView.SetPosition(position);
            
            return characterMeleeView;
        }

        private ICharacterMeleeView SpawnFromPool(CharacterMelee characterMelee)
        {
            CharacterMeleeView characterMeleeView = _enemyPool.Get<CharacterMeleeView>();

            if (characterMeleeView == null)
                return null;
            
            return _characterViewFactory.Create(characterMelee, characterMeleeView);
        }
    }
}