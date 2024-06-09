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
        private IEnemyKamikazeView _view;

        protected override string OnInit()
        {
            EnemyKamikazeDependencyProvider provider =
                blackboard.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;
            _view = provider.View;
            
            return null;
        }

        protected override bool OnCheck() =>
            Vector3.Distance(_view.Position, _view.CharacterMeleePoint.Position)
            <= _view.StoppingDistance;

    }
}