using System.Collections.Generic;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.GameServices.Cameras.Infrastructure.Services.Interfaces;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyKamikazeBomberState : FSMState
    {
        private Enemy _enemy;
        private IEnemyKamikazeView _view;
        private ICameraService _cameraService;
        private IOverlapService _overlapService;
        private ExplosionBodyViewFactory _explosionBodyViewFactory;

        [Construct]
        private void Construct(Enemy enemyKamikaze, IEnemyKamikazeView view)
        {
            _enemy = enemyKamikaze;
            _view = view;
        }

        [Inject]
        private void Construct(
            ICameraService cameraService, 
            IOverlapService overlapService, 
            ExplosionBodyViewFactory explosionBodyViewFactory)
        {
            _cameraService = cameraService;
            _overlapService = overlapService;
            _explosionBodyViewFactory = explosionBodyViewFactory;
        }

        protected override void OnEnter()
        {
            _cameraService.SetOnTimeCamera(CameraId.Explosion, 1.5f);
            Vector3 spawnPosition = _view.Position + Vector3.up;
            _explosionBodyViewFactory.Create(spawnPosition);
            Explode();
            _view.Destroy();
        }

        private void Explode()
        {
            IReadOnlyList<CharacterHealthView> characterHealthViews =
                _overlapService.OverlapSphere<CharacterHealthView>(
                    _view.Position, _view.FindRange,
                    LayerConst.Character,
                    LayerConst.Defaul);

            if (characterHealthViews.Count <= 0)
                return;

            foreach (CharacterHealthView characterHealthView in characterHealthViews)
                characterHealthView.TakeDamage(_enemy.EnemyAttacker.MassAttackDamage);
        }
    }
}