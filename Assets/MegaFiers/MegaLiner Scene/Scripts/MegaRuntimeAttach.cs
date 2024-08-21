using UnityEngine;
using Unity.Collections;

namespace MegaFiers
{
	// Example script Attaches object at every vertex on the the deforming mesh
	public class MegaRuntimeAttach : MonoBehaviour
	{
		public GameObject ExternalRecprtor;

		void Start()
		{
			NativeArray<Vector3> v = GetComponent<MegaModifyObject>().jsverts;
			for ( int i = 0; i < v.Length; i++ )
			{
				GameObject mag = new GameObject();
				mag.name = "Attach" + i;
				mag.transform.parent = gameObject.transform;

				mag.transform.position = transform.localToWorldMatrix.MultiplyPoint3x4(v[i]);

				MegaAttach ma = mag.AddComponent<MegaAttach>();
				ma.target = gameObject.GetComponent<MegaModifyObject>();
				ma.radius = 0.1f;

				mag.transform.position = transform.localToWorldMatrix.MultiplyPoint3x4(v[i]);
				ma.AttachIt();
				GameObject rec = (GameObject)Instantiate(ExternalRecprtor);
				rec.transform.parent = mag.transform;

				rec.transform.localPosition = Vector3.zero;
			}
		}
	}
}