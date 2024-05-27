using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.PresentationsInterfaces.Views;

namespace Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces
{
    public interface ICharacterRangeView : IView
    {
        public IEnemyHealthView EnemyHealthView { get; }

        public void SetEnemyHealth(IEnemyHealthView enemyHealthView);
    }
}