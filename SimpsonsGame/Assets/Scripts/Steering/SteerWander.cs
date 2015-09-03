using UnityEngine;
using System.Collections;
using System;

public class SteerWander : SteeringBehaviour {

	protected void Awake() {

	}
	
	public override Vector3 GetSteering() {

		float x = 0;
		float y = 0;
		float z = 0;

		return new Vector3(x, y, z);
	}
}
