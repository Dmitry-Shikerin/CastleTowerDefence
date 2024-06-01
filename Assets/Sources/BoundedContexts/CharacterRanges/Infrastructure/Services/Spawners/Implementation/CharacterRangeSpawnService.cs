﻿using System;
using System.Collections.Generic;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.Upgrades.Domain;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Implementation
{
    public class CharacterRangeSpawnService : ICharacterRangeSpawnService
    {
        private readonly IObjectPool<CharacterRangeView> _enemyPool;
        private readonly ICharacterRangeViewFactory _characterViewFactory;

        public CharacterRangeSpawnService(
            IObjectPool<CharacterRangeView> characterPool, 
            ICharacterRangeViewFactory characterViewFactory)
        {
            _enemyPool = characterPool ?? throw new ArgumentNullException(nameof(characterPool));
            _characterViewFactory = characterViewFactory ?? throw new ArgumentNullException(nameof(characterViewFactory));
        }

        public ICharacterRangeView Spawn(Vector3 position)
        {
            CharacterRange characterMelee = new CharacterRange(
                new CharacterHealth.Domain.CharacterHealth(
                    new Upgrade(
                        0,
                        0,
                        0, 
                        new List<int>( ), "CharacterHealthUpgrade"))
            );
            
            ICharacterRangeView characterView = SpawnFromPool(characterMelee) ?? 
                                                     _characterViewFactory.Create(characterMelee);
            characterView.SetPosition(position);
            
            return characterView;
        }

        private ICharacterRangeView SpawnFromPool(CharacterRange character)
        {
            CharacterRangeView characterView = _enemyPool.Get<CharacterRangeView>();

            if (characterView == null)
                return null;
            
            return _characterViewFactory.Create(character, characterView);
        }
    }
}