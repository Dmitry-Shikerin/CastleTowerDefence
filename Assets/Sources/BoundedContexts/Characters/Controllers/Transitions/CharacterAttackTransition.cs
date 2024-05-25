using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Characters.PresentationInterfaces;
using Sources.BoundedContexts.Enemies.Extensions;

namespace Sources.BoundedContexts.Characters.Controllers.Transitions
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterAttackTransition : ConditionTask
    {
        private ICharacterView _view;

        protected override string OnInit()
        {
            CharacterDependencyProvider provider = 
                blackboard.GetDependencyProvider<CharacterDependencyProvider>();
            _view = provider.View;
            return null;
        }

        protected override bool OnCheck() =>
            _view.EnemyHealthView != null;
    }
}