using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlmostEngine.Report
{
    public class FpsReportUI : MonoBehaviour
    {
        FPSCounter m_Report;

        public Text m_CurrentFPS;
        public Text m_MeanFPS;
        public Text m_MinFPS;
        public Text m_MaxFPS;

        public GameObject m_TimeStepBarPrefab;

        public Canvas m_Canvas;
        public RectTransform m_Container;
        public RectTransform m_BufferUI;
        public RectTransform m_StatsUI;

        public int m_LowFPSValue = 10;
        public int m_MedFPSValue = 30;
        public int m_HighFPSValue = 50;



        List<BarUI> m_BarUIs = new List<BarUI>();
        List<BarUI> m_StatsBarUIs = new List<BarUI>();


        public int m_MaxFPSTimeline = 144;

        public string display = "{0:F2} FPS";

        public class BarUI
        {
            public Image m_Image;
            public LayoutElement m_LayoutElement;
        }



        void Start()
        {
            m_Report = GameObject.FindObjectOfType<FPSCounter>();

            InitTimeline();
            InitHistogram();
        }

        void InitTimeline()
        {
            if (m_BufferUI.gameObject.activeInHierarchy == false)
                return;

            for (int i = 0; i < m_Report.m_BufferSize; ++i)
            {
                GameObject bar = GameObject.Instantiate(m_TimeStepBarPrefab);
                bar.transform.SetParent(m_BufferUI);
                BarUI barUi = new BarUI();
                barUi.m_Image = bar.GetComponent<Image>();
                barUi.m_LayoutElement = bar.GetComponent<LayoutElement>();
                bar.transform.localScale = Vector3.one;
                m_BarUIs.Add(barUi);
            }
        }

        void InitHistogram()
        {
            if (m_StatsUI.gameObject.activeInHierarchy == false)
                return;

            for (int i = 0; i < m_Report.m_MaxStat / m_Report.m_StatStep; ++i)
            {
                GameObject bar = GameObject.Instantiate(m_TimeStepBarPrefab);
                bar.transform.SetParent(m_StatsUI);
                BarUI barUi = new BarUI();
                barUi.m_Image = bar.GetComponent<Image>();
                barUi.m_LayoutElement = bar.GetComponent<LayoutElement>();
                bar.transform.localScale = Vector3.one;
                m_StatsBarUIs.Add(barUi);
            }
        }


        void Update()
        {
            UpdateTextUI(m_CurrentFPS, m_Report.m_Fps);
            UpdateTextUI(m_MeanFPS, (int)m_Report.m_MeanFps);
            UpdateTextUI(m_MinFPS, m_Report.m_MinFps);
            UpdateTextUI(m_MaxFPS, m_Report.m_MaxFps);
            UpdateTimelineUI();
            UpdateHistogramUI();
        }






        void UpdateTextUI(Text text, int val)
        {
            text.text = string.Format(display, val);
            if (text.color != GetColor(val))
            {
                text.color = GetColor(val);
            }
        }

        void UpdateTimelineUI()
        {
            if (m_BufferUI.gameObject.activeInHierarchy == false)
                return;

            for (int i = 0; i < m_Report.m_BufferSize; ++i)
            {
                BarUI bar = m_BarUIs[i];
                int val = m_Report.m_Values[(m_Report.m_CurrentId + i) % m_Report.m_BufferSize];
                bar.m_LayoutElement.preferredHeight = m_BufferUI.rect.height * (float)val / (float)m_MaxFPSTimeline;

                if (bar.m_Image.color != GetColor(val))
                {
                    bar.m_Image.color = GetColor(val);
                }
            }
        }

        void UpdateHistogramUI()
        {
            if (m_StatsUI.gameObject.activeInHierarchy == false)
                return;

            int max = 0;
            for (int i = 0; i < m_Report.m_MaxStat / m_Report.m_StatStep; ++i)
            {
                max = Mathf.Max(max, m_Report.m_Stats[i]);
            }


            for (int i = 0; i < m_Report.m_MaxStat / m_Report.m_StatStep; ++i)
            {
                BarUI bar = m_StatsBarUIs[i];
                int val = m_Report.m_Stats[i];
                bar.m_LayoutElement.preferredHeight = m_StatsUI.rect.height * val / max;

                if (bar.m_Image.color != GetColor(i * m_Report.m_StatStep))
                {
                    bar.m_Image.color = GetColor(i * m_Report.m_StatStep);
                }
            }
        }


        Color GetColor(int val)
        {

            // Change color
            if (val < m_LowFPSValue)
            {
                return Color.red;
            }
            else if (val < m_MedFPSValue)
            {
                return new Color(1f, 0.4f, 0f);
            }
            else if (val < m_HighFPSValue)
            {
                return Color.yellow;
            }
            else
            {
                return Color.green;
            }
        }
    }
}
