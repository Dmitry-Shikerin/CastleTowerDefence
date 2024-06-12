using FIMSpace.FTex;
using UnityEditor;
using UnityEngine;
using static FIMSpace.FEditor.FScale_EditorToolsMethods;

namespace FIMSpace.FEditor
{

    public class FScale_ChannelledGenerator : EditorWindow
    {

        public Texture2D From;
        public enum EChannelSelect { R, G, B, A, RGB }
        public EChannelSelect ChannelFrom = EChannelSelect.R;
        public EChannelSelect ApplyTo = EChannelSelect.A;

        public enum EDefaultColorMode { Black, White, Clear, Gray }
        public EDefaultColorMode DefaultColorMode = EDefaultColorMode.Black;

        int textureSize = 2048;
        string newFileName = "New PNG Texture";
        string customPath = "";
        bool AutoPath = true;
        string lastPath = "";

        public enum EChannelMode { Color, Texture, None }



        [System.Serializable]
        public class ChannelSetup
        {
            public EChannelMode Mode = EChannelMode.Color;
            public float ChannelValue = 0f;
            public Texture2D OtherTex;
            public EChannelSelect ChannelFromTex = EChannelSelect.R;

            public PixelProcessor PixelProc;

            internal void DrawGUI(Color styleColor)
            {
                GUI.backgroundColor = styleColor;
                EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle);

                GUI.backgroundColor = Color.Lerp(styleColor, Color.white, 0.4f);

                GUILayout.Space(4);

                bool skip = false;

                if (Mode == EChannelMode.None)
                {
                    if (styleColor == Color.white)
                    {
                        EditorGUILayout.LabelField("NOT USING ALPHA CHANNEL", FGUI_Resources.HeaderStyle);
                        GUILayout.Space(5);
                    }
                    else
                    {
                        if (styleColor == Color.blue)
                        {
                            EditorGUILayout.LabelField("If not using alpha and B -> generating RG texture");
                        }
                        else
                        {
                            if (styleColor == Color.green)
                            {
                                EditorGUILayout.LabelField("If not using alpha G and B -> generating R texture");
                            }
                        }
                    }

                    skip = true;
                }

                Mode = (EChannelMode)EditorGUILayout.EnumPopup("Mode:", Mode);


                if (!skip)
                {
                    GUILayout.Space(5);

                    if (Mode == EChannelMode.Color)
                    {
                        ChannelValue = EditorGUILayout.Slider("Channel Color Value:", ChannelValue, 0f, 1f);
                    }
                    else if (Mode == EChannelMode.Texture)
                    {
                        OtherTex = (Texture2D)EditorGUILayout.ObjectField("Get channel from:", OtherTex, typeof(Texture2D), false);
                        if (OtherTex != null)
                        {
                            ChannelFromTex = (EChannelSelect)EditorGUILayout.EnumPopup("Which channel to steal?", ChannelFromTex);
                        }

                        PixelProc?.DrawGUI();
                    }

                }


                GUI.backgroundColor = Color.white;

                EditorGUILayout.EndVertical();
            }

            internal float GetChannelOutOfPixel(Color32 pix)
            {
                if ( ChannelFromTex == EChannelSelect.R) return (float)(pix.r) / byte.MaxValue;
                else if ( ChannelFromTex == EChannelSelect.G) return (float)(pix.g) / byte.MaxValue;
                else if ( ChannelFromTex == EChannelSelect.B) return (float)(pix.b) / byte.MaxValue;
                return (float)(pix.a) / byte.MaxValue;
            }
        }



        [System.Serializable]
        public class PixelProcessor
        {
            public EprocessorType Type = EprocessorType.None;
            public enum EprocessorType
            {
                None, ResetContrast, Invert, Add, Multiply
            }

            public bool Clamp = true;
            public float AddValue = 0f;
            public float MulValue = 1f;
            public float ToGray = 1f;

            internal void DrawGUI()
            {
                EditorGUILayout.BeginVertical(FGUI_Resources.FrameBoxStyle);
                Type = (EprocessorType)EditorGUILayout.EnumPopup("Process Pixels:", Type);

                if (Type == EprocessorType.ResetContrast)
                {
                    ToGray = EditorGUILayout.Slider("Reset Contrast:", ToGray, 0f, 1f);
                }
                else if (Type == EprocessorType.Add)
                {
                    AddValue = EditorGUILayout.FloatField("Add/Subtract:", AddValue);
                }
                else if (Type == EprocessorType.Multiply)
                {
                    MulValue = EditorGUILayout.FloatField("Multiply:", MulValue);
                }
                EditorGUILayout.EndVertical();
            }

