using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterMovement : MonoBehaviour {

	public float turnSmoothing = 25f;	
	public float moveSpeed = 8f;	

	void Update ()
	{
		// Cache the inputs.
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		MovementManagement(h, v);
	}

	void MovementManagement (float horizontal, float vertical)
	{
		if(horizontal != 0f || vertical != 0f)
		{
			Rotating(horizontal, vertical);
		}

		Vector3 dir = new Vector3(horizontal, 0f, vertical);
		if (dir.magnitude > 1f) {
			dir.Normalize();
		}

		Vector3 move = dir * moveSpeed + Physics.gravity;
		move *= Time.deltaTime;

		GetComponent<CharacterController>().Move(move);
	}

	void Rotating (float horizontal, float vertical)
	{
		// Create a new vector of the horizontal and vertical inputs.
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);

		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);

		// Change the players rotation to this new rotation.
		transform.rotation = newRotation;
	}
}
