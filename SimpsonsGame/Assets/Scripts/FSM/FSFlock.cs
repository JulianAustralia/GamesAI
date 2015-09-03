using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSWander))]
public class FSFlock : FiniteState {

	public float maxFlockDistance;
	private SteeringController _steeringController;
	
	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
	}
	
	public override FiniteState CheckState() {

		GameObject[] homers = GameObject.FindGameObjectsWithTag("Homer");

		List<SteeringBehaviour> behaviours = new List<SteeringBehaviour>();

		foreach (GameObject homer in homers) {
			
			if (this.gameObject == homer) {
				
				continue;
			}

			Vector3 differenceV = (this.gameObject.transform.position - homer.transform.position);

			if (differenceV.sqrMagnitude > maxFlockDistance) {

				continue;
			}

			SteerToTarget steer = new SteerToTarget();
			steer.self = this.gameObject.transform;
			steer.target = homer.transform;

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
