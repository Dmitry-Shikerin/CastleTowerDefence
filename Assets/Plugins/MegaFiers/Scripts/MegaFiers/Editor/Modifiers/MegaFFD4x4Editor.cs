using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaFFD4x4))]
	public class MegaFFD4x4Editor : MegaFFD2x2Editor
	{
		public override string GetHelpString() { return "FFD4x4 Modifier by Chris West"; }
	}
}