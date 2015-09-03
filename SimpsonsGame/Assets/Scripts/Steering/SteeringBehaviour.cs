using UnityEngine;

[RequireComponent(typeof(SteeringController))]
public abstract class SteeringBehaviour : MonoBehaviour {
	
	public abstract Vector3 GetSteering();
}