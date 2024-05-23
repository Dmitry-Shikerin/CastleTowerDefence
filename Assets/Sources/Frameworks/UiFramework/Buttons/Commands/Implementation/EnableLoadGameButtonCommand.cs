using System;
using Sources.Domain.Models.Data.Ids;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons;
using Sources.Frameworks.UiFramework.Presentation.Buttons;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;
using Sources.InfrastructureInterfaces.Services.LoadServices;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons
{
    public class EnableLoadGameButtonCommand :  IButtonCommand
    {
        private readonly ILoadService _loadService;

        public EnableLoadGameButtonCommand(
            ILoadService loadService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public ButtonCommandId Id => ButtonCommandId.EnableLoadGameButton;
        
        public void Handle(IUiButton uiButton)
        {
            uiButton.Show();
            
            if (_loadService.HasKey(ModelId.PlayerWallet) == false)
                uiButton.Hide();
            
            // Debug.Log($"Enable load game, player wallet: {_loadService.HasKey(ModelId.PlayerWallet)}");
        }
    }
}