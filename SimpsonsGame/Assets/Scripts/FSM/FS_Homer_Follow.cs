using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Homer_Escape))]
[RequireComponent(typeof(Homer))]
public class FS_Homer_Follow : FiniteState {

	private SteeringController _steeringController;
	private FS_Homer_Escape _escape;
	private Homer _homer;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_homer = this.GetComponent<Homer>();
		_escape = this.GetComponent<FS_Homer_Escape>();
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
		
		if (_homer.GetTooCloseEnemies().Exists((Enemy e) => e.gameObject != _homer.capturer)) {

			_homer.capturer = null;
			return _escape;
		}

		_steeringController.Steer();
		
		return this;
	}
}
