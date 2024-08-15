using Sources.Frameworks.MyGameCreator.Achivements.Domain.Configs;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Commands.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Services.Implementation;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Services.Interfaces;
using Zenject;

namespace Sources.App.DIContainers.Common
{
    public class AchievementInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AchievementConfigCollector>()
                .FromResource("Configs/Achievements/AchievementConfigCollector")
                .AsSingle();
            Container.Bind<IAchievementService>().To<AchievementService>().AsSingle();
            
            //Commands
            Container.Bind<FirstKillEnemyAchievementCommand>().AsSingle();
            Container.Bind<FirstUpgradeAchievementCommand>().AsSingle();
            Container.Bind<FirstHealthBoosterUsageAchievementCommand>().AsSingle();
            Container.Bind<FirstWaveCompletedAchievement>().AsSingle();
        }
    }
}