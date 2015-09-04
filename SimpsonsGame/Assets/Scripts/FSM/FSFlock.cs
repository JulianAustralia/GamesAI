using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSWander))]
public class FSFlock : FiniteState {

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

		List<SteeringBehaviour> behaviours = new List<SteeringBehaviour>();

		foreach (GameObject homer in homers) {
			
			if (this.gameObject == homer) {
				
				continue;
			}

			float sMag = (this.gameObject.transform.position - homer.transform.position).sqrMagnitude;

			if (sMag > _maxSqr || sMag < _minSqr) {

				continue;
			}

			SteerToTarget steer = new SteerToTarget();
			steer.self = this.gameObject.transform;
			steer.target = homer.transform;
			steer.Initialize();

			behaviours.Add(steer);
		}

		if (behaviours.Count == 0) {

			return this.GetComponent<FSWander>();
		}

		_steeringController.SetBehaviours(behaviours);

		_steeringController.Steer();

		return this;
	}
}
