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
        private EnemyKamikazeDependencyProvider _provider;
        
        private EnemyKamikaze Enemy => _provider.EnemyKamikaze;

        protected override string OnInit()
        {
            _provider =
                blackboard.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;
            
            return null;
        }

        protected override bool OnCheck() =>
            Enemy.IsInitialized;
    }
}