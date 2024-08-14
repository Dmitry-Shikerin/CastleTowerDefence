using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace AlmostEngine.Shadows
{
	public class LightManager : Singleton<LightManager>
	{
		public Dictionary<Light, GameLight> m_RegisteredLights = new Dictionary<Light, GameLight>();

		public delegate void RegisterDelegate(GameLight light);

		public static RegisterDelegate onLightRegistered = (GameLight light) => {
		};
		public static RegisterDelegate onLightUnregistered = (GameLight light) => {
		};

		protected override void OnSingletonAwake()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void OnDestroy()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			RegisterAllSceneLights();
		}

		public void RegisterAllSceneLights()
		{
			m_RegisteredLights.Clear();
			Light[] lights = Resources.FindObjectsOfTypeAll<Light>();
			foreach (Light light in lights) {
				RegisterLight(light);
			}
		}

		public void RegisterLight(Light light)
		{
			// Ignore prefabs
			if (light.gameObject.scene.buildIndex < 0)
				return;

			// Ignore already registered lights
			if (m_RegisteredLights.ContainsKey(light)) {
				return;
			}

			// Create the gamelight component
			GameLight gameLight = light.GetComponent<GameLight>();
			if (gameLight == null) {
				gameLight = light.gameObject.AddComponent<GameLight>();
			}

			m_RegisteredLights.Add(light, gameLight);
			onLightRegistered(gameLight);

			//Debug.Log("nb: " + m_RegisteredLights.Count);
		}

		public void UnregisterLight(Light light)
		{
			if (light != null && m_RegisteredLights.ContainsKey(light)) {		
				onLightUnregistered(m_RegisteredLights[light]);		
				m_RegisteredLights.Remove(light);
			}
		}




	}
}