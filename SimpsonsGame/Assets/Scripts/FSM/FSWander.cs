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

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();

		_minSqr = minFlockDistance * minFlockDistance;
		_maxSqr = maxFlockDistance * maxFlockDistance;
	}

	public override FiniteState CheckState() {

		GameObject[] homers = GameObject.FindGameObjectsWithTag("Homer");
		
		foreach (GameObject homer in homers) {

			if (this.gameObject == homer) {

				continue;
			}

			float sMag = (this.gameObject.transform.position - homer.transform.position).sqrMagnitude;

			if (sMag < _maxSqr && sMag > _minSqr) {

				return this.GetComponent<FSFlock>();
			}
		}
	
		List<SteeringBehaviour> behvaiours = new List<SteeringBehaviour>();

		SteerWanderXZ steer = new SteerWanderXZ();
		steer.self = this.gameObject.transform;
		steer.Initialize ();
		
		behvaiours.Add(steer);
		
		_steeringController.SetBehaviours(behvaiours);

		_steeringController.Steer();

		return this;
	}
}