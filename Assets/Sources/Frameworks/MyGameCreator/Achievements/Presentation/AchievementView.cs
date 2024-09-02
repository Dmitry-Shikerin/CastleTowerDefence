using System;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Images;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Configs;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Models;
using Sources.Frameworks.UiFramework.Core.Services.Localizations.Interfaces;
using UnityEngine;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achievements.Presentation
{
    public class AchievementView : View
    { 
        [SerializeField] private ImageView _iconImage;
        [SerializeField] private TextView _titleTextView;
        [SerializeField] private TextView _discriptionTextView;
        [SerializeField] private ImageView _uncompletedImage;

        private Achievement _achievement;
        private ILocalizationService _localizationService;
        private AchievementConfig _achievementConfig;

        private void OnEnable()
        {
            if (_achievement == null)
                return;
            
            UpdateView();
            OnCompleted();
            _achievement.Completed += OnCompleted;
        }

        private void OnCompleted()
        {
            if (_achievement.IsCompleted == false)
                return;
            
            _uncompletedImage.HideImage();
            
            //Todo сделать вьюшку цветной
        }

        private void OnDisable()
        {
            if (_achievement == null)
                return;
            
            _achievement.Completed -= OnCompleted;
        }

        public void Construct(Achievement achievement, AchievementConfig config)
        {
            _achievementConfig = config ?? throw new ArgumentNullException(nameof(config));
            _achievement = achievement ?? throw new ArgumentNullException(nameof(achievement));
            Subscribe();
            UpdateView();
            OnCompleted();
        }

        private void Subscribe()
        {
            _achievement.Completed -= OnCompleted;
            _achievement.Completed += OnCompleted;
        }

        private void UpdateView()
        {
            if (_achievement == null)
                return;
            
            Sprite sprite = _achievementConfig.Sprite;
            _iconImage.SetSprite(sprite);
            string title = _localizationService.GetText(_achievementConfig.TitleId);
            _titleTextView.SetText(title);
            string description = _localizationService.GetText(_achievementConfig.DescriptionId);
            _discriptionTextView.SetText(description);
        }

        [Inject]
        private void OnBeforeConstruct(ILocalizationService localizationService)
        {
            _localizationService = localizationService ??
                                   throw new ArgumentNullException(nameof(localizationService));
        }
    }
}