using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaFFD3x3))]
	public class MegaFFD3x3Editor : MegaFFD2x2Editor
	{
		public override string GetHelpString() { return "FFD3x3 Modifier by Chris West"; }
	}
}