﻿using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.GameServices.Volumes.Presentations;

namespace Sources.Frameworks.GameServices.Volumes.Infrastucture.Factories
{
    public class SoundsChangerViewFactory
    {
        public SoundsChangerView Create(Volume volume, SoundsChangerView view)
        {
            view.Construct(volume);

            return view;
        }
    }
}