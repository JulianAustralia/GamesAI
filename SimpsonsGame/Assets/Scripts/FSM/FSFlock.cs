using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSWander))]
public class FSFlock : FiniteState {

	public float minFlockDistance;
	public float maxFlockDistance;
	public float minAvoidDistance;
	public float maxAvoidDistance;

	private SteeringController _steeringController;
	private FSWander _wander;
	private Homer _homer;

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_wander = GetComponent<FSWander>();
		_homer = GetComponent<Homer>();
	}
	
	public override FiniteState CheckState() {

		List<Homer> homersInRange = _homer.otherHomers.FindAll(
			(h) => _homer.WithinRange(h, minFlockDistance, maxFlockDistance)
		);

		if (homersInRange.Count == 0) return _wander;

		_steeringController.SetBehaviours(
			homersInRange.ConvertAll<SteeringBehaviour>(
				(h) => new SteerToTarget(this.gameObject.transform, h.transform)
			)
		);

		_steeringController.AddBehaviours(
			_homer.otherHomers.FindAll(
				(h) => _homer.WithinRange(h, minAvoidDistance, maxAvoidDistance)
			).ConvertAll<SteeringBehaviour>(
				(h) => new SteerFromTarget(this.gameObject.transform, h.transform)
			)
		);

		_steeringController.Steer();

		return this;
	}
}
