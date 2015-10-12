using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Homer_Wander))]
[RequireComponent(typeof(FS_Homer_Escape))]
[RequireComponent(typeof(Homer))]
public class FS_Homer_Flock : FiniteState {

	private SteeringController _steeringController;
	private FS_Homer_Wander _wander;
	private FS_Homer_Escape _escape;
	private Homer _homer;

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_wander = GetComponent<FS_Homer_Wander>();
		_escape = GetComponent<FS_Homer_Escape>();
		_homer = GetComponent<Homer>();
	}
	
	public override FiniteState CheckState() {
		
		if (_homer.EnemyTooClose()) return _escape;

		List<Homer> homersInFlockingRange = _homer.GetHomersCloseEnoughToFlock().FindAll(h => h.capturer == null);

		if (homersInFlockingRange.Count == 0) return _wander;

		_steeringController.SetBehaviours(
			homersInFlockingRange.ConvertAll<SteeringBehaviour>(
				(h) => new SteerToTarget(this.gameObject.transform, h.transform)
			)
		);

		_steeringController.AddBehaviours(
			_homer.GetTooCloseHomers().ConvertAll<SteeringBehaviour>(
				(h) => new SteerFromTarget(this.gameObject.transform, h.transform)
			)
		);

		_steeringController.AddBehaviours(new SteerAvoidBuildings(this.transform));

		_steeringController.Steer();

		return this;
	}
}
