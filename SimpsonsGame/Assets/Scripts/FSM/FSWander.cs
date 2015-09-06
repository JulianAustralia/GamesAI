using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSFlock))]
[RequireComponent(typeof(FSEscape))]
public class FSWander : FiniteState {

	private SteeringController _steeringController;
	private FSFlock _flock;
	private FSEscape _escape;
	private Homer _homer;

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_flock = GetComponent<FSFlock>();
		_escape = GetComponent<FSEscape>();
		_homer = GetComponent<Homer>();
	}

	public override FiniteState CheckState() {

		if (_homer.EnemyTooClose()) return _escape;

		if (_homer.HomerCloseEnoughToFlock()) return _flock;

		_steeringController.SetBehaviour(new SteerWanderXZ(this.gameObject.transform));

		_steeringController.Steer();

		return this;
	}
}