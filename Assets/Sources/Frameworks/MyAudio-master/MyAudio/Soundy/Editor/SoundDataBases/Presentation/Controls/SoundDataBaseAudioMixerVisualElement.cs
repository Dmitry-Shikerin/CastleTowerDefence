using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Editor.UIElements;
using Doozy.Runtime.UIElements.Extensions;
using UnityEditor.UIElements;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundDataBases.Presentation.Controls
{
    public class SoundDataBaseAudioMixerVisualElement : VisualElement
    {
        public VisualElement Container { get; }
        public ObjectField ObjectField { get; }
        
        public SoundDataBaseAudioMixerVisualElement()
        {
            Container = DesignUtils.row
                .ResetLayout()
                .SetStyleBackgroundColor(EditorColors.Default.Background)
                .SetStylePadding(4,4,4,4)
                .SetStyleBorderRadius(4,4,4,4);
            
            Image image = new Image();
            image
                .SetStyleBackgroundImage(EditorTextures.EditorUI.Icons.Filter)
                .SetStyleWidth(20)
                .SetStyleHeight(20);
            ObjectField = new ObjectField();
            ObjectField
                .SetStyleFlexGrow(1)
                .SetStyleMinHeight(20);
            ObjectField
                .SetObjectType(typeof(AudioMixerGroup))
                .SetValueWithoutNotify(null);

            Container
                .AddSpace(4)
                .AddChild(image)
                .AddSpace(8)
                .AddChild(ObjectField);
            
            Add(Container);
        }
    }
}