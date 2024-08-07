namespace MyAudios.Soundy.Sources.SoundSources.Enums
{
    /// <summary> Describes types of available sound sources </summary>
    public enum SoundSource
    {
        /// <summary> Soundy - Sound Manager. Powerful audio solution perfectly integrated with DoozyUI. </summary>
        Soundy,

        /// <summary> Audio clip direct reference. As simple as a drag and a drop. </summary>
        AudioClip,

        /// <summary> External audio solution plugin created by Dark Tonic. To work, this option requires that the Master Audio plugin be installed in the project </summary>
        MasterAudio
    }
}