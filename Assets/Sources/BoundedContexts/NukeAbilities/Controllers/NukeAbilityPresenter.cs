using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;
using Sources.Frameworks.Domain.Implementation.Constants;
using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.GameServices.Cameras.Infrastructure.Services.Interfaces;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.Frameworks.MyAudio_master.MyAudio.Soundy.Sources.Soundies.Infrastructure.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.NukeAbilities.Controllers
{
    public class NukeAbilityPresenter : PresenterBase
    {
        private readonly NukeAbility _nukeAbility;
        private readonly INukeAbilityView _nukeAbilityView;
        private readonly IOverlapService _overlapService;
        private readonly ISoundyService _soundyService;
        private readonly ICameraService _cameraService;

        private CancellationTokenSource _cancellationTokenSource;

        public NukeAbilityPresenter(
            IEntityRepository entityRepository, 
            INukeAbilityView nukeAbilityView,
            IOverlapService overlapService,
            ISoundyService soundyService,
            ICameraService cameraService)
        {
            if (entityRepository == null) 
                throw new ArgumentNullException(nameof(entityRepository));
            
            _nukeAbility = entityRepository.Get<NukeAbility>(ModelId.NukeAbility);
            _nukeAbilityView = nukeAbilityView ?? throw new ArgumentNullException(nameof(nukeAbilityView));
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _soundyService = soundyService ?? throw new ArgumentNullException(nameof(soundyService));
            _cameraService = cameraService ?? throw new ArgumentNullException(nameof(cameraService));
        }

        public override void Enable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _nukeAbility.AbilityApplied += ApplyAbility;
        }

        public override void Disable()
        {
            _nukeAbility.AbilityApplied -= ApplyAbility;
            _cancellationTokenSource.Cancel();
        }
        
        public void DealDamage(IEnemyHealthView enemyHealthView)
        {
            enemyHealthView.TakeDamage(20);
        }

        private async void ApplyAbility()
        {
            if(_nukeAbility.IsAvailable == false)
                return;
            
            try
            {
                _nukeAbilityView.BombView.SetPosition(_nukeAbilityView.BombView.FromPosition);
                _nukeAbilityView.BombView.Show();
                _soundyService.Play("Sounds", "Nuke");
                
                while (Vector3.Distance(_nukeAbilityView.BombView.Position, _nukeAbilityView.BombView.ToPosition) >
                       MathConst.Epsilon)
                {
                    _nukeAbilityView.BombView.Move();
                    await UniTask.Yield(_cancellationTokenSource.Token);
                }
                
                _cameraService.SetOnTimeCamera(CameraId.Explosion, 2f);
                _nukeAbilityView.BombView.Hide();
                _nukeAbilityView.PlayNukeParticle();
                DealDamage();
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void DealDamage()
        {
            IReadOnlyList<EnemyHealthView> enemies = _overlapService.OverlapBox<EnemyHealthView>(
                _nukeAbilityView.BombView.ToPosition, 
                _nukeAbilityView.DamageSize,
                LayerConst.Enemy);
            
            if(enemies.Count == 0)
                return;
            
            foreach (EnemyHealthView enemy in enemies)
                enemy.TakeDamage(_nukeAbility.Damage);
        }
    }
}