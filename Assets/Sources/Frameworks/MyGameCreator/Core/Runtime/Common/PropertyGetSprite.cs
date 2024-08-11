using System;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Core.Runtime.Common
{
    [Serializable]
    public class PropertyGetSprite : TPropertyGet<PropertyTypeGetSprite, Sprite>
    {
        public PropertyGetSprite() : base(new GetSpriteInstance())
        { }

        public PropertyGetSprite(PropertyTypeGetSprite defaultType) : base(defaultType)
        { }

        public PropertyGetSprite(Sprite value) : base(new GetSpriteInstance(value))
        { }
    }
}