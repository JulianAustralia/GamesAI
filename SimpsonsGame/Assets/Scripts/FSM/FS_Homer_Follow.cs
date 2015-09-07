using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(Homer))]
public class FS_Homer_Follow : FiniteState {
	
	private SteeringController _steer;
	private Homer _homer;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_homer = GetComponent<Homer>();
	}
	
	public override FiniteState CheckState() {
		
		// TODO
		
		return this;
	}
}
