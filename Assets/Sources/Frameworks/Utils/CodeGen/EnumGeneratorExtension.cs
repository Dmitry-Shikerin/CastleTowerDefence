using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Doozy.Engine.Soundy;
using MyAudios.Soundy.Sources.DataBases.Domain.Data;
using MyAudios.Soundy.Sources.Settings.Domain.Configs;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.Utils.CodeGen
{
    public static class EnumGeneratorExtension
    {
        public static Enum GenerateEnum(this IEnumerable<string> values)
        {
            // Get the current application domain for the current thread.
            // AppDomain currentDomain = AppDomain.CurrentDomain;

            // Create a dynamic assembly in the current application domain,
            // and allow it to be executed and saved to disk.
            // AssemblyName aName = new AssemblyName("TempAssembly");
            // AssemblyBuilder assemblyBuilder = currentDomain.DefineDynamicAssembly(
            //     aName, AssemblyBuilderAccess.RunAndSave);
            //
            // // Define a dynamic module in "TempAssembly" assembly. For a single-
            // // module assembly, the module has the same name as the assembly.
            // ModuleBuilder mb = assemblyBuilder.DefineDynamicModule(aName.Name, aName.Name + ".dll");
            //
            // // Define a public enumeration with the name "Elevation" and an
            // // underlying type of Integer.
            // EnumBuilder eb = mb.DefineEnum("Elevation", TypeAttributes.Public, typeof(int));
            //
            // // Define two members, "High" and "Low".
            // eb.DefineLiteral("Low", 0);
            // eb.DefineLiteral("High", 1);
            //
            // // Create the type and save the assembly.
            // Type finished = eb.CreateType();
            // assemblyBuilder.Save(aName.Name + ".dll");
            //
            // foreach( object o in Enum.GetValues(finished) )
            // {
            //     Console.WriteLine("{0}.{1} = {2}", finished, o, ((int) o));
            // }

            return null;
        }

        [InitializeOnLoadMethod]
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

            // Записываем код в файл
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