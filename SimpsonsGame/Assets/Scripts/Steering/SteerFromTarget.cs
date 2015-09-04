using UnityEngine;
using System.Collections;
using System;

public class SteerFromTarget : SteeringBehaviour {
	
	public Transform target;
	public Transform self;
	public float MaxForce = 2;
	public float MinDistance = 10;

	private float _MinDistSqr;
	
	protected void Awake() {
		
		_MinDistSqr = MinDistance * MinDistance;
	}

	public override void Initialize() {

	}
	
	public override Vector3 GetSteering() {
		
		Vector3 vecFromTarget = self.position - target.position;

		if (vecFromTarget.sqrMagnitude > _MinDistSqr) {
			
			return Vector3.zero;
		}
		
		return Vector3.ClampMagnitude(vecFromTarget, MaxForce);
	}
}
