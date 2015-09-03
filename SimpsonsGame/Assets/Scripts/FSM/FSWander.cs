using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSFlock))]
public class FSWander : FiniteState {

	public float distanceToFlock;
	private SteeringController _steeringController;

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();

		List<SteeringBehaviour> behvaiours = new List<SteeringBehaviour>();

		behvaiours.Add(new SteerWanderXZ());

		_steeringController.SetBehaviours(behvaiours);
	}

	public override FiniteState CheckState() {

		GameObject[] homers = GameObject.FindGameObjectsWithTag("Homer");
		
		foreach (GameObject homer in homers) {
			
			if (
				this.gameObject != homer &&
				(this.gameObject.transform.position - homer.transform.position).sqrMagnitude < distanceToFlock * distanceToFlock
			) {

				return this.GetComponent<FSFlock>();
			}
		}
	
		_steeringController.Steer();

		return this;
	}
}