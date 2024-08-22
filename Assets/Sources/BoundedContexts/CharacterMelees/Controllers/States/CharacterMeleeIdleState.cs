using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterMeleeIdleState : FSMState
    {
        private CharacterMeleeDependencyProvider _provider;
        
        private ICharacterMeleeView _view;
        private CharacterMelee _characterMelee;
        private ICharacterMeleeAnimation _animation;
        private IOverlapService _overlapService;

        private CancellationTokenSource _cancellationTokenSource;

        [Construct]
        private void Construct(CharacterMelee characterMelee, CharacterMeleeView characterMeleeView)
        {
            _characterMelee = characterMelee ?? throw new ArgumentNullException(nameof(characterMelee));
            _view = characterMeleeView ?? throw new ArgumentNullException(nameof(characterMeleeView));
            _animation = characterMeleeView.MeleeAnimation;
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
            
            if (enemyHealthView == null)
                return;

            _view.SetEnemyHealth(enemyHealthView);
        }
    }
}