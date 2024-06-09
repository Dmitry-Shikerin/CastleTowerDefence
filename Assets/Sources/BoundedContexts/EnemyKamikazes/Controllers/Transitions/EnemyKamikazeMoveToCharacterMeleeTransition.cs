using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyKamikazeMoveToCharacterMeleeTransition : ConditionTask
    {
        private IEnemyKamikazeView _view;
        private EnemyKamikaze _enemy;

        protected override string OnInit()
        {
            EnemyKamikazeDependencyProvider provider =
                blackboard.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;
            _enemy = provider.EnemyKamikaze;
            _view = provider.View;
            
            return null;
        }

        protected override bool OnCheck() =>
            _enemy.IsInitialized;
    }
}