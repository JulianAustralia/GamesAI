using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class KeyboardMovement : MonoBehaviour {

	private Movement _movement;

	protected void Awake(){
		_movement = GetComponent<Movement> ();
	}

	protected void Update () {
		var targetVelocity = new Vector3 (Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		_movement.Move (targetVelocity);
	}
}
