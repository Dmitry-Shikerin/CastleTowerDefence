using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using MegaFiers;
using Unity.Collections;

[InitializeOnLoad]
public class CompilerCheck
{
	static CompilerCheck()
	{
#if UNITY_2021_1_OR_NEWER
		CompilationPipeline.compilationStarted += DisposeNew;
#else
		CompilationPipeline.assemblyCompilationStarted += Dispose;
#endif
		CompilationPipeline.assemblyCompilationFinished += Dispose2;
		CompilationPipeline.compilationStarted += Dispose1;
		EditorApplication.playModeStateChanged += LogPlayModeState;
	}

	static void MemoryCleanup()
	{
#if UNITY_2022_2_OR_NEWER
		MegaModifyObject[] mods = GameObject.FindObjectsByType<MegaModifyObject>(FindObjectsSortMode.None);
#else
		MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();
#endif
		for ( int i = 0; i < mods.Length; i++ )
		{
			mods[i].ResetCorners();
			mods[i].DisposeArrays();
		}

#if UNITY_2022_2_OR_NEWER
		MegaWrap[] wraps = GameObject.FindObjectsByType<MegaWrap>(FindObjectsSortMode.None);
#else
		MegaWrap[] wraps = GameObject.FindObjectsOfType<MegaWrap>();
#endif
		for ( int i = 0; i < wraps.Length; i++ )
			wraps[i].DisposeArrays();
	}

	static void DisposeNew(object obj)
	{
		MemoryCleanup();
	}

	static void Dispose(string assembly)
	{
		MemoryCleanup();
	}

	static void Dispose2(string assembly, CompilerMessage[] msgs)
	{
		MemoryCleanup();
	}

	static void Dispose1(object obj)
	{
#if UNITY_2022_2_OR_NEWER
		MegaModifyObject[] mods = GameObject.FindObjectsByType<MegaModifyObject>(FindObjectsSortMode.None);
#else
		MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();
#endif

		for ( int i = 0; i < mods.Length; i++ )
			mods[i].ResetCorners();
	}

	static void LogPlayModeState(PlayModeStateChange state)
	{
		NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;

		if ( state == PlayModeStateChange.ExitingEditMode )
		{
			//MegaModifyObject.noUpdate = true;
			MegaWrap.IsPlaying = true;
#if UNITY_2022_2_OR_NEWER
			MegaModifyObject[] mods = GameObject.FindObjectsByType<MegaModifyObject>(FindObjectsSortMode.None);
#else
			MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();
#endif

			for ( int i = 0; i < mods.Length; i++ )
			{
				mods[i].noUpdate = true;
				mods[i].ResetCorners();
				mods[i].DisposeArrays();
			}

#if UNITY_2022_2_OR_NEWER
			MegaWrap[] wraps = GameObject.FindObjectsByType<MegaWrap>(FindObjectsSortMode.None);
#else
			MegaWrap[] wraps = GameObject.FindObjectsOfType<MegaWrap>();
#endif

			for ( int i = 0; i < wraps.Length; i++ )
			{
				wraps[i].canGetMesh = false;
				wraps[i].DisposeArrays();
			}
			return;
		}

		if ( state == PlayModeStateChange.ExitingPlayMode )
		{
#if UNITY_2022_2_OR_NEWER
			MegaModifyObject[] mods = GameObject.FindObjectsByType<MegaModifyObject>(FindObjectsSortMode.None);
#else
			MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();
#endif

			for ( int i = 0; i < mods.Length; i++ )
			{
				mods[i].noUpdate = true;
			}
		}

		if ( state == PlayModeStateChange.EnteredEditMode )
		{
#if UNITY_2022_2_OR_NEWER
			MegaModifyObject[] mods = GameObject.FindObjectsByType<MegaModifyObject>(FindObjectsSortMode.None);
#else
			MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();
#endif

			for ( int i = 0; i < mods.Length; i++ )
			{
				mods[i].noUpdate = false;
			}
		}

		if ( state == PlayModeStateChange.EnteredPlayMode )
		{
#if UNITY_2022_2_OR_NEWER
			MegaModifyObject[] mods = GameObject.FindObjectsByType<MegaModifyObject>(FindObjectsSortMode.None);
#else
			MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();
#endif

			for ( int i = 0; i < mods.Length; i++ )
				mods[i].noUpdate = false;

			//MegaModifyObject.noUpdate = false;
#if UNITY_2022_2_OR_NEWER
			MegaWrap[] wraps = GameObject.FindObjectsByType<MegaWrap>(FindObjectsSortMode.None);
#else
			MegaWrap[] wraps = GameObject.FindObjectsOfType<MegaWrap>();
#endif

			for ( int i = 0; i < wraps.Length; i++ )
				wraps[i].canGetMesh = true;
		}
		else
			MegaWrap.IsPlaying = false;
	}
}
