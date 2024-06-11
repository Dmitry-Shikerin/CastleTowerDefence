using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyMoveToCharacterMeleePointState : FSMState
    {
        [RequiredField] public BBParameter<EnemyDependencyProvider> _provider;

        private CancellationTokenSource _cancellationTokenSource;
        
        private IEnemyView View => _provider.value.View;
        private IEnemyAnimation Animation => _provider.value.Animation;
        private IOverlapService OverlapService => _provider.value.OverlapService;

        protected override void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Animation.PlayWalk();
            StartFind(_cancellationTokenSource.Token);
        }

        protected override void OnUpdate() =>
            View.Move(View.CharacterMeleePoint.Position);

        protected override void OnExit() =>
            _cancellationTokenSource.Cancel();

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
            var characterHealthView =
                OverlapService.OverlapSphere<CharacterHealthView>(
                        View.Position, View.FindRange,
                        LayerConst.Character,
                        LayerConst.Defaul)
                    .FirstOrDefault();
            
            if (characterHealthView == null)
                return;

            View.SetCharacterHealth(characterHealthView);
        }
    }
}