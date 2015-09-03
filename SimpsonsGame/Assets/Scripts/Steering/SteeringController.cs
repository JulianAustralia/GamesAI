using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class SteeringController : MonoBehaviour {
	
	private SteeringBehaviour[] _steeringBehaviours;
	private Movement _movement;
	
	void Awake() {

		_steeringBehaviours = GetComponents<SteeringBehaviour>();
		_movement = GetComponent<Movement>();
	}
	
	void Update() {

		// There are lots of ways to combine steering behaviours
		var steering = Vector3.zero;

		foreach (var behaviour in _steeringBehaviours) {

			steering += behaviour.GetSteering();
		}

		_movement.Move(steering);
	}
}
