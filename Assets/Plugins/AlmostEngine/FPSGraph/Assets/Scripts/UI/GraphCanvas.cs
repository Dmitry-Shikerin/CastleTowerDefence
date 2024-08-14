using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlmostEngine
{
	public class GraphCanvas : MonoBehaviour
	{
		public Text m_Title;
		public RectTransform m_AbscissaPanel;
		public RectTransform m_OrdinatePanel;
		public RectTransform m_ValuesPanel;

		public GameObject m_TimeStepBarPrefab;
		public GameObject m_OrdinatePrefab;
		public GameObject m_AbscissaPrefab;


//		int m_OrdinateMin = 0;
//		int m_OrdinateMax = 60;
//		int m_AbscissaMin = 0;
//		int m_AbscissaMax = 60;

		int[] m_Abscissas;
		int[] m_Ordinates;




		public void Init()
		{
			
		}

		public void AddOrdinatePanel(int y)
		{
		}

		public void AbscissaPanel(int x)
		{
		}


//		List<GameObject> m_Bars = new List<GameObject> ();


		Color GetColor (int val)
		{

			// Change color
			if (val < 10) {
				return Color.red;
			} else if (val < 30) {
				return new Color (1f, 0.4f, 0f);
			} else if (val < 60) {
				return Color.yellow;
			} else {
				return Color.green;
			}
		}

	}
}