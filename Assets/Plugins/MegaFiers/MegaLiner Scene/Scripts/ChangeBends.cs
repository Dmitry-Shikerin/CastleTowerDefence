using UnityEngine;

namespace MegaFiers
{
	public class ChangeBends : MonoBehaviour
	{
		MegaBend[]	bends;

		public float b1AngSpd = 1.0f;
		public float b1DirSpd = 1.0f;

		public float b2AngSpd = 1.0f;
		public float b2DirSpd = 1.0f;

		void Start()
		{
			bends = (MegaBend[])GetComponents<MegaBend>();
		}

		void Update()
		{
			bends[0].angle += b1AngSpd * Time.deltaTime;
			bends[0].angle = Mathf.Repeat(bends[0].angle, 360.0f);

			bends[0].dir += b1DirSpd * Time.deltaTime;
			bends[0].dir = Mathf.Repeat(bends[0].dir, 360.0f);

			bends[1].angle += b2AngSpd * Time.deltaTime;
			bends[1].angle = Mathf.Repeat(bends[1].angle, 360.0f);

			bends[1].dir += b2DirSpd * Time.deltaTime;
			bends[1].dir = Mathf.Repeat(bends[1].dir, 360.0f);
		}
	}
}