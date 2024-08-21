using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaPathDeform))]
	public class MegaPathDeformEditor : MegaModifierEditor
	{
		public override bool Inspector()
		{
			MegaPathDeform mod = (MegaPathDeform)target;

			MegaEditorGUILayout.Toggle(target, "Use Distance", ref mod.usedist);

			if ( mod.usedist )
				MegaEditorGUILayout.Float(target, "Distance", ref mod.distance);
			else
				MegaEditorGUILayout.Float(target, "Percent", ref mod.percent);

			MegaEditorGUILayout.Float(target, "Stretch", ref mod.stretch);
			MegaEditorGUILayout.Float(target, "Twist", ref mod.twist);
			MegaEditorGUILayout.Float(target, "Rotate", ref mod.rotate);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.Toggle(target, "Flip", ref mod.flip);
			MegaEditorGUILayout.ShapeObject(target, "Path", ref mod.path, true);

			if ( mod.path != null && mod.path.splines.Count > 1 )
			{
				MegaEditorGUILayout.Int(target, "Curve", ref mod.curve, 0, mod.path.splines.Count - 1);
				mod.curve = Mathf.Clamp(mod.curve, 0, mod.path.splines.Count - 1);
			}

			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.speed);
			MegaEditorGUILayout.LoopMode(target, "Loop Mode", ref mod.loopmode);
			MegaEditorGUILayout.Toggle(target, "Draw Path", ref mod.drawpath);
			MegaEditorGUILayout.Float(target, "Tangent", ref mod.tangent);
			MegaEditorGUILayout.Toggle(target, "Use Twist Curve", ref mod.UseStretchCurve);
			MegaEditorGUILayout.Curve(target, "Twist Curve", ref mod.twistCurve);
			MegaEditorGUILayout.Toggle(target, "Use Stretch Curve", ref mod.UseStretchCurve);
			MegaEditorGUILayout.Curve(target, "Stretch Curve", ref mod.stretchCurve);
			MegaEditorGUILayout.Vector3(target, "Up", ref mod.Up);

			return false;
		}

		void Display(MegaPathDeform pd)
		{
			if ( pd.path != null )
			{
				Matrix4x4 mat = pd.transform.localToWorldMatrix * pd.path.transform.localToWorldMatrix * pd.mat;

				for ( int s = 0; s < pd.path.splines.Count; s++ )
				{
					float ldist = pd.path.stepdist;
					if ( ldist < 0.1f )
						ldist = 0.1f;

					float ds = pd.path.splines[s].length / (pd.path.splines[s].length / ldist);

					int c	= 0;
					int k	= -1;
					int lk	= -1;

					Vector3 first = pd.path.splines[s].Interpolate(0.0f, pd.path.normalizedInterp, ref lk);

					for ( float dist = ds; dist < pd.path.splines[s].length; dist += ds )
					{
						float alpha = dist / pd.path.splines[s].length;
						Vector3 pos = pd.path.splines[s].Interpolate(alpha, pd.path.normalizedInterp, ref k);

						if ( k != lk )
						{
							for ( lk = lk + 1; lk <= k; lk++ )
							{
								Handles.DrawLine(mat.MultiplyPoint(first), mat.MultiplyPoint(pd.path.splines[s].knots[lk].p));
								first = pd.path.splines[s].knots[lk].p;
							}
						}

						lk = k;

						Handles.DrawLine(mat.MultiplyPoint(first), mat.MultiplyPoint(pos));

						c++;

						first = pos;
					}

					if ( pd.path.splines[s].closed )
					{
						Vector3 pos = pd.path.splines[s].Interpolate(0.0f, pd.path.normalizedInterp, ref k);
						Handles.DrawLine(mat.MultiplyPoint(first), mat.MultiplyPoint(pos));
					}
				}
			}
		}
	}
}