            //public Color ProcessPixel(Color c)
            //{
            //    if (Type == EprocessorType.None) return c;

            //    if ( Type == EprocessorType.Invert)
            //    {
            //        c = new Color(ProcessInvert(c.r), ProcessInvert(c.g), ProcessInvert(c.b), c.a);
            //    }

            //    return c;
            //}

            float ProcessAdd(float v)
            {
                return v + AddValue;
            }

            float ProcessMul(float v)
            {
                return v * MulValue;
            }

            float ProcessInvert(float v)
            {
                return 1f - v;
            }

            float ProcessResetContrast(float v)
            {
                return Mathf.LerpUnclamped(v, 0.5f, ToGray);
            }

            public float ProcessChannel(float v)
            {
                if (Type == EprocessorType.Invert) v = ProcessInvert(v);
                if (Type == EprocessorType.ResetContrast) v = ProcessResetContrast(v);
                if (Type == EprocessorType.Add) v = ProcessAdd(v);
                if (Type == EprocessorType.Multiply) v = ProcessMul(v);

                if (Clamp) v = Mathf.Clamp01(v);

                return v;
            }
        }







        ChannelSetup[] channels = new ChannelSetup[4];
        ChannelSetup R { get { return channels[0]; } }
        ChannelSetup G { get { return channels[1]; } }
        ChannelSetup B { get { return channels[2]; } }
        ChannelSetup A { get { return channels[3]; } }



        public static void Init()
        {
            FScale_ChannelledGenerator window = (FScale_ChannelledGenerator)GetWindow(typeof(FScale_ChannelledGenerator));

            window.minSize = new Vector2(570f, 425f);

            window.titleContent = new GUIContent("Channeled Generator");
            window.Show();
        }



        void OnGUI()
        {
            if (channels[0] == null) channels[0] = new ChannelSetup() { Mode = EChannelMode.Color, ChannelValue = 0 };
            if (channels[1] == null) channels[1] = new ChannelSetup() { Mode = EChannelMode.Color, ChannelValue = 0 };
            if (channels[2] == null) channels[2] = new ChannelSetup() { Mode = EChannelMode.Color, ChannelValue = 0 };
            if (channels[3] == null) channels[3] = new ChannelSetup() { Mode = EChannelMode.None, ChannelValue = 1 };

            if (R.PixelProc == null) R.PixelProc = new PixelProcessor();
            if (G.PixelProc == null) G.PixelProc = new PixelProcessor();
            if (B.PixelProc == null) B.PixelProc = new PixelProcessor();
            if (A.PixelProc == null) A.PixelProc = new PixelProcessor();

            EditorGUILayout.LabelField("Generate new PNG file with custom color channels", EditorStyles.centeredGreyMiniLabel);
            GUILayout.Space(6);

            EditorGUILayout.BeginHorizontal();
            textureSize = EditorGUILayout.IntField("Texture Size", textureSize);
            textureSize = Mathf.Clamp(textureSize, 1, 4096);

            if (GUILayout.Button("Power of 2", GUILayout.Width(80)))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("512x512"), textureSize == 512, () => { textureSize = 512; });
                menu.AddItem(new GUIContent("1024x1024"), textureSize == 1024, () => { textureSize = 1024; });
                menu.AddItem(new GUIContent("2048x2048"), textureSize == 2048, () => { textureSize = 2048; });
                menu.AddItem(new GUIContent("4096x4096"), textureSize == 4096, () => { textureSize = 4096; });
                menu.ShowAsContext();
            }

            EditorGUILayout.EndHorizontal();

            newFileName = EditorGUILayout.TextField("New Filename: ", newFileName);

            string autoPath = "";


            #region Selection Find

            if (Selection.objects.Length > 0)
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    var o = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(Selection.objects[i]));
                    if (o != null)
                    {
                        autoPath = AssetDatabase.GetAssetPath(Selection.objects[i]);
                        autoPath = System.IO.Path.GetDirectoryName(autoPath);
                        break;
                    }
                }
            }

            if (autoPath != "")
            {
                lastPath = autoPath;
            }
            else
            {
                autoPath = lastPath;
            }

            #endregion


            GUILayout.Space(8);
            EditorGUILayout.BeginHorizontal(); EditorGUIUtility.labelWidth = (38);
            AutoPath = EditorGUILayout.ToggleLeft("Auto:", AutoPath, GUILayout.Width(50));
            GUILayout.Space(16); EditorGUIUtility.labelWidth = (76);

            string finalPath = "";

            if (AutoPath)
            {
                GUI.enabled = false;
                EditorGUILayout.TextField("Save Path: ", autoPath == "" ? "Select some file to save in it's directory" : autoPath); GUI.enabled = true;
                finalPath = autoPath;
            }
            else
            {
                GUILayout.Space(16); EditorGUIUtility.labelWidth = (156);
                customPath = EditorGUILayout.TextField("Save Path:  (Assets/...)", customPath);
                finalPath = customPath;
            }

            EditorGUIUtility.labelWidth = (0);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(6);
            EditorGUILayout.LabelField("Channels Settings:", EditorStyles.boldLabel);


            EditorGUILayout.BeginHorizontal();
            channels[0].DrawGUI(Color.red);
            channels[1].DrawGUI(Color.green);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            channels[2].DrawGUI(Color.blue);
            channels[3].DrawGUI(Color.white);
            EditorGUILayout.EndHorizontal();


            FGUI_Inspector.DrawUILine(Color.white * 0.35f, 2, 16);


