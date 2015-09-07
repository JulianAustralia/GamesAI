using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Homer_Flock))]
[RequireComponent(typeof(FS_Homer_Escape))]
[RequireComponent(typeof(Homer))]
public class FS_Homer_Wander : FiniteState {

	private SteeringController _steeringController;
	private FS_Homer_Flock _flock;
	private FS_Homer_Escape _escape;
	private Homer _homer;

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_flock = GetComponent<FS_Homer_Flock>();
		_escape = GetComponent<FS_Homer_Escape>();
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