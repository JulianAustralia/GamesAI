using UnityEngine;

[RequireComponent(typeof(SteeringController))]
public abstract class SteeringBehaviour {

	public abstract void Initialize();

	public abstract Vector3 GetSteering();
}