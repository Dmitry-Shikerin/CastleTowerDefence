﻿using Sirenix.OdinInspector;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Implementation
{
    public class EnemySpawnerUi : PresentableView<EnemySpawnerUiPresenter>, IEnemySpawnerUi
    {
        [Required] [SerializeField] private TextView _currentWaveText;

        public ITextView CurrentWaveText => _currentWaveText;
    }
}