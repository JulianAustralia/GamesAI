using UnityEngine;
using System.Collections;
using System;

public class SteerWanderXZ : SteeringBehaviour {

	public float Speed = 10f;
	private float angle;

	protected void Awake() {

		angle = _newFloat((float) (2 * Math.PI));
	}
	
	public override Vector3 GetSteering() {

		float maxAdjustment = Speed * Time.deltaTime;

		angle += _newFloat(maxAdjustment);

		return new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
	}

	private float _newFloat(float minMax) {

		return UnityEngine.Random.Range(-minMax, minMax);
	}
}
