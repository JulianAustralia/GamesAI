using UnityEngine;
using System.Collections;

public class SafeZone : MonoBehaviour {

	public Enemy hunter;

	void Start () {

		Vector3 newPosition = new Vector3(
			transform.position.x,
			hunter.transform.position.y,
			transform.position.z
		);

		Vector3 newRotation = new Vector3 (
			-transform.position.x,
			hunter.transform.position.y,
			-transform.position.z
		);

		hunter.transform.position = newPosition;

		hunter.transform.rotation = Quaternion.LookRotation(newRotation);
	}

	void Update () {
	

	}
}
