using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	[InitializeOnLoad]
	[CustomEditor(typeof(MegaRopeDeform))]
	public class MegaRopeDeformEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Rope Deform Modifier by Chris West"; }

		bool showsoft = false;

		public override bool Inspector()
		{
			MegaRopeDeform mod = (MegaRopeDeform)target;

			if ( GUILayout.Button("Rebuild") )
				mod.init = true;

			MegaEditorGUILayout.Float(target, "Floor Off", ref mod.floorOff);
			MegaEditorGUILayout.Int(target, "Num Masses", ref mod.NumMasses);
			if ( mod.NumMasses < 2 )
				mod.NumMasses = 2;

			MegaEditorGUILayout.Float(target, "Mass", ref mod.Mass);
			if ( mod.Mass < 0.01f )
				mod.Mass = 0.01f;

			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);

			MegaEditorGUILayout.Curve(target, "Stiffness Crv", ref mod.stiffnessCrv);
			MegaEditorGUILayout.Float(target, "Stiff Spring", ref mod.stiffspring);
			MegaEditorGUILayout.Float(target, "Stiff Damp", ref mod.stiffdamp);
			MegaEditorGUILayout.Float(target, "Spring", ref mod.spring);
			MegaEditorGUILayout.Float(target, "Damp", ref mod.damp);
			MegaEditorGUILayout.Float(target, "Off", ref mod.off);
			MegaEditorGUILayout.Float(target, "Spring Compress", ref mod.SpringCompress);
			MegaEditorGUILayout.Toggle(target, "Bend Springs", ref mod.BendSprings);
			MegaEditorGUILayout.Toggle(target, "Constraints", ref mod.Constraints);
			MegaEditorGUILayout.Float(target, "Damping Ratio", ref mod.DampingRatio);
			MegaEditorGUILayout.Transform(target, "Left", ref mod.left, true);
			MegaEditorGUILayout.Transform(target, "Right", ref mod.right, true);
			MegaEditorGUILayout.Float(target, "Weight", ref mod.weight);
			MegaEditorGUILayout.Float(target, "Weight Pos", ref mod.weightPos);

			showsoft = EditorGUILayout.Foldout(showsoft, "Physics");

			if ( showsoft )
			{
				MegaEditorGUILayout.Slider(target, "Time Step", ref mod.soft.timeStep, 0.001f, 0.2f);
				MegaEditorGUILayout.Vector2(target, "Gravity", ref mod.soft.gravity);
				MegaEditorGUILayout.Float(target, "Air Drag", ref mod.soft.airdrag);
				MegaEditorGUILayout.Float(target, "Friction", ref mod.soft.friction);
				MegaEditorGUILayout.Int(target, "Iterations", ref mod.soft.iters);
				MegaEditorGUILayout.Integrator(target, "Method", ref mod.soft.method);
				MegaEditorGUILayout.Toggle(target, "Apply Constraints", ref mod.soft.applyConstraints);
			}

			MegaEditorGUILayout.BeginToggle(target, "Display Debug", ref mod.DisplayDebug);
			MegaEditorGUILayout.Int(target, "Draw Steps", ref mod.drawsteps);
			MegaEditorGUILayout.Float(target, "Box Size", ref mod.boxsize);
			MegaEditorGUILayout.EndToggle();

			return false;
		}

		static void Update()
		{
#if UNITY_2023_1_OR_NEWER
			MegaModifyObject[] mods = (MegaModifyObject[])GameObject.FindObjectsByType(typeof(MegaModifyObject), FindObjectsSortMode.None);
#else
			MegaModifyObject[] mods = (MegaModifyObject[])FindObjectsOfType(typeof(MegaModifyObject));
#endif

			for ( int i = 0; i < mods.Length; i++ )
				mods[i].ModifyObject();
		}
	}
}