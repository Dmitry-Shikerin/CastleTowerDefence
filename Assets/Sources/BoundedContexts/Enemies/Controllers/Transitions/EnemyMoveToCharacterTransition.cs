using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyMoveToCharacterTransition : ConditionTask
    {        
        [RequiredField] public BBParameter<EnemyDependencyProvider> Provider;
        
        private IEnemyView View => Provider.value.View;
        
        protected override bool OnCheck() =>
            View.CharacterHealthView != null && View.CharacterHealthView.CurrentHealth > 0;
    }
}