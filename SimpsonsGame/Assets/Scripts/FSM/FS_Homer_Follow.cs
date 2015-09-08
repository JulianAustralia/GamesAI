using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
public class FS_Homer_Follow : FiniteState {

	private SteeringController _steeringController;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
	}

	public void StartFollowing(Transform target) {

		_steeringController.SetBehaviours(
			new List<SteeringBehaviour>() {
				new SteerAvoidBuildings(this.transform),
				new SteerToTarget(this.transform, target.transform)
			}
		);
	}
	
	public override FiniteState CheckState() {
		
		_steeringController.Steer();
		
		return this;
	}
}
