using UnityEngine;

namespace MegaFiers
{
	public class SpinObject : MonoBehaviour
	{
		public MegaAxis	axis	= MegaAxis.Y;
		public float	speed	= 30.0f;
		public float	angle;
		Vector3			ang;

		private void Awake()
		{
		}

		void Start()
		{
			ang = transform.localEulerAngles;
		}

		void Update()
		{
			angle += Time.deltaTime * speed;
			angle = Mathf.Repeat(angle, 360.0f);
			Vector3 r = ang;
			r[(int)axis] += angle;

			transform.localEulerAngles = r;
		}
	}
}