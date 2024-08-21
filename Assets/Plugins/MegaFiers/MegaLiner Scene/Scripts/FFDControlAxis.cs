using UnityEngine;
using MegaFiers;

// Add this to an object with an FFD3x3x3 modifier and it will move the top of the object towards a target object with a spring effect also making the object
// stretch and squash
public class FFDControlAxis : MonoBehaviour
{
	public MegaFFD3x3x3		ffd;						// ffd3x3x3 to control
	public MegaAxis			axis		= MegaAxis.Y;	// Up Axis, deformation will happen along this axis
	public AnimationCurve	squish		= new AnimationCurve(new Keyframe(0.0f, 4.0f), new Keyframe(1, 1), new Keyframe(2, 0.5f));	// Controls the squash and stretch amounts
	public Transform		targetObj;					// Object to move towards
	[Range(0.01f, 1.0f)]
	public float			tau			= 0.25f;		// Spring rate, lower values gives faster movement
	[Range(0.0f, 1.0f)]
	public float			critical	= 1.0f;			// Damping for spring, lower values will mean less damping
	[Range(1, 5)]
	public int				rate		= 1;			// Increase this value to make the spring more snappy
	public bool				active		= false;		// Set to true to move to target object clear to release
	public float			minDistance	= 0.0f;			// Min distance between top and bottom objects we will squash to
	public float			maxDistance	= 40.0f;		// Max distance between top and bottom objects we will stretch to
	public bool				bezLerp		= false;		// Use Bezier lerp for middle object, can give nice bends
	[Range(0.0f, 1.0f)]
	public float			tensionBot	= 0.5f;			// Controls the tightness of the bezier bend
	[Range(0.0f, 1.0f)]
	public float			tensionTop	= 0.5f;			// Controls the tightness of the bezier bend
	public Transform		top;						// Top object transform, can be left blank
	public Transform		middle;						// Middle object transform can be left blank
	public Transform		bottom;						// Bottom object transform can be left blank
	float					startDist;
	Vector3					vel;
	Vector3					topstartpos;
	Vector3					tpos;
	Vector3[]				latticeStart;
	Vector3[]				toppoints;
	Vector3[]				midpoints;
	Vector3[]				botpoints;

