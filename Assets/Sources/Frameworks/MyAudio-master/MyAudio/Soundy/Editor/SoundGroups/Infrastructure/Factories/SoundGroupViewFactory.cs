using Doozy.Engine.Soundy;
using MyAudios.Soundy.Editor.SoundGroups.Controllers;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Implementation;
using MyAudios.Soundy.Editor.SoundGroups.Presentation.Views.Interfaces;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;

namespace MyAudios.Soundy.Editor.SoundGroups.Infrastructure.Factories
{
    public class SoundGroupViewFactory
    {
        public ISoundGroupView Create(SoundGroupData soundGroup, SoundDatabase soundDatabase)
        {
            var view = new SoundGroupView();
            var presenter = new SoundGroupPresenter(soundGroup, soundDatabase, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}