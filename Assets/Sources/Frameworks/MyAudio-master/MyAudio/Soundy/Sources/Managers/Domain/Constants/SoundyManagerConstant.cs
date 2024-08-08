namespace MyAudios.Soundy.Sources.Managers.Domain.Constants
{
    public class SoundyManagerConstant
    {
        private const int SoundyMenuOrder = 13;
        private const string _soundyAddComponentMenuPath = "Doozy/Soundy/";
        private const string _soundyMenuItemPath = "GameObject/Doozy/Soundy/";
        
        public const string SoundyManagerGameObjectName = "Soundy Manager";
        public const string SoundyManagerAddComponentMenuMenuName = _soundyAddComponentMenuPath + SoundyManagerGameObjectName;
        public const int SoundyManagerAddComponentMenuOrder = SoundyMenuOrder;
        public const string SoundyManagerMenuItemItemName = _soundyMenuItemPath + SoundyManagerGameObjectName;
        public const int SoundyManagerMenuItemPriority = SoundyMenuOrder;
        
        public const string General = "General";
        public const string Database = "Database";
        public const string NewSoundGroup = "New Sound Group";
        public const string NoSound = "No Sound";
        public const string Sounds = "Sounds";
        public const string Soundy = "Soundy";
    }
}