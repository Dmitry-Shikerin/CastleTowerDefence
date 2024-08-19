using System.Collections.Generic;
using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.InfrastructureInterfaces.Services.Cameras;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyKamikazeBomberState : FSMState
    {
        private EnemyKamikazeDependencyProvider _provider;
        private ICameraService _cameraService;

        private EnemyKamikaze Enemy => _provider.EnemyKamikaze;
        private IEnemyKamikazeView View => _provider.View;
        private IOverlapService OverlapService => _provider.OverlapService;
        private ExplosionBodyViewFactory ExplosionBodyViewFactory => 
            _provider.ExplosionBodyViewFactory;

        protected override void OnInit()
        {
            _provider =
                graphBlackboard.parent.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;
            _cameraService = _provider.CameraService;
        }

        protected override void OnEnter()
        {
            _cameraService.SetOnTimeCamera(CameraId.Explosion, 1.5f);
            Vector3 spawnPosition = View.Position + Vector3.up;
            ExplosionBodyViewFactory.Create(spawnPosition);
            Explode();
            View.Destroy();
        }

        private void Explode()
        {
            IReadOnlyList<CharacterHealthView> characterHealthViews =
                OverlapService.OverlapSphere<CharacterHealthView>(
                    View.Position, View.FindRange,
                    LayerConst.Character,
                    LayerConst.Defaul);

            if (characterHealthViews.Count <= 0)
                return;

            foreach (CharacterHealthView characterHealthView in characterHealthViews)
                characterHealthView.TakeDamage(Enemy.EnemyAttacker.MassAttackDamage);
        }
    }
}