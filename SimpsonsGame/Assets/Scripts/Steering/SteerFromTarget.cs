using UnityEngine;
using System.Collections;
using System;

public class SteerFromTarget : SteeringBehaviour {
	
	public Transform target;
	public Transform self;
	public float MinDistance = 10;

	private float _MinDistSqr;
	
	public SteerFromTarget(Transform s, Transform t) {

		self = s;
		target = t;

		_MinDistSqr = MinDistance * MinDistance;
	}

	public override Vector3 GetSteering() {
		
		Vector3 vecFromTarget = self.position - target.position;

		if (vecFromTarget.sqrMagnitude > _MinDistSqr) {
			
			return Vector3.zero;
		}
		
		return vecFromTarget.normalized;
	}
}
