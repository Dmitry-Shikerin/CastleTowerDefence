using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.BurnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.Soundies.Infrastructure.Interfaces;
using Sources.Frameworks.UniTaskTweens;
using Sources.Frameworks.UniTaskTweens.Sequences;
using UnityEngine;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Controllers
{
    public class FlamethrowerAbilityPresenter : PresenterBase
    {
        private readonly FlamethrowerAbility _flamethrowerAbility;
        private readonly ISoundyService _soundyService;
        private readonly IFlamethrowerAbilityView _view;
        
        private UTSequence _sequence;

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

        public override void Initialize()
        {
            IFlamethrowerView view = _view.FlamethrowerView;
            Vector3 from = view.FromPosition;
            Vector3 to = view.ToPosition;

            _sequence = UTTween
                .Sequence()
                .Add(() => view.SetPosition(from))
                .Add(() => view.Show())
                .Add(_view.PlayParticle)
                .Add(() => _soundyService.Play("Sounds", "Flamethrower"))
                .Add(token => MoveAsync(to, token))
                .Add(token => MoveAsync(from, token))
                .Add(_view.StopParticle)
                .Add(() => _soundyService.Stop("Sounds", "Flamethrower"));
        }

        public override void Enable()
        {
            _flamethrowerAbility.AbilityApplied += ApplyAbility;
        }

        public override void Disable()
        {
            _sequence.Stop();
            _flamethrowerAbility.AbilityApplied -= ApplyAbility;
        }

        private void ApplyAbility()
        {
            _sequence.StartAsync();
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