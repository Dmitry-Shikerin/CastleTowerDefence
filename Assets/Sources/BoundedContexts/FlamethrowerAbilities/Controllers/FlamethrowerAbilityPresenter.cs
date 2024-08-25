using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sources.BoundedContexts.BurnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Implementation;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.Soundies.Infrastructure.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Controllers
{
    public class FlamethrowerAbilityPresenter : PresenterBase
    {
        private readonly FlamethrowerAbility _flamethrowerAbility;
        private readonly ISoundyService _soundyService;
        private readonly IFlamethrowerAbilityView _view;

        private CancellationTokenSource _cancellationTokenSource;

        public FlamethrowerAbilityPresenter(
            IEntityRepository entityRepository,
            ISoundyService soundyService,
            IFlamethrowerAbilityView view)
        {
            if (entityRepository == null) 
                throw new ArgumentNullException(nameof(entityRepository));

            _flamethrowerAbility = entityRepository.Get<FlamethrowerAbility>(ModelId.FlamethrowerAbility);
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _flamethrowerAbility.AbilityApplied += ApplyAbility;
        }

        public override void Disable()
        {
            _cancellationTokenSource.Cancel();
            _flamethrowerAbility.AbilityApplied -= ApplyAbility;
        }

        private async void ApplyAbility()
        {
            try
            {
                IFlamethrowerView view = _view.FlamethrowerView;
                Vector3 from = view.FromPosition;
                Vector3 to = view.ToPosition;
                CancellationToken token = _cancellationTokenSource.Token;
                
                view.SetPosition(from);
                view.Show();
                _view.PlayParticle();
                _soundyService.Play("Sounds", "Flamethrower");
                await MoveAsync(to, token);
                await MoveAsync(from, token);
                _view.StopParticle();
                _soundyService.Stop("Sounds", "Flamethrower");
                
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async UniTask MoveAsync(Vector3 target, CancellationToken cancellationToken)
        {
            while (Vector3.Distance(_view.FlamethrowerView.Position, target) > 0.001f)
            {
                _view.FlamethrowerView.Move(target);

                await UniTask.Yield(cancellationToken);
            }
        }

        public void DealDamage(IBurnable burnable)
        {
            int instantDamage = 5;
            int overtimeDamage = 1;
            burnable.Burn(instantDamage, overtimeDamage);
        }
    }
}