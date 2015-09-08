using UnityEngine;
using System.Collections;
using System;

public class SteerAvoidBuildings : SteeringBehaviour {

	public float distanceToAvoid = 1.5f;

	private Transform _self;
	private int _buildingMask;

	public SteerAvoidBuildings(Transform s) {

		_self = s;
		_buildingMask = 1 << LayerMask.NameToLayer("Building");
	}
	
	public override Vector3 GetSteering() {

		Vector3 forwardRight = new Vector3(Mathf.Sqrt(2), 0, Mathf.Sqrt(2));
		Vector3 forwardLeft = new Vector3(-Mathf.Sqrt(2), 0, Mathf.Sqrt(2));
		Vector3 backRight = new Vector3(Mathf.Sqrt(2), 0, -Mathf.Sqrt(2));
		Vector3 backLeft = new Vector3(-Mathf.Sqrt(2), 0, -Mathf.Sqrt(2));

		Vector3 vecResult = Vector3.zero;

		vecResult += _Raycast(Vector3.right);
		vecResult += _Raycast(forwardRight);
		vecResult += _Raycast(backRight);
		vecResult += _Raycast(Vector3.left);
		vecResult += _Raycast(forwardLeft);
		vecResult += _Raycast(backLeft);
		vecResult += _Raycast(Vector3.forward);
		vecResult += _Raycast(Vector3.back);

		return vecResult.normalized;
	}

	private Vector3 _Raycast(Vector3 direction) {

		if (Physics.Raycast(_self.transform.position, direction, distanceToAvoid, _buildingMask)) {

			return -direction;
		}

		return Vector3.zero;
	}
}