	void Start()
    {
		if ( !ffd )
			ffd = GetComponent<MegaFFD3x3x3>();

		if ( ffd )
		{
			if ( !bottom )
			{
				bottom = new GameObject("Bottom").transform;
				bottom.parent = transform;
				switch ( axis )
				{
					case MegaAxis.X: bottom.position = ffd.GetPointWorld(0, 1, 1); break;
					case MegaAxis.Y: bottom.position = ffd.GetPointWorld(1, 0, 1); break;
					case MegaAxis.Z: bottom.position = ffd.GetPointWorld(1, 1, 0); break;
				}

				bottom.localRotation = Quaternion.identity;
			}

			if ( !middle )
			{
				middle = new GameObject("Middle").transform;
				middle.parent = transform;
				middle.position = ffd.GetPointWorld(1, 1, 1);
				switch ( axis )
				{
					case MegaAxis.X: middle.position = ffd.GetPointWorld(1, 1, 1); break;
					case MegaAxis.Y: middle.position = ffd.GetPointWorld(1, 1, 1); break;
					case MegaAxis.Z: middle.position = ffd.GetPointWorld(1, 1, 1); break;
				}

				middle.localRotation = Quaternion.identity;
			}

			if ( !top )
			{
				top = new GameObject("Top").transform;
				top.parent = transform;
				top.position = ffd.GetPointWorld(1, 2, 1);

				switch ( axis )
				{
					case MegaAxis.X: top.position = ffd.GetPointWorld(2, 1, 1); break;
					case MegaAxis.Y: top.position = ffd.GetPointWorld(1, 2, 1); break;
					case MegaAxis.Z: top.position = ffd.GetPointWorld(1, 1, 2); break;
				}

				top.localRotation = Quaternion.identity;
			}

			topstartpos = top.position;
			startDist = Vector3.Distance(bottom.position, top.position);

			latticeStart = new Vector3[ffd.NumPoints()];

			int ffdsize = ffd.GridSize();
			int count = ffdsize * ffdsize;

			toppoints = new Vector3[count];
			midpoints = new Vector3[count];
			botpoints = new Vector3[count];

			switch ( axis )
			{
				case MegaAxis.X: top.right = (top.position - bottom.position).normalized; break;
				case MegaAxis.Y: top.up = (top.position - bottom.position).normalized; break;
				case MegaAxis.Z: top.forward = (top.position - bottom.position).normalized; break;
			}
			middle.rotation = Quaternion.Slerp(bottom.rotation, top.rotation, 0.5f);

			for ( int i = 0; i < ffd.NumPoints(); i++ )
				latticeStart[i] = ffd.GetPointWorld(i);

			int index = 0;
			for ( int x = 0; x < ffdsize; x++ )
			{
				for ( int z = 0; z < ffdsize; z++ )
				{
					switch ( axis )
					{
						case MegaAxis.X: toppoints[index++] = top.InverseTransformPoint(ffd.GetPointWorld(2, x, z)); break;
						case MegaAxis.Y: toppoints[index++] = top.InverseTransformPoint(ffd.GetPointWorld(x, 2, z)); break;
						case MegaAxis.Z: toppoints[index++] = top.InverseTransformPoint(ffd.GetPointWorld(x, z, 2)); break;
					}
				}
			}

			index = 0;
			for ( int x = 0; x < ffdsize; x++ )
			{
				for ( int z = 0; z < ffdsize; z++ )
				{
					switch ( axis )
					{
						case MegaAxis.X: midpoints[index++] = middle.InverseTransformPoint(ffd.GetPointWorld(1, x, z)); break;
						case MegaAxis.Y: midpoints[index++] = middle.InverseTransformPoint(ffd.GetPointWorld(x, 1, z)); break;
						case MegaAxis.Z: midpoints[index++] = middle.InverseTransformPoint(ffd.GetPointWorld(x, z, 1)); break;
					}
				}
			}

			index = 0;
			for ( int x = 0; x < ffdsize; x++ )
			{
				for ( int z = 0; z < ffdsize; z++ )
				{
					switch ( axis )
					{
						case MegaAxis.X: botpoints[index++] = bottom.InverseTransformPoint(ffd.GetPointWorld(0, x, z)); break;
						case MegaAxis.Y: botpoints[index++] = bottom.InverseTransformPoint(ffd.GetPointWorld(x, 0, z)); break;
						case MegaAxis.Z: botpoints[index++] = bottom.InverseTransformPoint(ffd.GetPointWorld(x, z, 0)); break;
					}
				}
			}

			if ( active )
			{
				tpos = targetObj.position;
			}
		}
	}

