using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
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

	public void Rotate(float xRot, float yRot, float zRot) {

		_rigidbody.transform.Rotate(xRot, yRot, zRot);
	}

	public void Move(Vector3 targetVelocity) {

		if (targetVelocity.sqrMagnitude > 1) {

			targetVelocity.Normalize();
		}

		targetVelocity *= Speed;

		Vector3 acceleration = (targetVelocity - _rigidbody.velocity) * Accel;

		if (acceleration.magnitude > Accel) {

			acceleration = acceleration.normalized * Accel;
		}

		_rigidbody.velocity += acceleration * Time.deltaTime;
	}

	public void MoveForward(float targetVelocity) {

		Move(
			_rigidbody.transform.forward * targetVelocity
		);
	}
}
