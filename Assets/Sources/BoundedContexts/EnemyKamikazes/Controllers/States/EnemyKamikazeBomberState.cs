using System.Collections.Generic;
using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyKamikazeBomberState : FSMState
    {
        private EnemyKamikaze _enemy;
        private IEnemyKamikazeView _view;
        private IEnemyAnimation _animation;
        private IOverlapService _overlapService;
        private IExplosionBodySpawnService _explosionBodySpawnService;

        protected override void OnInit()
        {
            EnemyKamikazeDependencyProvider provider =
                graphBlackboard.parent.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;

            _enemy = provider.EnemyKamikaze;
            _view = provider.View;
            _animation = provider.Animation;
            _overlapService = provider.OverlapService;
            _explosionBodySpawnService = provider.ExplosionBodySpawnService;
        }

        protected override void OnEnter()
        {
            Vector3 spawnPosition = _view.Position + Vector3.up;
            _explosionBodySpawnService.Spawn(spawnPosition);

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
                characterHealthView.TakeDamage(_enemy.EnemyAttacker.Damage);
        }
    }
}