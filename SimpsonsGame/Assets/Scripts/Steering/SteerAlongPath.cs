using UnityEngine;
using System.Collections.Generic;

public class SteerAlongPath : SteeringBehaviour {

	public const float distanceTouchPoint = 1.5f;

	public Transform self;
	public List<Vector3> path;
	
	public SteerAlongPath(Transform s, List<Vector3> p) {

		self = s;
		path = p;
	}
	
	public override Vector3 GetSteering() {
		
		if (path.Count == 0) return Vector3.zero;

		Vector3 next;

		// Manually creating vector because we want to use our current Y coordinate
		while (
			(
				next = new Vector3(
					path[0].x - self.position.x,
					self.position.y,
					path[0].z - self.position.z
				)
			).sqrMagnitude <= distanceTouchPoint * distanceTouchPoint
		) {
			//Debug.Log("To " + path[path.Count - 1].x + " " + path[path.Count - 1].z + " Done " + path[0].x + " " + path[0].z);
			path.RemoveAt(0);

			if (path.Count == 0) return Vector3.zero;//Debug.Log("To " + path[path.Count - 1].x + " " + path[path.Count - 1].z + " Next " + path[0].x + " " + path[0].z);
		}

		return next.normalized;
	}
}
