namespace MyAudios.MyUiFramework.Utils
{
    /// <summary> Execution order for all the components inside DoozyUI </summary>
    public static class SoundyExecutionOrder
    {
        private const int Component = -100;
        private const int Manager = -200;
        
        public const int GameEventListener = Component;
        public const int GameEventManager = Manager;
        public const int SoundyController = Component;
        public const int SoundyManager = Manager;
        public const int SoundyPooler = Component;
    }
}