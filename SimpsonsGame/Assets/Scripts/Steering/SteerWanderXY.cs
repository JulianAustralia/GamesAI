using UnityEngine;
using System.Collections;
using System;

public class SteerWanderXZ : SteeringBehaviour {

	public float maxChange = .1f;
	public Transform self;
	private float _angle;

	public SteerWanderXZ(Transform s) {

		self = s;

		_angle = self.rotation.eulerAngles.y * Mathf.PI / 180f;
	}

	public override Vector3 GetSteering() {

		_angle += UnityEngine.Random.Range(-30, 30) * Time.deltaTime;

		return new Vector3(Mathf.Sin(_angle), 0, Mathf.Cos(_angle));
	}
}
