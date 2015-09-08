using UnityEngine;
using System.Collections;
using System;

public class SteerAvoidBuildings : SteeringBehaviour {

	private Transform _self;
	private int _buildingMask;

	public SteerAvoidBuildings(Transform s) {

		_self = s;
	}

	protected void Start() {

		_buildingMask = 1 << LayerMask.NameToLayer("Building");
	}
	
	public override Vector3 GetSteering() {
		
		Vector3 vecResult = Vector3.zero;


		if (Physics.Raycast(_self.transform.position, Vector3.right, 1, _buildingMask)) {

			vecResult += Vector3.left;
		}

		if (Physics.Raycast(_self.transform.position, Vector3.left, 1, _buildingMask)) {

			vecResult += Vector3.right;
		}

		if (Physics.Raycast(_self.transform.position, Vector3.forward, 1, _buildingMask)) {

			vecResult += Vector3.back;
		}

		if (Physics.Raycast(_self.transform.position, Vector3.back, 1, _buildingMask)) {

			vecResult += Vector3.forward;
		}

		return vecResult.normalized;
	}
}
