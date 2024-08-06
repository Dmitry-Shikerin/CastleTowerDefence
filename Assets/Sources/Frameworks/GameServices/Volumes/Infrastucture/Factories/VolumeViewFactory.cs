using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.GameServices.Volumes.Presentations;

namespace Sources.Frameworks.GameServices.Volumes.Infrastucture.Factories
{
    public class VolumeViewFactory
    {
        public VolumeView Create(Volume volume, VolumeView view)
        {
            view.Construct(volume);

            return view;
        }
    }
}