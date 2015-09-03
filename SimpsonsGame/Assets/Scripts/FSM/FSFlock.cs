using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteeringController))]
public class FSFlock : FiniteState {

	private SteeringController _steeringController;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
	}
	
	public override FiniteState CheckState() {

		_steeringController.SetBehaviours(_UpdateBehvaiours());

		_steeringController.Steer();

		return this;
	}

	private SteeringBehaviour[] _UpdateBehvaiours() {

		SteeringBehaviour[] newBehaviours = {

		};

		return newBehaviours;
	}
}