	void Update()
	{
		if ( ffd )
		{
			if ( Input.GetMouseButton(0) || active )
				tpos = targetObj.position;
			else
				tpos = topstartpos;

			float stretch = Vector3.Distance(bottom.position, top.position) / startDist;
			stretch = squish.Evaluate(stretch);

			// Rotate top node
			switch ( axis )
			{
				case MegaAxis.X: top.right = (top.position - bottom.position).normalized;	break;
				case MegaAxis.Y: top.up = (top.position - bottom.position).normalized;		break;
				case MegaAxis.Z: top.forward = (top.position - bottom.position).normalized;	break;
			}

			if ( bezLerp )
			{
				Vector3 t1 = Vector3.zero;
				Vector3 t2 = Vector3.zero;

				switch ( axis )
				{
					case MegaAxis.X:
						t1 = bottom.position + (bottom.right * tensionBot * startDist * 0.5f);
						t2 = top.position - (top.right * tensionTop * startDist * 0.5f);
						break;
					case MegaAxis.Y:
						t1 = bottom.position + (bottom.up * tensionBot * startDist * 0.5f);
						t2 = top.position - (top.up * tensionTop * startDist * 0.5f);
						break;
					case MegaAxis.Z:
						t1 = bottom.position + (bottom.forward * tensionBot * startDist * 0.5f);
						t2 = top.position - (top.forward * tensionTop * startDist * 0.5f);
						break;
				}

				middle.position = cubeBezier3(bottom.position, t1, t2, top.position, 0.5f);
			}
			else
			{
				middle.position = Vector3.Lerp(bottom.position, top.position, 0.5f);
			}

			middle.rotation = Quaternion.Slerp(bottom.rotation, top.rotation, 0.5f);

			int ffdsize = ffd.GridSize();

			int index = 0;
			for ( int x = 0; x < ffdsize; x++ )
			{
				for ( int z = 0; z < ffdsize; z++ )
				{
					switch ( axis )
					{
						case MegaAxis.X: ffd.SetPointWorld(1, x, z, middle.TransformPoint(midpoints[index++] * stretch)); break;
						case MegaAxis.Y: ffd.SetPointWorld(x, 1, z, middle.TransformPoint(midpoints[index++] * stretch)); break;
						case MegaAxis.Z: ffd.SetPointWorld(x, z, 1, middle.TransformPoint(midpoints[index++] * stretch)); break;
					}
				}
			}

			index = 0;
			for ( int x = 0; x < ffdsize; x++ )
			{
				for ( int z = 0; z < ffdsize; z++ )
				{
					switch ( axis )
					{
						case MegaAxis.X: ffd.SetPointWorld(2, x, z, top.TransformPoint(toppoints[index++])); break;
						case MegaAxis.Y: ffd.SetPointWorld(x, 2, z, top.TransformPoint(toppoints[index++])); break;
						case MegaAxis.Z: ffd.SetPointWorld(x, z, 2, top.TransformPoint(toppoints[index++])); break;
					}
				}
			}

			index = 0;
			for ( int x = 0; x < ffdsize; x++ )
			{
				for ( int z = 0; z < ffdsize; z++ )
				{
					switch ( axis )
					{
						case MegaAxis.X: ffd.SetPointWorld(0, x, z, bottom.TransformPoint(botpoints[index++])); break;
						case MegaAxis.Y: ffd.SetPointWorld(x, 0, z, bottom.TransformPoint(botpoints[index++])); break;
						case MegaAxis.Z: ffd.SetPointWorld(x, z, 0, bottom.TransformPoint(botpoints[index++])); break;
					}
				}
			}
		}
	}

	void FixedUpdate()
	{
		if ( ffd )
		{
			for ( int i = 0; i < rate; i++ )
			{
				top.position = SpringDamp(top.position, tpos, ref vel, Time.deltaTime, tau, critical);
				Vector3 lpos = bottom.InverseTransformPoint(top.position);

				if ( lpos[(int)axis] < minDistance )
				{
					lpos[(int)axis] = minDistance;
					top.position = bottom.TransformPoint(lpos);
					vel = Vector3.zero;
				}

				if ( lpos[(int)axis] > maxDistance )
				{
					lpos[(int)axis] = maxDistance;
					top.position = bottom.TransformPoint(lpos);
					vel = Vector3.zero;
				}
			}
		}
	}

	static public Vector3 SpringDamp(Vector3 curr, Vector3 trg, ref Vector3 velocity, float time, float tau, float critical)
	{
		Vector3 Force = -1.0f / (tau * tau) * (curr - trg) - critical * 2.0f / tau * velocity;
		velocity = velocity + (Force * time);
		return curr += velocity * time;
	}

	public static Vector3 cubeBezier3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		return (((-p0 + 3.0f * (p1 - p2) + p3) * t + (3.0f * (p0 + p2) - 6.0f * p1)) * t + 3.0f * (p1 - p0)) * t + p0;
	}
}