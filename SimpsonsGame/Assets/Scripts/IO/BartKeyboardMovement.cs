using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class BartKeyboardMovement : MonoBehaviour {

	private Movement _movement;

	protected void Awake(){

		_movement = GetComponent<Movement>();
	}

	protected void Update () {

		int forwardback = Input.GetKey(KeyCode.UpArrow) ? -1 : Input.GetKey(KeyCode.DownArrow) ? 1 : 0;
		int leftright = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;

		_movement.Rotate(0, -forwardback * leftright, 0);

		_movement.MoveForward(
			forwardback
		);
	}
}
