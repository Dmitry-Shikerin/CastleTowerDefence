using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HighlightPlus {

    [CustomEditor(typeof(HighlightEffect))]
    [CanEditMultipleObjects]
    public class HighlightEffectEditor : Editor {

#pragma warning disable 0618

        SerializedProperty profile, profileSync, camerasLayerMask, ignoreObjectVisibility, reflectionProbes, GPUInstancing, optimizeSkinnedMesh;
        SerializedProperty ignore, previewInEditor, effectGroup, effectGroupLayer, effectNameFilter, effectNameUseRegEx, combineMeshes, alphaCutOff, cullBackFaces, padding, depthClip, normalsOption, subMeshMask;
        SerializedProperty highlighted, fadeInDuration, fadeOutDuration, flipY, constantWidth, minimumWidth;
        SerializedProperty overlay, overlayMode, overlayColor, overlayAnimationSpeed, overlayMinIntensity, overlayBlending, overlayTexture, overlayTextureUVSpace, overlayTextureScale, overlayTextureScrolling, overlayVisibility;
        SerializedProperty outline, outlineColor, outlineColorStyle, outlineGradient, outlineGradientInLocalSpace, outlineWidth, outlineBlurPasses, outlineQuality, outlineEdgeMode, outlineEdgeThreshold, outlineDownsampling, outlineVisibility, outlineOptimalBlit, outlineBlitDebug, outlineIndependent, outlineContourStyle, outlineSharpness;
        SerializedProperty glow, glowWidth, glowQuality, glowBlurMethod, glowDownsampling, glowHQColor, glowDithering, glowDitheringStyle, glowMagicNumber1, glowMagicNumber2, glowAnimationSpeed, glowBlendPasses, glowPasses, glowVisibility, glowBlendMode, glowOptimalBlit, glowBlitDebug, glowIgnoreMask;
        SerializedProperty innerGlow, innerGlowWidth, innerGlowColor, innerGlowBlendMode, innerGlowVisibility;
        SerializedProperty seeThrough, seeThroughOccluderMask, seeThroughOccluderMaskAccurate, seeThroughOccluderThreshold, seeThroughOccluderCheckInterval, seeThroughOccluderCheckIndividualObjects, seeThroughDepthOffset, seeThroughMaxDepth;
        SerializedProperty seeThroughIntensity, seeThroughTintAlpha, seeThroughTintColor, seeThroughNoise, seeThroughBorder, seeThroughBorderWidth, seeThroughBorderColor, seeThroughOrdered, seeThroughBorderOnly, seeThroughTexture, seeThroughTextureUVSpace, seeThroughTextureScale, seeThroughChildrenSortingMode;
        SerializedProperty targetFX, targetFXTexture, targetFXColor, targetFXCenter, targetFXRotationSpeed, targetFXInitialScale, targetFXEndScale, targetFXScaleToRenderBounds, targetFXUseEnclosingBounds, targetFXOffset;
        SerializedProperty targetFXAlignToGround, targetFXFadePower, targetFXGroundMaxDistance, targetFXGroundLayerMask, targetFXTransitionDuration, targetFXStayDuration, targetFXVisibility;
        SerializedProperty hitFxInitialIntensity, hitFxMode, hitFxFadeOutDuration, hitFxColor, hitFxRadius;
        SerializedProperty cameraDistanceFade, cameraDistanceFadeNear, cameraDistanceFadeFar;

        HighlightEffect thisEffect;
        bool profileChanged, enableProfileApply;
        bool expandGeneralSettings, expandHighlightOptions;
        bool showCurrentOccluders;
        const string HP_GENERAL_SETTINGS = "HPGeneralSettings";
        const string HP_HIGHLIGHT_OPTIONS = "HPHighlightOptions";
        GUIStyle foldoutBold;
        readonly List<Transform> occluders = new List<Transform>();

        void OnEnable() {
            expandGeneralSettings = EditorPrefs.GetBool("HPGeneralSettings", true);
            expandHighlightOptions = EditorPrefs.GetBool("HPHighlightOptions", true);

            profile = serializedObject.FindProperty("profile");
            profileSync = serializedObject.FindProperty("profileSync");
            camerasLayerMask = serializedObject.FindProperty("camerasLayerMask");
            reflectionProbes = serializedObject.FindProperty("reflectionProbes");
            ignoreObjectVisibility = serializedObject.FindProperty("ignoreObjectVisibility");
            GPUInstancing = serializedObject.FindProperty("GPUInstancing");
            optimizeSkinnedMesh = serializedObject.FindProperty("optimizeSkinnedMesh");
            normalsOption = serializedObject.FindProperty("normalsOption");
            subMeshMask = serializedObject.FindProperty("subMeshMask");

            ignore = serializedObject.FindProperty("ignore");
            previewInEditor = serializedObject.FindProperty("previewInEditor");
            effectGroup = serializedObject.FindProperty("effectGroup");
            effectGroupLayer = serializedObject.FindProperty("effectGroupLayer");
            effectNameFilter = serializedObject.FindProperty("effectNameFilter");
            effectNameUseRegEx = serializedObject.FindProperty("effectNameUseRegEx");
            combineMeshes = serializedObject.FindProperty("combineMeshes");
            alphaCutOff = serializedObject.FindProperty("alphaCutOff");
            cullBackFaces = serializedObject.FindProperty("cullBackFaces");
            padding = serializedObject.FindProperty("padding");
            depthClip = serializedObject.FindProperty("depthClip");
            highlighted = serializedObject.FindProperty("_highlighted");
            fadeInDuration = serializedObject.FindProperty("fadeInDuration");
            fadeOutDuration = serializedObject.FindProperty("fadeOutDuration");
            flipY = serializedObject.FindProperty("flipY");
            constantWidth = serializedObject.FindProperty("constantWidth");
            minimumWidth = serializedObject.FindProperty("minimumWidth");
            overlay = serializedObject.FindProperty("overlay");
            overlayMode = serializedObject.FindProperty("overlayMode");
            overlayColor = serializedObject.FindProperty("overlayColor");
            overlayAnimationSpeed = serializedObject.FindProperty("overlayAnimationSpeed");
            overlayMinIntensity = serializedObject.FindProperty("overlayMinIntensity");
            overlayBlending = serializedObject.FindProperty("overlayBlending");
            overlayTexture = serializedObject.FindProperty("overlayTexture");
            overlayTextureUVSpace = serializedObject.FindProperty("overlayTextureUVSpace");
            overlayTextureScale = serializedObject.FindProperty("overlayTextureScale");
            overlayTextureScrolling = serializedObject.FindProperty("overlayTextureScrolling");
            overlayVisibility = serializedObject.FindProperty("overlayVisibility");
            outline = serializedObject.FindProperty("outline");
            outlineColor = serializedObject.FindProperty("outlineColor");
            outlineColorStyle = serializedObject.FindProperty("outlineColorStyle");
            outlineGradient = serializedObject.FindProperty("outlineGradient");
            outlineGradientInLocalSpace = serializedObject.FindProperty("outlineGradientInLocalSpace");
            outlineWidth = serializedObject.FindProperty("outlineWidth");
            outlineBlurPasses = serializedObject.FindProperty("outlineBlurPasses");
            outlineQuality = serializedObject.FindProperty("outlineQuality");
            outlineEdgeMode = serializedObject.FindProperty("outlineEdgeMode");
            outlineEdgeThreshold = serializedObject.FindProperty("outlineEdgeThreshold");
            outlineSharpness = serializedObject.FindProperty("outlineSharpness");
            outlineDownsampling = serializedObject.FindProperty("outlineDownsampling");
            outlineVisibility = serializedObject.FindProperty("outlineVisibility");
            outlineOptimalBlit = serializedObject.FindProperty("outlineOptimalBlit");
            outlineBlitDebug = serializedObject.FindProperty("outlineBlitDebug");
            outlineIndependent = serializedObject.FindProperty("outlineIndependent");
            outlineContourStyle = serializedObject.FindProperty("outlineContourStyle");
            glow = serializedObject.FindProperty("glow");
            glowWidth = serializedObject.FindProperty("glowWidth");
            glowQuality = serializedObject.FindProperty("glowQuality");
            glowBlurMethod = serializedObject.FindProperty("glowBlurMethod");
            glowDownsampling = serializedObject.FindProperty("glowDownsampling");
            glowHQColor = serializedObject.FindProperty("glowHQColor");
            glowAnimationSpeed = serializedObject.FindProperty("glowAnimationSpeed");
            glowBlendPasses = serializedObject.FindProperty("glowBlendPasses");
            glowDithering = serializedObject.FindProperty("glowDithering");
            glowDitheringStyle = serializedObject.FindProperty("glowDitheringStyle");
            glowMagicNumber1 = serializedObject.FindProperty("glowMagicNumber1");
            glowMagicNumber2 = serializedObject.FindProperty("glowMagicNumber2");
            glowAnimationSpeed = serializedObject.FindProperty("glowAnimationSpeed");
            glowPasses = serializedObject.FindProperty("glowPasses");
            glowVisibility = serializedObject.FindProperty("glowVisibility");
            glowBlendMode = serializedObject.FindProperty("glowBlendMode");
            glowOptimalBlit = serializedObject.FindProperty("glowOptimalBlit");
            glowBlitDebug = serializedObject.FindProperty("glowBlitDebug");
            glowIgnoreMask = serializedObject.FindProperty("glowIgnoreMask");
            innerGlow = serializedObject.FindProperty("innerGlow");
            innerGlowColor = serializedObject.FindProperty("innerGlowColor");
            innerGlowWidth = serializedObject.FindProperty("innerGlowWidth");
            innerGlowBlendMode = serializedObject.FindProperty("innerGlowBlendMode");
            innerGlowVisibility = serializedObject.FindProperty("innerGlowVisibility");
            seeThrough = serializedObject.FindProperty("seeThrough");
            seeThroughOccluderMask = serializedObject.FindProperty("seeThroughOccluderMask");
            seeThroughOccluderMaskAccurate = serializedObject.FindProperty("seeThroughOccluderMaskAccurate");
            seeThroughOccluderThreshold = serializedObject.FindProperty("seeThroughOccluderThreshold");
            seeThroughOccluderCheckInterval = serializedObject.FindProperty("seeThroughOccluderCheckInterval");
            seeThroughOccluderCheckIndividualObjects = serializedObject.FindProperty("seeThroughOccluderCheckIndividualObjects");
            seeThroughDepthOffset = serializedObject.FindProperty("seeThroughDepthOffset");
            seeThroughMaxDepth = serializedObject.FindProperty("seeThroughMaxDepth");
            seeThroughIntensity = serializedObject.FindProperty("seeThroughIntensity");
            seeThroughTintAlpha = serializedObject.FindProperty("seeThroughTintAlpha");
            seeThroughTintColor = serializedObject.FindProperty("seeThroughTintColor");
            seeThroughNoise = serializedObject.FindProperty("seeThroughNoise");
            seeThroughBorder = serializedObject.FindProperty("seeThroughBorder");
            seeThroughBorderWidth = serializedObject.FindProperty("seeThroughBorderWidth");
            seeThroughBorderColor = serializedObject.FindProperty("seeThroughBorderColor");
            seeThroughOrdered = serializedObject.FindProperty("seeThroughOrdered");
            seeThroughBorderOnly = serializedObject.FindProperty("seeThroughBorderOnly");
            seeThroughTexture = serializedObject.FindProperty("seeThroughTexture");
            seeThroughTextureScale = serializedObject.FindProperty("seeThroughTextureScale");
            seeThroughTextureUVSpace = serializedObject.FindProperty("seeThroughTextureUVSpace");
            seeThroughChildrenSortingMode = serializedObject.FindProperty("seeThroughChildrenSortingMode");
            targetFX = serializedObject.FindProperty("targetFX");
            targetFXTexture = serializedObject.FindProperty("targetFXTexture");
            targetFXRotationSpeed = serializedObject.FindProperty("targetFXRotationSpeed");
            targetFXInitialScale = serializedObject.FindProperty("targetFXInitialScale");
            targetFXEndScale = serializedObject.FindProperty("targetFXEndScale");
            targetFXScaleToRenderBounds = serializedObject.FindProperty("targetFXScaleToRenderBounds");
            targetFXUseEnclosingBounds = serializedObject.FindProperty("targetFXUseEnclosingBounds");
            targetFXOffset = serializedObject.FindProperty("targetFXOffset");
            targetFXAlignToGround = serializedObject.FindProperty("targetFXAlignToGround");
            targetFXFadePower = serializedObject.FindProperty("targetFXFadePower");
            targetFXGroundMaxDistance = serializedObject.FindProperty("targetFXGroundMaxDistance");
            targetFXGroundLayerMask = serializedObject.FindProperty("targetFXGroundLayerMask");
            targetFXColor = serializedObject.FindProperty("targetFXColor");
            targetFXCenter = serializedObject.FindProperty("targetFXCenter");
            targetFXTransitionDuration = serializedObject.FindProperty("targetFXTransitionDuration");
            targetFXStayDuration = serializedObject.FindProperty("targetFXStayDuration");
            targetFXVisibility = serializedObject.FindProperty("targetFXVisibility");
            hitFxInitialIntensity = serializedObject.FindProperty("hitFxInitialIntensity");
            hitFxMode = serializedObject.FindProperty("hitFxMode");
            hitFxFadeOutDuration = serializedObject.FindProperty("hitFxFadeOutDuration");
            hitFxColor = serializedObject.FindProperty("hitFxColor");
            hitFxRadius = serializedObject.FindProperty("hitFxRadius");
            cameraDistanceFade = serializedObject.FindProperty("cameraDistanceFade");
            cameraDistanceFadeNear = serializedObject.FindProperty("cameraDistanceFadeNear");
            cameraDistanceFadeFar = serializedObject.FindProperty("cameraDistanceFadeFar");

            thisEffect = (HighlightEffect)target;
            thisEffect.Refresh();
        }

        private void OnDisable() {
            EditorPrefs.SetBool(HP_GENERAL_SETTINGS, expandGeneralSettings);
            EditorPrefs.SetBool(HP_HIGHLIGHT_OPTIONS, expandHighlightOptions);
        }

        public override void OnInspectorGUI() {
            bool isManager = IsDefaultEffectUsedByManager();
            EditorGUILayout.Separator();
            serializedObject.Update();

            bool isMeshObject = !thisEffect.spriteMode;

            if (foldoutBold == null) {
                foldoutBold = new GUIStyle(EditorStyles.foldout);
                foldoutBold.fontStyle = FontStyle.Bold;
            }

            EditorGUILayout.BeginHorizontal();
            HighlightProfile prevProfile = (HighlightProfile)profile.objectReferenceValue;
            EditorGUILayout.PropertyField(profile, new GUIContent("Profile", "Create or load stored presets."));
            if (profile.objectReferenceValue != null) {

                if (prevProfile != profile.objectReferenceValue) {
                    profileChanged = true;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("", GUILayout.Width(EditorGUIUtility.labelWidth));
                if (GUILayout.Button(new GUIContent("Create", "Creates a new profile which is a copy of the current settings."), GUILayout.Width(60))) {
                    CreateProfile();
                    profileChanged = false;
                    enableProfileApply = false;
                    GUIUtility.ExitGUI();
                    return;
                }
                if (GUILayout.Button(new GUIContent("Load", "Updates settings with the profile configuration."), GUILayout.Width(60))) {
                    profileChanged = true;
                }
                if (!enableProfileApply)
                    GUI.enabled = false;
                if (GUILayout.Button(new GUIContent("Save", "Updates profile configuration with changes in this inspector."), GUILayout.Width(60))) {
                    enableProfileApply = false;
                    profileChanged = false;
                    thisEffect.profile.Save(thisEffect);
                    EditorUtility.SetDirty(thisEffect.profile);
                    GUIUtility.ExitGUI();
                    return;
                }
                GUI.enabled = true;
                if (GUILayout.Button(new GUIContent("Locate", "Finds the profile in the project"), GUILayout.Width(60))) {
                    if (thisEffect.profile != null) {
                        Selection.activeObject = thisEffect.profile;
                        EditorGUIUtility.PingObject(thisEffect.profile);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(profileSync, new GUIContent("Sync With Profile", "If disabled, profile settings will only be loaded when clicking 'Load' which allows you to customize settings after loading a profile and keep those changes."));
                EditorGUILayout.BeginHorizontal();
            } else {
                if (GUILayout.Button(new GUIContent("Create", "Creates a new profile which is a copy of the current settings."), GUILayout.Width(60))) {
                    CreateProfile();
                    GUIUtility.ExitGUI();
                    return;
                }
            }
            EditorGUILayout.EndHorizontal();


            if (isManager) {
                EditorGUILayout.HelpBox("These are default settings for highlighted objects. If the highlighted object already has a Highlight Effect component, those properties will be used.", MessageType.Info);
            }

            expandGeneralSettings = EditorGUILayout.Foldout(expandGeneralSettings, "General Settings", true, foldoutBold);
            if (expandGeneralSettings) {
                if (!isManager) {
                    EditorGUILayout.PropertyField(previewInEditor);
                }

                EditorGUILayout.PropertyField(camerasLayerMask);
                EditorGUILayout.PropertyField(ignoreObjectVisibility);
                if (thisEffect.staticChildren) {
                    EditorGUILayout.HelpBox("This GameObject or one of its children is marked as static. If highlight is not visible, add a MeshCollider to them (the MeshCollider can be disabled).", MessageType.Warning);
                }

                EditorGUILayout.PropertyField(reflectionProbes);

                if (isMeshObject) {
                    EditorGUILayout.PropertyField(normalsOption);
                }
                EditorGUILayout.PropertyField(optimizeSkinnedMesh);
                if (isMeshObject || optimizeSkinnedMesh.boolValue) {
                    EditorGUILayout.PropertyField(GPUInstancing);
                }
                EditorGUILayout.Separator();
            }

            if (!isManager) {
                EditorGUILayout.LabelField("State", EditorStyles.boldLabel);
                if (isManager) {
                    EditorGUILayout.LabelField(new GUIContent("Highlighted", "Highlight state (controlled by Highlight Manager)."), new GUIContent(thisEffect.highlighted.ToString()));
                } else {
                    EditorGUILayout.PropertyField(highlighted);
                }
                EditorGUILayout.LabelField(new GUIContent("Selected", "Selection state (used by Highlight Trigger or Manager) when using multi-selection option."), new GUIContent(thisEffect.isSelected.ToString()));
                EditorGUILayout.Separator();
            }

            EditorGUILayout.BeginHorizontal();
            expandHighlightOptions = EditorGUILayout.Foldout(expandHighlightOptions, "Highlight Options", true, foldoutBold);
            if (!isMeshObject) {
                GUILayout.Label(new GUIContent("SPRITE MODE", "Highlight Effect over sprites. Some effects are not available in sprite mode"), EditorStyles.centeredGreyMiniLabel);
            }
            if (GUILayout.Button("Help", GUILayout.Width(50))) {
                EditorUtility.DisplayDialog("Quick Help", "Move the mouse over a setting for a short description.\n\nVisit kronnect.com's forum for support, questions and more cool assets.\n\nIf you like Highlight Plus please rate it or leave a review on the Asset Store! Thanks.", "Ok");
            }
            EditorGUILayout.EndHorizontal();
            if (expandHighlightOptions) {
                EditorGUI.BeginChangeCheck();
                if (!isManager) {
                    EditorGUILayout.PropertyField(ignore, new GUIContent("Ignore", "This object won't be highlighted."));
                }
                if (!ignore.boolValue) {
                    EditorGUILayout.PropertyField(effectGroup, new GUIContent("Include", "Additional objects to highlight. Pro tip: when highlighting multiple objects at the same time include them in the same layer or under the same parent."));
                    if (effectGroup.intValue == (int)TargetOptions.LayerInScene || effectGroup.intValue == (int)TargetOptions.LayerInChildren) {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(effectGroupLayer, new GUIContent("Layer"));
                        EditorGUI.indentLevel--;
                    }
                    bool usesHQEffects = (outlineQuality.intValue == (int)QualityLevel.Highest && outline.floatValue > 0) || (glowQuality.intValue == (int)QualityLevel.Highest && glow.floatValue > 0);
                    if (effectGroup.intValue != (int)TargetOptions.OnlyThisObject) {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(effectNameFilter, new GUIContent("Object Name Filter"));
                        if (effectNameUseRegEx.boolValue && !string.IsNullOrEmpty(thisEffect.lastRegExError)) {
                            EditorGUILayout.HelpBox(thisEffect.lastRegExError, MessageType.Error);
                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(effectNameUseRegEx, new GUIContent("Use Regular Expressions", "If enabled, the Object Name Filter is a regular expression."));
                        if (effectNameUseRegEx.boolValue) {
                            if (GUILayout.Button("Help", GUILayout.Width(50))) {
                                if (EditorUtility.DisplayDialog("Regular Expressions", "Check the online Microsoft documentation for regular expressions syntax. You can also use ChatGPT to obtain regular expressions patterns. Some examples:\n^[^A].* will match any name not starting with an A\n.*[^\\d]$ matches any name not ending in a number.", "Online Reference", "Close")) {
                                    Application.OpenURL("https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference");
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        if (isMeshObject) {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PropertyField(combineMeshes);
                            if (combineMeshes.boolValue) {
                                if (GUILayout.Button("Refresh", GUILayout.Width(70))) {
                                    thisEffect.Refresh(true);
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.PropertyField(alphaCutOff, new GUIContent("Alpha Cut Off", "Only for semi-transparent objects. Leave this to zero for normal opaque objects."));
                    if (isMeshObject) {
                        EditorGUILayout.PropertyField(cullBackFaces);
                    }
                    EditorGUILayout.PropertyField(padding);
                    EditorGUILayout.PropertyField(fadeInDuration);
                    EditorGUILayout.PropertyField(fadeOutDuration);
                    EditorGUILayout.PropertyField(cameraDistanceFade);
                    if (cameraDistanceFade.boolValue) {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(cameraDistanceFadeNear, new GUIContent("Near Distance"));
                        EditorGUILayout.PropertyField(cameraDistanceFadeFar, new GUIContent("Far Distance"));
                        EditorGUI.indentLevel--;
                    }
                    if (usesHQEffects) {
                        EditorGUILayout.PropertyField(depthClip);
                        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS || EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) {
                            EditorGUILayout.HelpBox("On mobile, consider enabling 'Depth Clip' option for better results.", MessageType.Info);
                        }
                        if (VRCheck.IsActive()) {
                            EditorGUILayout.PropertyField(flipY, new GUIContent("Flip Y Fix", "Flips outline/glow effect to fix bug introduced in Unity 2019.1.0 when VR is enabled."));
                        }
                    }
                    if (glowQuality.intValue != (int)QualityLevel.Highest || outlineQuality.intValue != (int)QualityLevel.Highest) {
                        EditorGUILayout.PropertyField(constantWidth, new GUIContent("Constant Width", "Compensates outline/glow width with depth increase."));
                        if (!constantWidth.boolValue) {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(minimumWidth);
                            EditorGUI.indentLevel--;
                        }
                    }
                    if (isMeshObject) {
                        EditorGUILayout.PropertyField(subMeshMask);
                        EditorGUILayout.PropertyField(outlineIndependent, new GUIContent("Independent", "Do not combine outline or glow with other highlighted objects."));
                    }
                }
            }

            if (!ignore.boolValue) {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(GUI.skin.box);
                DrawSectionField(outline, "Outline", outline.floatValue > 0);
                if (outline.floatValue > 0) {
                    EditorGUI.indentLevel++;
                    if (isMeshObject) {
                        EditorGUILayout.BeginHorizontal();
                        QualityPropertyField(outlineQuality);
                        if (outlineQuality.intValue == (int)QualityLevel.Highest) {
                            GUILayout.Label("(Screen-Space Effect)");
                        } else {
                            GUILayout.Label("(Mesh-based Effect)");
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    CheckVRSupport(outlineQuality.intValue);
                    if (outlineQuality.intValue == (int)QualityLevel.Highest) {
                        EditorGUILayout.PropertyField(outlineEdgeMode, new GUIContent("Edges"));
                        if (outlineEdgeMode.intValue == (int)OutlineEdgeMode.Any) {
                            EditorGUILayout.PropertyField(outlineEdgeThreshold, new GUIContent("Edge Detection Threshold"));
                        }
                        EditorGUILayout.PropertyField(outlineContourStyle, new GUIContent("Contour Style"));
                        EditorGUILayout.PropertyField(outlineWidth, new GUIContent("Width"));
                        EditorGUILayout.PropertyField(outlineColor, new GUIContent("Color"));
                        EditorGUILayout.PropertyField(outlineBlurPasses, new GUIContent("Blur Passes"));
                        EditorGUILayout.PropertyField(outlineSharpness, new GUIContent("Sharpness"));
                    } else {
                        EditorGUILayout.PropertyField(outlineWidth, new GUIContent("Width"));
                        EditorGUILayout.PropertyField(outlineColorStyle, new GUIContent("Color Style"));
                        switch ((ColorStyle)outlineColorStyle.intValue) {
                            case ColorStyle.SingleColor:
                                EditorGUILayout.PropertyField(outlineColor, new GUIContent("Color"));
                                break;
                            case ColorStyle.Gradient:
                                EditorGUI.indentLevel++;
                                EditorGUILayout.PropertyField(outlineGradient, new GUIContent("Gradient"));
                                EditorGUILayout.PropertyField(outlineGradientInLocalSpace, new GUIContent("In Local Space"));
                                EditorGUI.indentLevel--;
                                break;
                        }
                    }
                    if (outlineQuality.intValue == (int)QualityLevel.Highest && outlineEdgeMode.intValue != (int)OutlineEdgeMode.Any) {
                        EditorGUILayout.PropertyField(outlineDownsampling, new GUIContent("Downsampling"));
                    }
                    if (outlineQuality.intValue == (int)QualityLevel.Highest) {
                        EditorGUILayout.PropertyField(outlineOptimalBlit, new GUIContent("Optimal Blit", "Blits result over a section of the screen instead of rendering to the full screen buffer."));
                        if (outlineOptimalBlit.boolValue) {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(outlineBlitDebug, new GUIContent("Debug View", "Shows the blitting rectangle on the screen."));
                            if (outlineBlitDebug.boolValue && (!previewInEditor.boolValue || !highlighted.boolValue)) {
                                EditorGUILayout.HelpBox("Enable \"Preview In Editor\" and \"Highlighted\" to display the outline Debug View.", MessageType.Warning);
                            }
                            EditorGUI.indentLevel--;
                        }
                    }

                    GUI.enabled = outlineQuality.intValue != (int)QualityLevel.Highest || CheckForwardMSAA(outlineVisibility.intValue == (int)Visibility.OnlyWhenOccluded);
                    if (outlineQuality.intValue == (int)QualityLevel.Highest && glowQuality.intValue == (int)QualityLevel.Highest && glow.floatValue > 0) {
                        EditorGUILayout.PropertyField(glowVisibility, new GUIContent("Visibility"));
                    } else {
                        EditorGUILayout.PropertyField(outlineVisibility, new GUIContent("Visibility"));
                    }
                    GUI.enabled = true;

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.box);
                DrawSectionField(glow, "Outer Glow", glow.floatValue > 0);
                if (glow.floatValue > 0) {
                    EditorGUI.indentLevel++;
                    if (isMeshObject) {
                        EditorGUILayout.BeginHorizontal();
                        QualityPropertyField(glowQuality);
                        if (glowQuality.intValue == (int)QualityLevel.Highest) {
                            GUILayout.Label("(Screen-Space Effect)");
                        } else {
                            GUILayout.Label("(Mesh-based Effect)");
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    CheckVRSupport(glowQuality.intValue);
                    if (glowQuality.intValue == (int)QualityLevel.Highest) {
                        EditorGUILayout.PropertyField(outlineContourStyle, new GUIContent("Contour Style"));
                        EditorGUILayout.PropertyField(glowWidth, new GUIContent("Width"));
                        EditorGUILayout.PropertyField(glowHQColor, new GUIContent("Color"));
                        EditorGUILayout.PropertyField(glowBlurMethod, new GUIContent("Blur Method", "Gaussian: better quality. Kawase: faster."));
                        EditorGUILayout.PropertyField(glowDownsampling, new GUIContent("Downsampling"));
                        EditorGUILayout.PropertyField(glowOptimalBlit, new GUIContent("Optimal Blit", "Blits result over a section of the screen instead of rendering to the full screen buffer."));
                        if (glowOptimalBlit.boolValue) {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(glowBlitDebug, new GUIContent("Debug View", "Shows the blitting rectangle on the screen."));
                            if (glowBlitDebug.boolValue && (!previewInEditor.boolValue || !highlighted.boolValue)) {
                                EditorGUILayout.HelpBox("Enable \"Preview In Editor\" and \"Highlighted\" to display the glow Debug View.", MessageType.Warning);
                            }
                            EditorGUI.indentLevel--;
                        }
                        GUI.enabled = glowQuality.intValue != (int)QualityLevel.Highest || CheckForwardMSAA(glowVisibility.intValue == (int)Visibility.OnlyWhenOccluded);
                        EditorGUILayout.PropertyField(glowVisibility, new GUIContent("Visibility"));
                        EditorGUILayout.PropertyField(glowBlendMode, new GUIContent("Blend Mode"));
                        GUI.enabled = true;
                    } else {
                        EditorGUILayout.PropertyField(glowWidth, new GUIContent("Width"));
                        GUI.enabled = glowQuality.intValue != (int)QualityLevel.Highest || CheckForwardMSAA(false);
                        EditorGUILayout.PropertyField(glowVisibility, new GUIContent("Visibility"));
                        GUI.enabled = true;
                        EditorGUILayout.PropertyField(glowDithering, new GUIContent("Dithering"));
                        if (glowDithering.floatValue > 0) {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(glowDitheringStyle, new GUIContent("Style"));
                            if (glowDitheringStyle.intValue == (int)GlowDitheringStyle.Pattern) {
                                EditorGUILayout.PropertyField(glowMagicNumber1, new GUIContent("Magic Number 1"));
                                EditorGUILayout.PropertyField(glowMagicNumber2, new GUIContent("Magic Number 2"));
                            }
                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.PropertyField(glowBlendPasses, new GUIContent("Blend Passes"));
                        if (!glowBlendPasses.boolValue) {
                            HighlightEffect ef = (HighlightEffect)target;
                            if (ef.glowPasses != null) {
                                for (int k = 0; k < ef.glowPasses.Length - 1; k++) {
                                    if (ef.glowPasses[k].offset > ef.glowPasses[k + 1].offset) {
                                        EditorGUILayout.HelpBox("Glow pass " + k + " has a greater offset than the next one. Reduce it to ensure the next glow pass is visible.", MessageType.Warning);
                                    }
                                }
                            }
                        }
                        EditorGUILayout.PropertyField(glowPasses, true);
                    }
                    EditorGUILayout.PropertyField(glowAnimationSpeed, new GUIContent("Animation Speed"));
                    EditorGUILayout.PropertyField(glowIgnoreMask, new GUIContent("Ignore Mask"));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();

                if (isMeshObject) {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    DrawSectionField(innerGlow, "Inner Glow", innerGlow.floatValue > 0);
                    if (innerGlow.floatValue > 0) {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(innerGlowColor, new GUIContent("Color"));
                        EditorGUILayout.PropertyField(innerGlowWidth, new GUIContent("Width"));
                        EditorGUILayout.PropertyField(innerGlowBlendMode, new GUIContent("Blend Mode"));
                        EditorGUILayout.PropertyField(innerGlowVisibility, new GUIContent("Visibility"));
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.BeginVertical(GUI.skin.box);
                DrawSectionField(overlay, "Overlay", overlay.floatValue > 0);
                if (overlay.floatValue > 0) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(overlayMode, new GUIContent("Mode"));
                    EditorGUILayout.PropertyField(overlayColor, new GUIContent("Color"));
                    EditorGUILayout.PropertyField(overlayTexture, new GUIContent("Texture"));
                    if (overlayTexture.objectReferenceValue != null) {
                        EditorGUILayout.PropertyField(overlayTextureUVSpace, new GUIContent("UV Space"));
                        EditorGUILayout.PropertyField(overlayTextureScale, new GUIContent("Texture Scale"));
                        if ((TextureUVSpace)overlayTextureUVSpace.intValue != TextureUVSpace.Triplanar) {
                            EditorGUILayout.PropertyField(overlayTextureScrolling, new GUIContent("Texture Scrolling"));
                        }
                    }
                    EditorGUILayout.PropertyField(overlayBlending, new GUIContent("Blending"));
                    EditorGUILayout.PropertyField(overlayMinIntensity, new GUIContent("Min Intensity"));
                    EditorGUILayout.PropertyField(overlayAnimationSpeed, new GUIContent("Animation Speed"));
                    EditorGUILayout.PropertyField(overlayVisibility, new GUIContent("Visibility"));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.box);
                DrawSectionField(targetFX, "Target", targetFX.boolValue);
                if (targetFX.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(targetFXTexture, new GUIContent("Texture", "The texture that contains the shape to be drawn over the highlighted object."));
                    EditorGUILayout.PropertyField(targetFXColor, new GUIContent("Color"));
                    EditorGUILayout.PropertyField(targetFXUseEnclosingBounds, new GUIContent("Use Enclosing Bounds"));
                    if (!targetFXUseEnclosingBounds.boolValue) {
                        EditorGUILayout.PropertyField(targetFXCenter, new GUIContent("Center", "Optionally assign a transform. Target will follow transform. If the object is skinned, you can also assign a bone to reflect currenct animation position."));
                    }
                    EditorGUILayout.PropertyField(targetFXRotationSpeed, new GUIContent("Rotation Speed"));
                    EditorGUILayout.PropertyField(targetFXInitialScale, new GUIContent("Initial Scale"));
                    EditorGUILayout.PropertyField(targetFXEndScale, new GUIContent("End Scale"));
                    EditorGUILayout.PropertyField(targetFXScaleToRenderBounds, new GUIContent("Scale To Object Bounds"));
                    EditorGUILayout.PropertyField(targetFXOffset, new GUIContent("Offset"));
                    EditorGUILayout.PropertyField(targetFXAlignToGround, new GUIContent("Align To Ground"));
                    if (targetFXAlignToGround.boolValue) {
                        EditorGUI.indentLevel++;
                        if (thisEffect.includedObjectsCount > 1 && targetFXCenter.objectReferenceValue == null && effectGroup.intValue != (int)TargetOptions.OnlyThisObject) {
                            EditorGUILayout.HelpBox("It's recommended to specify in the 'Center' property above, the specific object used to position the target fx image (will be rendered under that object on the ground).", MessageType.Info);
                        }
                        EditorGUILayout.PropertyField(targetFXGroundMaxDistance, new GUIContent("Ground Max Distance"));
                        EditorGUILayout.PropertyField(targetFXGroundLayerMask, new GUIContent("Ground Layer Mask"));
                        HighlightEffect he = (HighlightEffect)target;
                        if ((targetFXGroundLayerMask.intValue & (1 << he.gameObject.layer)) != 0) {
                            EditorGUILayout.HelpBox("Ground Layer Mask should not include this object layer.", MessageType.Warning);
                        }
                        EditorGUILayout.PropertyField(targetFXFadePower, new GUIContent("Fade Power"));
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.PropertyField(targetFXTransitionDuration, new GUIContent("Transition Duration"));
                    EditorGUILayout.PropertyField(targetFXStayDuration, new GUIContent("Stay Duration"));
                    EditorGUILayout.PropertyField(targetFXVisibility, new GUIContent("Visibility"));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();
            }

            if (isMeshObject) {

                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.PropertyField(seeThrough);
                if (seeThrough.intValue != (int)SeeThroughMode.Never) {
                    if (isManager && seeThrough.intValue == (int)SeeThroughMode.AlwaysWhenOccluded) {
                        EditorGUILayout.HelpBox("This option is not valid in Manager.\nTo make an object always visible add a Highlight Effect component to the gameobject and enable this option on the component.", MessageType.Error);
                    }
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(seeThroughOccluderMask, new GUIContent("Occluder Layer"));
                    if (seeThroughOccluderMask.intValue > 0) {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(seeThroughOccluderMaskAccurate, new GUIContent("Accurate"));
                        EditorGUILayout.PropertyField(seeThroughOccluderThreshold, new GUIContent("Radius Threshold", "Multiplier to the object bounds. Making the bounds smaller prevents false occlusion tests."));
                        EditorGUILayout.PropertyField(seeThroughOccluderCheckInterval, new GUIContent("Check Interval", "Interval in seconds between occlusion tests."));
                        EditorGUILayout.PropertyField(seeThroughOccluderCheckIndividualObjects, new GUIContent("Check Individual Objects"));
                        if (!showCurrentOccluders && Camera.main != null) {
                            GUI.enabled = Application.isPlaying;
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("", GUILayout.Width(30));
                            if (GUILayout.Button(" Show Current Occluders (only during Play Mode) ")) {
                                showCurrentOccluders = true;
                            }
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            GUI.enabled = true;
                        }
                        if (showCurrentOccluders) {
                            HighlightEffect h = (HighlightEffect)target;
                            h.GetOccluders(Camera.main, occluders);
                            int count = occluders != null ? occluders.Count : 0;
                            if (count == 0) {
                                EditorGUILayout.LabelField("No occluders found (using main camera)");
                            } else {
                                EditorGUILayout.LabelField("Occluders found (using main camera):");
                                for (int k = 0; k < count; k++) {
                                    if (occluders[k] == null) continue;
                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField(occluders[k].name);
                                    if (GUILayout.Button("Select")) {
                                        Selection.activeGameObject = occluders[k].gameObject;
                                        GUIUtility.ExitGUI();
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.PropertyField(seeThroughDepthOffset, new GUIContent("Depth Offset" + ((seeThroughDepthOffset.floatValue > 0) ? " •" : "")));
                    EditorGUILayout.PropertyField(seeThroughMaxDepth, new GUIContent("Max Depth" + ((seeThroughMaxDepth.floatValue > 0) ? " •" : "")));
                    EditorGUILayout.PropertyField(seeThroughIntensity, new GUIContent("Intensity"));
                    EditorGUILayout.PropertyField(seeThroughTintColor, new GUIContent("Color"));
                    EditorGUILayout.PropertyField(seeThroughTintAlpha, new GUIContent("Color Blend"));
                    EditorGUILayout.PropertyField(seeThroughNoise, new GUIContent("Noise"));
                    EditorGUILayout.PropertyField(seeThroughTexture, new GUIContent("Texture"));
                    if (seeThroughTexture.objectReferenceValue != null) {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(seeThroughTextureUVSpace, new GUIContent("UV Space"));
                        EditorGUILayout.PropertyField(seeThroughTextureScale, new GUIContent("Texture Scale"));
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.PropertyField(seeThroughBorder, new GUIContent("Border When Hidden" + ((seeThroughBorder.floatValue > 0) ? " •" : "")));
                    if (seeThroughBorder.floatValue > 0) {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(seeThroughBorderWidth, new GUIContent("Width"));
                        EditorGUILayout.PropertyField(seeThroughBorderColor, new GUIContent("Color"));
                        EditorGUILayout.PropertyField(seeThroughBorderOnly, new GUIContent("Border Only"));
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.PropertyField(seeThroughChildrenSortingMode, new GUIContent("Children Sorting Mode"));
                    EditorGUILayout.PropertyField(seeThroughOrdered, new GUIContent("Ordered"));

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.BeginVertical(GUI.skin.box);
            DrawSectionField(hitFxInitialIntensity, "Hit FX", hitFxInitialIntensity.floatValue > 0);
            if (hitFxInitialIntensity.floatValue > 0) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(hitFxMode, new GUIContent("Mode"));
                EditorGUILayout.PropertyField(hitFxFadeOutDuration, new GUIContent("Fade Out Duration"));
                EditorGUILayout.PropertyField(hitFxColor, new GUIContent("Color"));
                if ((HitFxMode)hitFxMode.intValue == HitFxMode.LocalHit) {
                    EditorGUILayout.PropertyField(hitFxRadius, new GUIContent("Radius"));
                }
                EditorGUI.indentLevel--;
            }

            if (!Application.isPlaying) {
                EditorGUILayout.HelpBox("Enter Play Mode to test this feature. In your code, call effect.HitFX() method to execute this hit effect.", MessageType.Info);
            } else {
                EditorGUI.indentLevel++;
                if (GUILayout.Button("Execute Hit")) {
                    thisEffect.HitFX();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();


            if (serializedObject.ApplyModifiedProperties() || profileChanged || Event.current.commandName == "UndoRedoPerformed") {
                if (thisEffect.profile != null) {
                    if (profileChanged) {
                        EditorUtility.SetDirty(target);
                        thisEffect.profile.Load(thisEffect);
                        profileChanged = false;
                        enableProfileApply = false;
                    } else {
                        enableProfileApply = true;
                    }
                }

                foreach (HighlightEffect effect in targets) {
                    effect.Refresh();
                }
            }
        }

        void DrawSectionField(SerializedProperty property, string label, bool active) {
            EditorGUILayout.PropertyField(property, new GUIContent(active ? label + " •" : label));
        }

        void CheckVRSupport(int qualityLevel) {
            if (qualityLevel == (int)QualityLevel.Highest && VRCheck.IsActive()) {
                if (PlayerSettings.stereoRenderingPath != StereoRenderingPath.MultiPass) {
                    EditorGUILayout.HelpBox("Highest Quality only supports VR Multi-Pass as CommandBuffers do not support this VR mode yet. Either switch to 'High Quality' or change VR Stereo mode to Multi-Pass.", MessageType.Error);
                }
            }
        }

        bool CheckForwardMSAA(bool alwaysWhenOccluded) {
            if (QualitySettings.antiAliasing > 1) {
                if (Camera.main != null && Camera.main.allowMSAA) {
                    if (alwaysWhenOccluded) {
                        EditorGUILayout.HelpBox("Visibility 'Always When Occluded' may not work with MSAA. You may need to disable MSAA in Quality Settings OR choose a different quality level.", MessageType.Info);
                    } else if (!depthClip.boolValue) {
                        EditorGUILayout.HelpBox("Effect will be shown always on top due to MSAA. To enable depth clipping enable 'Depth Clip' option above OR disable MSAA in Quality Settings OR choose a different quality level.", MessageType.Info);
                        return false;
                    }
                }
            }
            return true;
        }

        static readonly int[] qualityValues = { 0, 3, 1, 2 };
        static readonly GUIContent[] qualityTexts = { new GUIContent("Fastest"), new GUIContent("Medium"), new GUIContent("High"), new GUIContent("Highest") };

        public static void QualityPropertyField(SerializedProperty prop) {
            prop.intValue = EditorGUILayout.IntPopup(new GUIContent("Quality", "Default and High use a mesh displacement technique. Highest quality can provide best look and also performance depending on the complexity of mesh."), prop.intValue, qualityTexts, qualityValues);
        }

        bool IsDefaultEffectUsedByManager() {
            MonoBehaviour[] components = thisEffect.GetComponents<MonoBehaviour>();
            if (components != null) {
                for (int k = 0; k < components.Length; k++) {
                    if (components[k] == null || !components[k].enabled)
                        continue;
                    string name = components[k].GetType().Name;
                    if ("HighlightManager".Equals(name)) return true;
                }
            }
            return false;
        }

        #region Profile handling

        void CreateProfile() {

            HighlightProfile newProfile = CreateInstance<HighlightProfile>();
            newProfile.Save(thisEffect);

            AssetDatabase.CreateAsset(newProfile, "Assets/Highlight Plus Profile.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newProfile;

            thisEffect.profile = newProfile;
        }


        #endregion

#pragma warning restore 0618

    }

}