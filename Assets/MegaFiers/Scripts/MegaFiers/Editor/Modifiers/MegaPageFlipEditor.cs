using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaPageFlip))]
	public class MegaPageFlipEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Page Flip Modifier by Chris West"; }

		bool advanced = false;

		public override bool Inspector()
		{
			MegaPageFlip mod = (MegaPageFlip)target;

			MegaEditorGUILayout.Float(target, "Turn", ref mod.turn);
			MegaEditorGUILayout.Float(target, "Ap1", ref mod.ap1);
			MegaEditorGUILayout.Float(target, "Ap2", ref mod.ap2);
			MegaEditorGUILayout.Float(target, "Ap3", ref mod.ap3);
			MegaEditorGUILayout.Toggle(target, "Flip X", ref mod.flipx);

			advanced = EditorGUILayout.Foldout(advanced, "Advanced");
			if ( advanced )
			{
				MegaEditorGUILayout.Toggle(target, "Anim T", ref mod.animT);
				MegaEditorGUILayout.Toggle(target, "Auto Mode", ref mod.autoMode);
				MegaEditorGUILayout.Toggle(target, "Lock Rho", ref mod.lockRho);
				MegaEditorGUILayout.Toggle(target, "Lock Theta", ref mod.lockTheta);
				MegaEditorGUILayout.Float(target, "TimeStep", ref mod.timeStep);
				MegaEditorGUILayout.Float(target, "Rho", ref mod.rho);
				MegaEditorGUILayout.Float(target, "Theta", ref mod.theta);
				MegaEditorGUILayout.Float(target, "DeltaT", ref mod.deltaT);
				MegaEditorGUILayout.Float(target, "kT", ref mod.kT);

			}
			return false;
		}
	}
}