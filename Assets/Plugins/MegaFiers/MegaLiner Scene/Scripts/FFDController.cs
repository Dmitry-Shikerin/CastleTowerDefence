using UnityEngine;

// Add this to an object with an FFD3x3x3 modifier and it will move the top of the object towards a target object with a spring effect also making the object
// stretch and squash
namespace MegaFiers
{
	public class FFDController : MonoBehaviour
	{
		public MegaFFD3x3x3		ffd;					// ffd3x3x3 to control
		public AnimationCurve	squish		= new AnimationCurve(new Keyframe(0.0f, 4.0f), new Keyframe(1, 1), new Keyframe(2, 0.5f));	// Controls the squash and stretch amounts
		public Transform		targetObj;				// Object to move towards
		[Range(0.01f, 1.0f)]
		public float			tau			= 0.25f;	// Spring rate, lower values gives faster movement
		[Range(0.0f, 1.0f)]
		public float			critical	= 1.0f;		// Damping for spring, lower values will mean less damping
		[Range(1, 5)]
		public int				rate		= 1;		// Increase this value to make the spring more snappy
		public bool				active		= false;	// Set to true to move to target object clear to release
		public bool				disableSpring	= false;	// Turns of the spring system
		public float			minDistance	= 0.0f;		// Min distance between top and bottom objects we will squash to
		public float			maxDistance	= 40.0f;	// Max distance between top and bottom objects we will stretch to
		public bool				bezLerp		= false;	// Use Bezier lerp for middle object, can give nice bends
		[Range(0.0f, 1.0f)]
		public float			tensionBot	= 0.5f;		// Controls the tightness of the bezier bend
		[Range(0.0f, 1.0f)]
		public float			tensionTop	= 0.5f;		// Controls the tightness of the bezier bend
		public Transform		top;					// Top object transform, can be left blank
		public Transform		middle;					// Middle object transform can be left blank
		public Transform		bottom;					// Bottom object transform can be left blank
		float					startDist;
		Vector3					vel;
		Vector3					topstartpos;
		Vector3					tpos;
		Vector3[]				latticeStart;
		Vector3[]				toppoints;
		Vector3[]				midpoints;


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
					bottom.position = ffd.GetPointWorld(1, 0, 1);
				}

				if ( !middle )
				{
					middle = new GameObject("Middle").transform;
					middle.parent = transform;
					middle.position = ffd.GetPointWorld(1, 1, 1);
				}

				if ( !top )
				{
					top = new GameObject("Top").transform;
					top.parent = transform;
					top.position = ffd.GetPointWorld(1, 2, 1);
				}

				topstartpos = top.position;
				startDist = Vector3.Distance(bottom.position, top.position);

				latticeStart = new Vector3[ffd.NumPoints()];

				int ffdsize = ffd.GridSize();
				int count = ffdsize * ffdsize;

				toppoints = new Vector3[count];
				midpoints = new Vector3[count];

				for ( int i = 0; i < ffd.NumPoints(); i++ )
					latticeStart[i] = ffd.GetPointWorld(i);

				int index = 0;
				for ( int x = 0; x < ffdsize; x++ )
				{
					for ( int z = 0; z < ffdsize; z++ )
						toppoints[index++] = top.InverseTransformPoint(ffd.GetPointWorld(x, 2, z));
				}

				index = 0;
				for ( int x = 0; x < ffdsize; x++ )
				{
					for ( int z = 0; z < ffdsize; z++ )
						midpoints[index++] = middle.InverseTransformPoint(ffd.GetPointWorld(x, 1, z));
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
				top.up = (top.position - bottom.position).normalized;
				if ( bezLerp )
				{
					Vector3 t1 = bottom.position + (bottom.up * tensionBot * startDist * 0.5f);
					Vector3 t2 = top.position - (top.up * tensionTop * startDist * 0.5f);
					middle.position = cubeBezier3(bottom.position, t1, t2, top.position, 0.5f);
				}
				else
					middle.position = Vector3.Lerp(bottom.position, top.position, 0.5f);
				middle.rotation = Quaternion.Slerp(bottom.rotation, top.rotation, 0.5f);

				int ffdsize = ffd.GridSize();

				int index = 0;
				for ( int x = 0; x < ffdsize; x++ )
				{
					for ( int z = 0; z < ffdsize; z++ )
						ffd.SetPointWorld(x, 1, z, middle.TransformPoint(midpoints[index++] * stretch));
				}

				index = 0;
				for ( int x = 0; x < ffdsize; x++ )
				{
					for ( int z = 0; z < ffdsize; z++ )
						ffd.SetPointWorld(x, 2, z, top.TransformPoint(toppoints[index++]));
				}
			}
		}

		void FixedUpdate()
		{
			if ( ffd )
			{
				if ( disableSpring && targetObj )
				{
					top.position = targetObj.position;
				}
				else
				{
					for ( int i = 0; i < rate; i++ )
					{
						top.position = SpringDamp(top.position, tpos, ref vel, Time.deltaTime, tau, critical);

						Vector3 lpos = bottom.InverseTransformPoint(top.position);
						if ( lpos.y < minDistance )
						{
							lpos.y = minDistance;
							top.position = bottom.TransformPoint(lpos);
							vel = Vector3.zero;
						}

						if ( lpos.y > maxDistance )
						{
							lpos.y = maxDistance;
							top.position = bottom.TransformPoint(lpos);
							vel = Vector3.zero;
						}
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
			return (((-p0 + 3 * (p1 - p2) + p3) * t + (3 * (p0 + p2) - 6 * p1)) * t + 3 * (p1 - p0)) * t + p0;
		}
	}
}