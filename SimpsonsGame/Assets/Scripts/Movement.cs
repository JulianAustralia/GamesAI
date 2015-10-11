using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GameObject))]
public class Movement : MonoBehaviour {

	public float Speed = 2;
	public float Accel = 10;

	private Rigidbody _rigidbody;

	protected void Reset() {

		_rigidbody = GetComponent<Rigidbody>();
	}

	protected void Awake() {

		_rigidbody = GetComponent<Rigidbody>();
	}

	public void Rotate(float yRot) {

		_rigidbody.transform.Rotate(0, yRot, 0);
	}

	public void Move(Vector3 targetVelocity) {
	
		Vector3 acceleration = ((targetVelocity.normalized * Speed) - _rigidbody.velocity) * Accel;

		if (acceleration.magnitude > Accel) {

			acceleration = acceleration.normalized * Accel;
		}

		_rigidbody.velocity += acceleration * Time.deltaTime;

		_rigidbody.transform.eulerAngles = new Vector3(
			0,
			_rigidbody.transform.eulerAngles.y,
			0
		);
	}

	public void MoveForward(float targetVelocity) {

		Move(
			_rigidbody.transform.forward * targetVelocity
		);

		_rigidbody.transform.eulerAngles = new Vector3(
			0,
			_rigidbody.transform.eulerAngles.y,
			0
		);
	}
}
