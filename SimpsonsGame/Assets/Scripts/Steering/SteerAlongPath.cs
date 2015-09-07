using UnityEngine;
using System.Collections.Generic;

public class SteerAlongPath : SteeringBehaviour {
	
	public Transform self;
	public List<Vector3> path;
	
	public SteerAlongPath(Transform s, List<Vector3> p) {
		
		self = s;
		path = p;
	}
	
	public override Vector3 GetSteering() {
		
		// TODO

		return Vector3.zero;
	}
}
