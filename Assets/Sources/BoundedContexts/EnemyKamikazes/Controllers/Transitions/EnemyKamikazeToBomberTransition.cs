using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyKamikazeToBomberTransition : ConditionTask
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
            Vector3.Distance(View.Position, View.CharacterMeleePoint.Position)
            <= View.StoppingDistance;

    }
}