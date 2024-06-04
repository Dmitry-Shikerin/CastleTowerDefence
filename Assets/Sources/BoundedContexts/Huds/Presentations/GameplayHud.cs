using Sirenix.OdinInspector;
using Sources.BoundedContexts.Abilities.Presentation.Implementation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Huds;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.Huds.Presentations
{
    public class GameplayHud : MonoBehaviour, IHud
    {
        [FoldoutGroup("UiFramework")]
        [Required] [SerializeField] private UiCollector _uiCollector;
        
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private AbilityApplierView _nukeAbilityApplier;
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private AbilityApplierView _spawnAbilityApplier;

        public UiCollector UiCollector => _uiCollector;

        public AbilityApplierView NukeAbilityApplier => _nukeAbilityApplier;
        public AbilityApplierView SpawnAbilityApplier => _spawnAbilityApplier;
    }
}