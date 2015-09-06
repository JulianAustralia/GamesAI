using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSFlock))]
public class FSWander : FiniteState {

	public float minFlockDistance;
	public float maxFlockDistance;

	private SteeringController _steeringController;
	private FSFlock _flock;
	private Homer _homer;

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_flock = GetComponent<FSFlock>();
		_homer = GetComponent<Homer>();
	}

	public override FiniteState CheckState() {

		if (
			_homer.otherHomers.Exists(
				(h) => _homer.WithinRange (h, minFlockDistance, maxFlockDistance)
			)
		) {

			return _flock;
		}

		_steeringController.SetBehaviour(new SteerWanderXZ(this.gameObject.transform));

		_steeringController.Steer();

		return this;
	}
}