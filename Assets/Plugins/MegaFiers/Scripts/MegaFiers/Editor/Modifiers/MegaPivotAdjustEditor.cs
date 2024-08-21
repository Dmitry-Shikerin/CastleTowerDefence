using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaPivotAdjust))]
	public class MegaPivotAdjustEditor : MegaModifierEditor
	{
		public override bool Inspector()
		{
			return false;
		}
	}
}