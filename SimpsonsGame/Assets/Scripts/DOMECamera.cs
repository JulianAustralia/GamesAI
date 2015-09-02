using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class DOMECamera : MonoBehaviour {

	public Vector3 center;
	public float radius;
	public float horizontalSpeed = 2f;
	public float verticalSpeed = 2f;

	public float horizontalAngle = 0f;
	public float verticalAngle = .1f;
	
	private Camera _camera;
	
	protected void Awake(){
		
		_camera = GetComponent<Camera>();

		_updateChanges(0, 0);
	}

	void Update () {
	
		float dRotationHorizontal = 0;
		float dRotationVertical = 0;

		if (Input.GetKey(KeyCode.W)) {
			
			dRotationVertical -= verticalSpeed;
		}
		
		if (Input.GetKey(KeyCode.S)) {
			
			dRotationVertical += verticalSpeed;
		}
		
		if (Input.GetKey(KeyCode.A)) {
			
			dRotationHorizontal += horizontalSpeed;
		}
		
		if (Input.GetKey(KeyCode.D)) {
			
			dRotationHorizontal -= horizontalSpeed;
		}

		if (dRotationHorizontal != 0 || dRotationVertical != 0) {

			_updateChanges(dRotationHorizontal, dRotationVertical);
		}
	}

	private void _updateChanges(float drH, float drV) {

		horizontalAngle = (horizontalAngle + drH * Time.deltaTime) % 360;
		verticalAngle = Mathf.Clamp(
			verticalAngle + drV * Time.deltaTime,
			.1f,
			Mathf.PI / 2 - .1f
		);
		
		float radiusSinVertical = radius * Mathf.Sin(verticalAngle);
		
		_camera.transform.position = new Vector3(
			radiusSinVertical * Mathf.Sin(horizontalAngle),
			radius * Mathf.Cos(verticalAngle),
			radiusSinVertical * Mathf.Cos(horizontalAngle)
			);
		_camera.transform.LookAt(center);
	}
}
