using System;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons;
using Sources.Frameworks.UiFramework.Presentation.Buttons;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Forms;

namespace Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons
{
    public class ShowFormCommand : IButtonCommand
    {
        private readonly IFormService _formService;

        public ShowFormCommand(IFormService formService)
        {
            _formService = formService ?? throw new ArgumentNullException(nameof(formService));
        }

        public ButtonCommandId Id => ButtonCommandId.ShowForm;

        public void Handle(IUiButton uiButton)
        {
            _formService.Show(uiButton.FormId);
        }
    }
}