
namespace MegaFiers
{
	// Save setting option, 
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
	public class Adjust : System.Attribute
	{
		public string name = null;
		public float min = 0f;
		public float max = 0f;
		public float step = 0.1f;
		public bool	save = true;

		public Adjust(string name)
		{
			this.name = name;
		}

		public Adjust(float min, float max, float step = 0.1f)
		{
			this.min = min;
			this.max = max;
			this.step = step;
		}

		public Adjust(string name, float min, float max, float step = 0.1f)
		{
			this.name = name;
			this.min = min;
			this.max = max;
			this.step = step;
		}

		public Adjust(string name, float min, float max, float step = 0.1f, bool save = true)
		{
			this.name = name;
			this.min = min;
			this.max = max;
			this.step = step;
			this.save = save;
		}

		public Adjust() { }
	}
}