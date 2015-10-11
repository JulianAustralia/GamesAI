using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class BartKeyboardMovement : MonoBehaviour {

	private Movement _movement;

	protected void Awake(){

		_movement = GetComponent<Movement>();
	}

	protected void Update () {

		bool u = Input.GetKey(KeyCode.UpArrow);
		bool d = Input.GetKey(KeyCode.DownArrow);
		bool l = Input.GetKey(KeyCode.LeftArrow);
		bool r = Input.GetKey(KeyCode.RightArrow);

		float forwardback = 0;
		float leftright = 0;

		if (u) {

			if (!d) {

				forwardback = 1;
			}
		} else if (d) {

			forwardback = -1;
		}

		if (l) {

			if (!r) {

				leftright = -1;
			}
		} else if (r) {

			leftright = 1;
		}

		// If forwardback is negative then invert leftright to make Bart behave like 4wheeled vehicle
		if (forwardback < 0) {

			leftright = -leftright;
		}

		_movement.Rotate(leftright);

		_movement.MoveForward(
			forwardback
		);
	}
}
