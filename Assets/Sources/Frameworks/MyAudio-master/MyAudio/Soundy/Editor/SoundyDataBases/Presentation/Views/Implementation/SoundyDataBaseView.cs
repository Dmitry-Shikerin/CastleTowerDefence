using System;
using System.Collections.Generic;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Components.Internal;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Controls;
using MyAudios.Soundy.Editor.SoundyDataBases.Controllers;
using MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Controls;
using MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Views.Interfaces;
using MyAudios.Soundy.Editor.SoundySetting.Presentation.Views.Interfaces;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Views.Implementation
{
    public class SoundyDataBaseView : ISoundyDataBaseView
    {
        private SoundyDataBasePresenter _presenter;
        private SoundyDataBaseWindowLayout _fluidWindowLayout;
        private List<FluidToggleButtonTab> _fluidToggleButtonTabs;
        private List<SoundGroupVisualElement> _soundGroups;
        private List<FluidToggleButtonTab> _databasesButtons;
        
        public IReadOnlyList<FluidToggleButtonTab> DatabaseButtons => _databasesButtons;
        public ISoundySettingsView SettingsView { get; private set; }
        public ISoundDataBaseView SoundDataBaseView { get; private set; }

        public VisualElement Root { get; private set; }

        public void Construct(SoundyDataBasePresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));

            CreateView();
            Initialize();
        }

        public void CreateView()
        {
            _fluidWindowLayout = new SoundyDataBaseWindowLayout();
            _fluidWindowLayout
                .sideMenu
                .SetMenuLevel(FluidSideMenu.MenuLevel.Level_2)
                .IsCollapsable(false);
            _soundGroups = new List<SoundGroupVisualElement>();
            _databasesButtons = new List<FluidToggleButtonTab>();
            Root = _fluidWindowLayout;
        }

        public void Initialize()
        {
            _fluidWindowLayout.SettingsButton.SetOnClick(() => _presenter.OpenSettings());
            _fluidWindowLayout.NewDataBaseButton.SetOnClick(() => _presenter.CreateNewDataBase());
            _fluidWindowLayout.RefreshButton.SetOnClick(() => _presenter.RefreshDataBases());
            _presenter.Initialize();
        }

        public void Dispose()
        {
            _presenter?.Dispose();
        }

        public void RefreshDataBasesButtons()
        {
            foreach (FluidToggleButtonTab button in _databasesButtons)
                button.Recycle();

            _databasesButtons.Clear();
        }

        public void RenameButtons()
        {
            _presenter.RenameButtons();
        }

        public void UpdateDataBase()
        {
            _presenter.UpdateDataBase();
        }

        public void ClearButtons()
        {
            foreach (FluidToggleButtonTab button in _databasesButtons)
            {
                _fluidWindowLayout.sideMenu.buttons.Remove(button);
                button.RemoveFromHierarchy();
            }
        }

        public void AddSettings(ISoundySettingsView soundySettingsView)
        {
            SettingsView?.Dispose();
            SoundDataBaseView?.Dispose();
            SettingsView = soundySettingsView ?? throw new ArgumentNullException(nameof(soundySettingsView));
            _fluidWindowLayout.content.AddChild(SettingsView.Root);
        }

        public void AddDataBaseButton(string name, UnityAction callback)
        {
            FluidToggleButtonTab button =
                _fluidWindowLayout.sideMenu
                    .AddButton(name, EditorSelectableColors.EditorUI.Orange)
                    .SetElementSize(ElementSize.Normal)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.ToggleON)
                    .AddOnClick(() => callback?.Invoke());
            
            _databasesButtons.Add(button);
        }

        public void SetSoundDataBase(ISoundDataBaseView dataBaseView)
        {
            SettingsView?.Dispose();
            SoundDataBaseView?.Dispose();
            SoundDataBaseView = dataBaseView;
            _fluidWindowLayout.content.AddChild(dataBaseView.Root);
        }
    }
}