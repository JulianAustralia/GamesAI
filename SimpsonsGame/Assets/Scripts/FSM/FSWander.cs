using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSFlock))]
public class FSWander : FiniteState {

	public float minFlockDistance;
	public float maxFlockDistance;

	private float _minSqr;
	private float _maxSqr;

	private SteeringController _steeringController;
	private FSFlock _flock;
	private Homer _homer;
	
	public FSWander() {
		
		_maxSqr = maxFlockDistance * maxFlockDistance;
		_minSqr = minFlockDistance * minFlockDistance;
	}

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_flock = GetComponent<FSFlock>();
		_homer = GetComponent<Homer>();
	}

	public override FiniteState CheckState() {

		Transform thisTransform = this.gameObject.transform;

		if (
			_homer.otherHomers.Exists (
				(Homer homer) => {

					float sMag = (thisTransform.position - homer.transform.position).sqrMagnitude;

					return _minSqr <= sMag && sMag <= _maxSqr;
				}
			)
		) return _flock;

		List<SteeringBehaviour> behvaiours = new List<SteeringBehaviour>();

		behvaiours.Add(new SteerWanderXZ(thisTransform));
		
		_steeringController.SetBehaviours(behvaiours);

		_steeringController.Steer();

		return this;
	}
}