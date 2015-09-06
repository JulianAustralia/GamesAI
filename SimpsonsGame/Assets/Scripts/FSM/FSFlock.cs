using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSWander))]
[RequireComponent(typeof(FSEscape))]
public class FSFlock : FiniteState {

	private SteeringController _steeringController;
	private FSWander _wander;
	private FSEscape _escape;
	private Homer _homer;

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_wander = GetComponent<FSWander>();
		_escape = GetComponent<FSEscape>();
		_homer = GetComponent<Homer>();
	}
	
	public override FiniteState CheckState() {
		
		if (_homer.EnemyTooClose()) return _escape;

		List<Homer> homersInFlockingRange = _homer.GetHomersCloseEnoughToFlock();

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

		_steeringController.Steer();

		return this;
	}
}
