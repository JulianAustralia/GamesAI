using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSWander))]
public class FSFlock : FiniteState {

	public float minFlockDistance;
	public float maxFlockDistance;

	private float _maxSqr;
	private float _minSqr;

	private SteeringController _steeringController;
	private FSWander _wander;
	private Homer _homer;

	public FSFlock() {

		_maxSqr = maxFlockDistance * maxFlockDistance;
		_minSqr = minFlockDistance * minFlockDistance;
	}

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_wander = GetComponent<FSWander>();
		_homer = GetComponent<Homer>();
	}
	
	public override FiniteState CheckState() {

		Transform thisTransform = this.gameObject.transform;

		List<Homer> homersInRange = _homer.otherHomers.FindAll(
			(Homer homer) => {

				float sMag = (thisTransform.position - homer.transform.position).sqrMagnitude;

				return _minSqr <= sMag && sMag <= _maxSqr;
			}
		);

		if (homersInRange.Count == 0) return _wander;

		_steeringController.SetBehaviours (
			homersInRange.ConvertAll<SteeringBehaviour>(
				(Homer homer) => new SteerToTarget(thisTransform, homer.transform)
			)
		);

		_steeringController.Steer();

		return this;
	}
}
