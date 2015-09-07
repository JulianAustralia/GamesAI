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
		
		if (path.Count == 0) return Vector3.zero;

		while ((path[0] - self.transform.position).sqrMagnitude <= 1) {

			path.RemoveAt(0);

			if (path.Count == 0) return Vector3.zero;
		}

		return (path[0] - self.transform.position).normalized;
	}
}
