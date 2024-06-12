using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyMoveToBunkerTransition : ConditionTask
    {
        [RequiredField] public BBParameter<EnemyDependencyProvider> Provider;
        
        private IEnemyView View => Provider.value.View;
        
        protected override bool OnCheck() =>
            Vector3.Distance(View.Position, View.CharacterMeleePoint.Position)
            <= View.StoppingDistance && View.CharacterHealthView == null;
    }
}