#if UNITY_2019_4_OR_NEWER

            //if (QualitySettings.renderPipeline is UnityEngine.Rendering.HighDefinition.HDRenderPipelineAsset)
            {
                GUILayout.Space(18);
                EditorGUILayout.HelpBox("Quick Tip: HDRP mask map channels are:\nR - Metallic     G - Ambient Occlusion\nB - Detail Mask       A - Smothness", MessageType.None);
            }

#endif

            GUILayout.FlexibleSpace();

            if (finalPath == "") GUI.enabled = false;
            if (GUILayout.Button("Generate '" + newFileName + "'.png", GUILayout.Height(32)))
            {
                ProcessChanneling(finalPath);
            }
        }



        public void ProcessChanneling(string path)
        {
            try
            {

                #region Define Format

                TextureFormat format = TextureFormat.RGB24;

                if (A.Mode == EChannelMode.None)
                {
                    if (B.Mode == EChannelMode.None)
                    {
                        format = TextureFormat.RG16;
                    }
                    else if (G.Mode == EChannelMode.None)
                    {
                        format = TextureFormat.R8;
                    }
                }
                else
                {
                    format = TextureFormat.RGBA32;
                }

                #endregion


                EditorUtility.DisplayProgressBar("Channeling textures...", "Working to generate file at" + path + "/" + newFileName + ".png\nTarget format = " + format, 0.2f);

                Texture2D newTex = new Texture2D(textureSize, textureSize, format, true);
                newTex.name = newFileName;

                var newPix = newTex.GetPixels();

                #region Refresh

                if (R.PixelProc == null) R.PixelProc = new PixelProcessor();
                if (G.PixelProc == null) G.PixelProc = new PixelProcessor();
                if (B.PixelProc == null) B.PixelProc = new PixelProcessor();
                if (A.PixelProc == null) A.PixelProc = new PixelProcessor();

                #endregion


                #region Define default color

                Color defaultColor = Color.white;
                if (DefaultColorMode == EDefaultColorMode.Black) defaultColor = Color.black;
                else if (DefaultColorMode == EDefaultColorMode.Gray) defaultColor = Color.gray;
                else if (DefaultColorMode == EDefaultColorMode.Clear) defaultColor = Color.clear;

                // Default color values
                if (R.Mode == EChannelMode.Color) defaultColor.r = R.ChannelValue;
                if (G.Mode == EChannelMode.Color) defaultColor.g = G.ChannelValue;
                if (B.Mode == EChannelMode.Color) defaultColor.b = B.ChannelValue;
                if (A.Mode == EChannelMode.Color) defaultColor.a = A.ChannelValue;


                for (int p = 0; p < newPix.Length; p++) newPix[p] = defaultColor;

                #endregion



                #region Handling temporary scaled other textures for stealing selective channels


                // Preparing channel textures
                if (R.Mode == EChannelMode.Texture && R.OtherTex != null)
                {
                    Color32[] pix = GetScaledPixelsOf(R.OtherTex);

                    for (int i = 0; i < pix.Length; i++)
                    {
                        Color px = newPix[i];
                        px.r = R.GetChannelOutOfPixel(pix[i]);
                        px.r = R.PixelProc.ProcessChannel(px.r);
                        newPix[i] = px;
                    }
                }

                if (G.Mode == EChannelMode.Texture && G.OtherTex != null)
                {
                    Color32[] pix = GetScaledPixelsOf(G.OtherTex);

                    for (int i = 0; i < pix.Length; i++)
                    {
                        Color px = newPix[i];
                        px.g = G.GetChannelOutOfPixel(pix[i]);
                        px.g = G.PixelProc.ProcessChannel(px.g);
                        newPix[i] = px;
                    }
                }

                if (B.Mode == EChannelMode.Texture && B.OtherTex != null)
                {
                    Color32[] pix = GetScaledPixelsOf(B.OtherTex);

                    for (int i = 0; i < pix.Length; i++)
                    {
                        Color px = newPix[i];
                        px.b = B.GetChannelOutOfPixel(pix[i]);
                        px.b = B.PixelProc.ProcessChannel(px.b);
                        newPix[i] = px;
                    }
                }

                if (A.Mode == EChannelMode.Texture && A.OtherTex != null)
                {
                    Color32[] pix = GetScaledPixelsOf(A.OtherTex);

                    if (pix.Length != newPix.Length) { UnityEngine.Debug.Log("Wrong Scaled Textures? " + pix.Length + " VS " + newPix.Length + " pixel counts!"); }

                    for (int i = 0; i < pix.Length; i++)
                    {
                        Color px = newPix[i];
                        px.a = A.GetChannelOutOfPixel(pix[i]);
                        px.a = A.PixelProc.ProcessChannel(px.a);
                        newPix[i] = px;
                    }
                }

                #endregion


                EditorUtility.DisplayProgressBar("Channeling textures...", "Working to generate file at" + path + "/" + newFileName + ".png\nFinalizing..." + format, 0.75f);

                // Finalize
                newTex.SetPixels(newPix);
                newTex.Apply(true, false);

                string texPath = (path + "/" + newFileName) + ".png";


                #region Save File

                //string texPath = System.IO.Path.Combine(path, newFileName) + ".png";
                System.IO.File.WriteAllBytes(texPath, newTex.EncodeToPNG());
                AssetDatabase.Refresh(ImportAssetOptions.Default);
                AssetDatabase.ImportAsset(texPath, ImportAssetOptions.Default);

                newTex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);

                #endregion


                #region Adjust file settings

                TextureImporter srcImporter = GetTextureAsset(newTex);

                srcImporter.isReadable = true;

                var formatSett = srcImporter.GetPlatformTextureSettings("Standalone");
                formatSett.textureCompression = TextureImporterCompression.Compressed;
                if (formatSett.maxTextureSize < newTex.width) formatSett.maxTextureSize = newTex.width;
                if (srcImporter.maxTextureSize < newTex.width) srcImporter.maxTextureSize = newTex.width;

                if (format == TextureFormat.RGBA32) { formatSett.format = TextureImporterFormat.RGBA32; formatSett.overridden = true; }
                else
                if (format == TextureFormat.RG16) { formatSett.format = TextureImporterFormat.RG32; formatSett.overridden = true; }
                else if (format == TextureFormat.R8) { formatSett.format = TextureImporterFormat.R8; formatSett.overridden = true; }
                else formatSett.overridden = false;

                srcImporter.SetPlatformTextureSettings(formatSett);

                #endregion


                #region Refresh Asset

                srcImporter.SaveAndReimport();
                AssetDatabase.ImportAsset(texPath);

                srcImporter = GetTextureAsset(newTex);
                srcImporter.isReadable = false;
                srcImporter.SaveAndReimport();
                AssetDatabase.ImportAsset(texPath);
                AssetDatabase.Refresh();

                EditorUtility.SetDirty(newTex);

                #endregion


                EditorUtility.ClearProgressBar();

            }
            catch (System.Exception exc)
            {
                EditorUtility.ClearProgressBar();
                UnityEngine.Debug.LogError("[Fimpo Image Tools] Something went wrong when channeling textures!");
                UnityEngine.Debug.LogException( exc);
            }
        }




        Color32[] GetScaledPixelsOf(Texture2D texFile)
        {
            var importer = GetTextureAsset(texFile);
            bool wasRead = importer.isReadable;
            importer.isReadable = true;
            importer.SaveAndReimport();

            Color32[] sameScalePix = null;

            try
            {
                if (texFile.width == textureSize)
                {
                    sameScalePix = texFile.GetPixels32();
                }
                else
                {
                    EditorUtility.DisplayProgressBar("Channeling textures...", "Scaling " + texFile.name + " for accurate pixels...", 0.5f);

                    sameScalePix = texFile.GetPixels32();
                    sameScalePix = FTex_ScaleLanczos.ScaleTexture(sameScalePix, texFile.width, texFile.height, textureSize, textureSize, 4);
                }
            }
            catch (System.Exception exc)
            {
                EditorUtility.ClearProgressBar();
                UnityEngine.Debug.LogError("[Fimpo Image Tools] Something went wrong when scaling textures! " + exc);
            }

            importer.isReadable = wasRead;
            importer.SaveAndReimport();

            return sameScalePix;
        }


    }
}