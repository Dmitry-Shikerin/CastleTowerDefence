using Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Commands.Implementation;
using Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Services.Implementation;
using Sources.Frameworks.MyGameCreator.Achievements.Infrastructure.Services.Interfaces;
using Zenject;

namespace Sources.App.DIContainers.Common
{
    public class AchievementInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAchievementService>().To<AchievementService>().AsSingle();
            
            //Commands
            Container.Bind<FirstKillEnemyAchievementCommand>().AsSingle();
            Container.Bind<FirstUpgradeAchievementCommand>().AsSingle();
            Container.Bind<FirstHealthBoosterUsageAchievementCommand>().AsSingle();
            Container.Bind<FirstWaveCompletedAchievementCommand>().AsSingle();
            Container.Bind<ScullsDiggerAchievementCommand>().AsSingle();
            Container.Bind<MaxUpgradeAchievementCommand>().AsSingle();
            Container.Bind<FiftyWaveCompletedAchievementCommand>().AsSingle();
            Container.Bind<AllAbilitiesUsedAchievementCommand>().AsSingle();
            Container.Bind<CompleteGameWithOneHealthAchievementCommand>().AsSingle();
        }
    }
}