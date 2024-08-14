using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlmostEngine.Report
{
    public class FPSCounter : MonoBehaviour
    {







        public FpsReportUI m_UI;

        public float m_UpdateInterval = 0.5f;

        [Header("Timeline")]
        public int m_Fps;
        public int m_MinFps = 9999;
        public int m_MaxFps = 1;
        public float m_MeanFps = 0f;
        public int m_BufferSize = 50;
        public int[] m_Values;

        [Header("Histogram")]
        public int m_StatStep = 10;
        public int m_MaxStat = 144;
        public int[] m_Stats;
        public int m_NbSamples = 0;

        [Header("Misc.")]
        public KeyCode m_Hotkey = KeyCode.F10;



        [HideInInspector]
        public bool m_Update = true;
        [HideInInspector]
        public int m_CurrentId = 0;
        [HideInInspector]
        public int m_FrameCount = 0;
        [HideInInspector]
        public float m_Dt = 0f;

        void Start()
        {

            InitBuffer();
            InitStats();
        }

        void InitBuffer()
        {
            m_Values = new int[m_BufferSize];
            for (int i = 0; i < m_BufferSize; ++i)
            {
                m_Values[i] = 0;
            }
        }

        void InitStats()
        {
            m_Stats = new int[m_MaxStat / m_StatStep];
            for (int i = 0; i < m_MaxStat / m_StatStep; ++i)
            {
                m_Stats[i] = 0;
            }
        }

        void Update()
        {
            if (m_Update)
            {
                UpdateFPS();
            }

            // Show/Hide
            if (Input.GetKeyDown(m_Hotkey))
            {
                Show(!m_Update);
            }
        }

        void Show(bool show)
        {
            m_Update = show;

            m_UI.m_Canvas.gameObject.SetActive(m_Update);
        }

        void UpdateFPS()
        {
            m_Dt += Time.deltaTime;
            m_FrameCount++;


            // measure average frames per second
            if (m_Dt >= m_UpdateInterval)
            {
                //				Debug.Log (string.Format ("{0} frames in {1} seconds", m_FrameCount, m_Dt));
                m_Fps = (int)(m_FrameCount / m_Dt);

                // Update stat
                SetCurrentFPS(m_Fps);

                // Reset
                m_Dt -= m_UpdateInterval;
                m_FrameCount = 0;


            }
        }

        int m_Nb = 0;

        void SetCurrentFPS(int val)
        {

            // Push in buffer
            m_Values[m_CurrentId] = m_Fps;
            m_CurrentId = (m_CurrentId + 1) % m_BufferSize;

            // Add stat
            m_Stats[Mathf.Min(val / m_StatStep, m_MaxStat / m_StatStep - 1)] = m_Stats[Mathf.Min(val / m_StatStep, m_MaxStat / m_StatStep - 1)] + 1;
            m_NbSamples++;

            // Update min/max
            m_MinFps = 9999;
            m_MaxFps = 1;
            for (int i = 0; i < m_BufferSize; ++i)
            {
                if (m_Values[i] == 0)
                    continue;
                m_MinFps = Mathf.Min(m_MinFps, m_Values[i]);
                m_MaxFps = Mathf.Max(m_MaxFps, m_Values[i]);
            }

            // Update Mean
            m_MeanFps = (m_MeanFps * (float)(m_Nb) + val) / (float)(m_Nb + 1);
            m_Nb++;
        }


    }
}
