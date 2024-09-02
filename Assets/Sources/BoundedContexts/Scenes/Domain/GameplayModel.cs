using System.Collections.Generic;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Tutorials.Domain.Models;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;

namespace Sources.BoundedContexts.Scenes.Domain
{
    public class GameplayModel
    {
        public GameplayModel(
            Upgrade characterHealthUpgrade,
            Upgrade characterAttackUpgrade,
            Upgrade nukeAbilityUpgrade,
            Upgrade flamethrowerAbilityUpgrade,
            Bunker bunker,
            EnemySpawner enemySpawner,
            CharacterSpawnAbility characterSpawnAbility,
            NukeAbility nukeAbility,
            FlamethrowerAbility flamethrowerAbility,
            KillEnemyCounter killEnemyCounter,
            PlayerWallet playerWallet,
            Volume musicVolume,
            Volume soundsVolume,
            IEnumerable<Achievement> achievements,
            HealthBooster healthBooster,
            Tutorial tutorial)
        {
            CharacterHealthUpgrade = characterHealthUpgrade;
            CharacterAttackUpgrade = characterAttackUpgrade;
            NukeAbilityUpgrade = nukeAbilityUpgrade;
            FlamethrowerAbilityUpgrade = flamethrowerAbilityUpgrade;
            Bunker = bunker;
            EnemySpawner = enemySpawner;
            CharacterSpawnAbility = characterSpawnAbility;
            NukeAbility = nukeAbility;
            FlamethrowerAbility = flamethrowerAbility;
            KillEnemyCounter = killEnemyCounter;
            PlayerWallet = playerWallet;
            MusicVolume = musicVolume;
            SoundsVolume = soundsVolume;
            Achievements = achievements;
            HealthBooster = healthBooster;
            Tutorial = tutorial;
        }

        public Upgrade CharacterHealthUpgrade { get; }
        public Upgrade CharacterAttackUpgrade { get; }
        public Upgrade NukeAbilityUpgrade { get; }
        public Upgrade FlamethrowerAbilityUpgrade { get; }
        public Bunker Bunker { get; }
        public EnemySpawner EnemySpawner { get; }
        public CharacterSpawnAbility CharacterSpawnAbility { get; }
        public NukeAbility NukeAbility { get; }
        public FlamethrowerAbility FlamethrowerAbility { get; }
        public KillEnemyCounter KillEnemyCounter { get; }
        public PlayerWallet PlayerWallet { get; }
        public Volume MusicVolume { get; }
        public Volume SoundsVolume { get; }
        public IEnumerable<Achievement> Achievements { get; }
        public HealthBooster HealthBooster { get; }
        public Tutorial Tutorial { get; }
    }
}