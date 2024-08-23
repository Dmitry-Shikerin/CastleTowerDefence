using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterRangeIdleState : FSMState
    {
        private CharacterRange _characterRange;
        private ICharacterRangeView _view;
        private ICharacterRangeAnimation _animation;
        private IOverlapService _overlapService;
        
        private CancellationTokenSource _cancellationTokenSource;

        [Construct]
        private void Construct(CharacterRange characterRange, CharacterRangeView view)
        {
            _characterRange = characterRange ?? throw new ArgumentNullException(nameof(characterRange));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _animation = view.RangeAnimation ?? throw new ArgumentNullException(nameof(view.RangeAnimation));
        }

        [Inject]
        private void Construct(IOverlapService overlapService)
        {
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
        }
        
        protected override void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _animation.PlayIdle();
            StartFind(_cancellationTokenSource.Token);
        }

        protected override void OnUpdate()
        {
            _view.SetLookRotation(0);
        }

        protected override void OnExit()
        {
            _cancellationTokenSource.Cancel();
        }
        
        private async void StartFind(CancellationToken cancellationToken)
        {
            try
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellationToken);
                    FindTarget();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        private void FindTarget()
        {
            IEnemyHealthView enemyHealthView = 
                _overlapService.OverlapSphere<EnemyHealthView>(
                        _view.Position, _view.FindRange, 
                        LayerConst.Enemy, 
                        LayerConst.Defaul)
                    .FirstOrDefault();

            if(enemyHealthView?.CurrentHealth <= 0)
                return;
            
            if (enemyHealthView != null)
                _view.SetEnemyHealth(enemyHealthView);
        }
    }
}