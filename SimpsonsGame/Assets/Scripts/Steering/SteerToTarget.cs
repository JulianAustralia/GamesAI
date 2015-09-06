using UnityEngine;
using System.Collections;
using System;

public class SteerToTarget : SteeringBehaviour {
	
	public Transform self;
	public Transform target;
	public float MinDistance = 1;

	private float _MinDistSqr;

	public SteerToTarget(Transform s, Transform t) {

		self = s;
		target = t;

		_MinDistSqr = MinDistance * MinDistance;
	}

	public override Vector3 GetSteering() {

		Vector3 vecToTarget = target.position - self.position;

		if (vecToTarget.sqrMagnitude < _MinDistSqr) {
			
			return Vector3.zero;
		}
		
		return vecToTarget.normalized;
	}
}
