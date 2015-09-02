using UnityEngine;
using System.Collections;
using System;

public class SteerToTarget : SteeringBehaviour {
	
	public Transform target;
	public float MaxForce = 2;
	public float MinDistance = 3;

	private float _MinDistSqr;

	protected void Awake() {
		
		_MinDistSqr = MinDistance * MinDistance;
	}
	
	public override Vector3 GetSteering() {
		
		Vector3 vecToTarget = target.position - transform.position;

		if (vecToTarget.sqrMagnitude < _MinDistSqr) {
			
			return Vector3.zero;
		}
		
		return Vector3.ClampMagnitude(vecToTarget, MaxForce);
	}
}
