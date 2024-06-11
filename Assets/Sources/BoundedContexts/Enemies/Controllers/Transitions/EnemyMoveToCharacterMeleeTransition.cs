using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyMoveToCharacterMeleeTransition : ConditionTask
    {
        [RequiredField] public BBParameter<EnemyDependencyProvider> Provider;
        
        private Enemy Enemy => Provider.value.Enemy;
        
        protected override bool OnCheck() =>
            Enemy.IsInitialized;
    }
}