using System;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using MyAudios.Soundy.Sources.Settings.Domain.Configs;
using UnityEngine;

namespace Sources.Frameworks.Utils.CodeGen
{
    public static class EnumGeneratorExtension
    {
        // [InitializeOnLoadMethod]
        public static void GenerateEnum()
        {
            List<string> soundNames = new List<string>();

            foreach (SoundDatabase soundDatabase in SoundySettings.Database.SoundDatabases)
            foreach (SoundGroupData soundGroupData in soundDatabase.Database)
            {
                if (soundNames.Contains(soundGroupData.SoundName))
                    continue;

                soundNames.Add(soundGroupData.SoundName);
            }

            int i = 0;
            string path = $"{Application.dataPath}/Sources/Generated/SoundNames.cs";
            string code = $@"namespace Sources.Generated
{{
   public enum SoundNames 
   {{
{
    String.Join("\n",
        soundNames
            .Where(soundName => soundName != "")
            .Select(soundName => $"      {soundName.Replace(" ", "")} = {i++},"))
}
   }}
}}";
            
            System.IO.File.WriteAllText(path, code);
        }

        // [InitializeOnLoadMethod]
        public static void GenerateLayers()
        {
            string path = $"{Application.dataPath}/Sources/Generated/Layers.cs";
            string code = $@"public static class Layers 
{{
{
    String.Join("\n",
        GetLayerNames()
            .Where(layerName => layerName != "")
            .Select(layerName => $"const string {layerName.Replace(" ", "_").ToUpper()} = \"{layerName}\";"))
}
}}";

            // Записываем код в файл
            System.IO.File.WriteAllText(path, code);
        }

        private static string[] GetLayerNames()
        {
            string[] layers = new string[32];

            for (int i = 0; i < 32; i++)
                layers[i] = LayerMask.LayerToName(i);

            return layers;
        }
    }
}