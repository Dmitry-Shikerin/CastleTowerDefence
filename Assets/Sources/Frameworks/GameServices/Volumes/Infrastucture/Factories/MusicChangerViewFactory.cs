using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.GameServices.Volumes.Presentations;

namespace Sources.Frameworks.GameServices.Volumes.Infrastucture.Factories
{
    public class MusicChangerViewFactory
    {
        public MusicChangerView Create(Volume volume, MusicChangerView view)
        {
            view.Construct(volume);

            return view;
        }
    }
}