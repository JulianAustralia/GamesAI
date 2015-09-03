using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class BartKeyboardMovement : MonoBehaviour {

	private Movement _movement;

	protected void Awake(){

		_movement = GetComponent<Movement>();
	}

	protected void Update () {

		_movement.Rotate(
			0,
			Input.GetAxis("Horizontal"),
			0
		);

		_movement.MoveForward(
			Input.GetAxis("Vertical")
		);
	}
}
