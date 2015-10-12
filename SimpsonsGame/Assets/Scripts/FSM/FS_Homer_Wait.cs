using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Homer_Escape))]
[RequireComponent(typeof(Homer))]
public class FS_Homer_Wait : FiniteState {

	private SteeringController _steeringController;
	private FS_Homer_Escape _escape;
	private Transform _waitPoint;
	private Homer _homer;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_homer = GetComponent<Homer>();
		_escape = GetComponent<FS_Homer_Escape>();
	}

	public void StartWaiting(Transform waitPoint) {

		_waitPoint = waitPoint;
	}
	
	public override FiniteState CheckState() {

		if (_homer.GetTooCloseEnemies().Exists((Enemy e) => e.gameObject != _homer.capturer)) {

			_homer.capturer = null;
			return _escape;
		}

		if ((_homer.capturer.transform.position - this.transform.position).sqrMagnitude < 9) {

			_steeringController.SetBehaviours(
				new List<SteeringBehaviour>() {
					new SteerAvoidBuildings(this.transform),
					new SteerFromTarget(this.transform, _homer.capturer.transform)
				}
			);
		} else {

			_steeringController.SetBehaviours(
				new List<SteeringBehaviour>() {
					new SteerAvoidBuildings(this.transform),
					new SteerToTarget(this.transform, _waitPoint)
				}
			);
		}
		
		_steeringController.Steer();
		
		return this;
	}
}
