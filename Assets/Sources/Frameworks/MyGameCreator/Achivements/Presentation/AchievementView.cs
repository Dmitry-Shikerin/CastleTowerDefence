using System;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Images;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MyGameCreator.Achivements.Domain;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Configs;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.MyGameCreator.Achivements.Infrastructure.Services.Interfaces;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Localizations;
using UnityEngine;
using Zenject;

namespace Sources.Frameworks.MyGameCreator.Achivements.Presentation
{
    public class AchievementView : View
    { 
        [SerializeField] private ImageView _iconImage;
        [SerializeField] private TextView _titleTextView;
        [SerializeField] private TextView _discriptionTextView;
        [SerializeField] private ImageView _uncompletedImage;

        private Achievement _achievement;
        private IAchievementService _achievementService;
        private ILocalizationService _localizationService;

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

        public void Construct(Achievement achievement)
        {
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
            
            string achievementId = _achievement.Id;
            AchievementConfig config = _achievementService.GetConfig(achievementId);
            Sprite sprite = config.Sprite;
            _iconImage.SetSprite(sprite);
            string title = _localizationService.GetText(config.TitleId);
            _titleTextView.SetText(title);
            string description = _localizationService.GetText(config.DescriptionId);
            _discriptionTextView.SetText(description);
        }

        [Inject]
        private void OnBeforeConstruct(IAchievementService achievementService, ILocalizationService localizationService)
        {
            _achievementService = achievementService ??
                                  throw new ArgumentNullException(nameof(achievementService));
            _localizationService = localizationService ??
                                   throw new ArgumentNullException(nameof(localizationService));
        }
    }
}