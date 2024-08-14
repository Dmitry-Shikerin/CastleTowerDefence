using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{

	public Transform m_Target;
	public float slerpSpeed = 5f;

	Vector3 m_DefaultOffset;

	// Use this for initialization
	void Start()
	{
		m_DefaultOffset = transform.position - m_Target.transform.position;
	}
	
	// Update is called once per frame
	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, m_Target.transform.position + m_DefaultOffset, Time.deltaTime * slerpSpeed);
	}
}
