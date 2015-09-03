using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteeringController))]
public class FSWander : FiniteState {

	private SteeringController _steeringController;

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();

		SteeringBehaviour[] behvaiours = {new SteerWander()};

		_steeringController.SetBehaviours(behvaiours);
	}

	public override FiniteState CheckState() {
	
		_steeringController.Steer();

		return this;
	}
}