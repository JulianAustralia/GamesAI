using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class SteeringController : MonoBehaviour {
	
	private SteeringBehaviour[] _steeringBehaviours;
	private Movement _movement;

	protected void Awake() {

		_movement = GetComponent<Movement>();
	}
	
	public void SetBehaviours(SteeringBehaviour[] steeringBehaviours) {

		_steeringBehaviours = steeringBehaviours;
	}
	
	public void Steer() {

		var steering = Vector3.zero;

		foreach (var behaviour in _steeringBehaviours) {

			steering += behaviour.GetSteering();
		}

		_movement.Move(steering);
	}
}
