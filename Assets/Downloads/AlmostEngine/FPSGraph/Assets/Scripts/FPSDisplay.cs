using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AlmostEngine.Report
{

	public class FPSDisplay : MonoBehaviour
	{

		public Text m_Text;

		float m_DeltaTime = 0.0f;

		// How many sample to get before to display its mean
		int m_RefreshRate = 1;
		int m_It = 0;
		float[] m_Samples;
		float[] m_SamplesSecs;

		// The last computed value
		float m_Fps = 0;
		float m_MSecs = 0;


		void Awake ()
		{
			// Init samples
			m_Samples = new float[m_RefreshRate];
			m_SamplesSecs = new float[m_RefreshRate];
			for (int i = 0; i < m_Samples.Length; ++i) {
				m_Samples [i] = 0;
				m_SamplesSecs [i] = 0;
			}
		}

		void Update ()
		{
			// Compute frames per seconds
			m_DeltaTime += (Time.deltaTime - m_DeltaTime) * 0.1f;		
			float msec = m_DeltaTime * 1000.0f;
			float fps = 1.0f / m_DeltaTime;

			// Record the sample
			m_Samples [m_It] = fps;
			m_SamplesSecs [m_It] = msec;
			m_It++;

			// Compute fps mean
			if (m_It >= m_Samples.Length) {
				m_It = 0;
				m_Fps = 0;
				m_MSecs = 0;
				for (int i = 0; i < m_Samples.Length; ++i) {
					m_Fps += m_Samples [i];
					m_MSecs += m_SamplesSecs [i];
				}
				m_Fps /= (float)m_Samples.Length;
				m_MSecs /= (float)m_Samples.Length;
			}
			m_Text.text = "FPS: " + (int)m_Fps;
		}

	}

}