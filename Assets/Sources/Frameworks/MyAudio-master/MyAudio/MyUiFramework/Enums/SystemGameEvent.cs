namespace MyAudios.MyUiFramework.Enums
{
    /// <summary> Contains DoozyUI special game events, also known as system events, that trigger predefined actions </summary>
    public enum SystemGameEvent
    {
        /// <summary> Activates all the Scenes that have been loaded by SceneLoaders and are ready to be activated </summary>
        ActivateLoadedScenes,

        /// <summary> Exits play mode (if in editor) or quits the application if in build mode </summary>
        ApplicationQuit,

        /// <summary> Triggers the 'Back' button system function </summary>
        Back,
    }
}