using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyKamikazeToDeathTransition : ConditionTask
    {
        private EnemyKamikazeDependencyProvider _provider;
        
        private IEnemyKamikazeView View => _provider.View;

        protected override string OnInit()
        {
            _provider =
                blackboard.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;

            return null;
        }

        protected override bool OnCheck() =>
            View.EnemyHealthView.CurrentHealth <= 0;
    }
}