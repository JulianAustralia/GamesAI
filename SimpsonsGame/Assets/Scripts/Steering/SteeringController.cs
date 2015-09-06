using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Movement))]
public class SteeringController : MonoBehaviour {
	
	private List<SteeringBehaviour> _steeringBehaviours;
	private Movement _movement;

	protected void Awake() {

		_movement = GetComponent<Movement>();
	}
	
	public void SetBehaviours(List<SteeringBehaviour> steeringBehaviours) {
		
		_steeringBehaviours = steeringBehaviours;
	}

	public void SetBehaviour(SteeringBehaviour behaviour) {
		
		_steeringBehaviours = new List<SteeringBehaviour>(1);
		_steeringBehaviours[0] = behaviour;
	}
	
	public void Steer() {

		var steering = Vector3.zero;

		foreach (var behaviour in _steeringBehaviours) {

			steering += behaviour.GetSteering();
		}

		_movement.Move(steering);
	}
}
