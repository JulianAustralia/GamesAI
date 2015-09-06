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
		_steeringBehaviours.Add(behaviour);
	}
	
	public void AddBehaviours(List<SteeringBehaviour> steeringBehaviours) {
		
		if (_steeringBehaviours == null) {
			
			SetBehaviours(steeringBehaviours);
		} else {
			
			_steeringBehaviours.AddRange(steeringBehaviours);
		}
	}
	
	public void AddBehaviours(SteeringBehaviour steeringBehaviour) {
		
		if (_steeringBehaviours == null) {
			
			SetBehaviour(steeringBehaviour);
		} else {
			
			_steeringBehaviours.Add(steeringBehaviour);
		}
	}
	
	public void Steer() {

		var steering = Vector3.zero;

		foreach (var behaviour in _steeringBehaviours) {

			steering += behaviour.GetSteering();
		}

		_movement.Move(steering);
	}
}
