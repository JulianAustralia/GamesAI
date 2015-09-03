using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomerFactory : MonoBehaviour {

	public int numberOfHomers;
	public List<GameObject> homers;

	void Start () {
	
		GameObject homer = GameObject.Find("Homer");
		GameObject dome = GameObject.Find("Dome");

		float radX = dome.transform.localScale.x / 2f;
		float minX = dome.transform.position.x - radX;
		float maxX = dome.transform.position.x + radX;

		float radZ = dome.transform.localScale.z / 2f;
		float minZ = dome.transform.position.z - radZ;
		float maxZ = dome.transform.position.z + radZ;

		for (int i = 0; i < numberOfHomers; ++i) {

			homers.Add(
				(GameObject) Instantiate(
					homer,
					new Vector3(
						UnityEngine.Random.Range(minX, maxX),
						homer.transform.position.y,
						UnityEngine.Random.Range(minZ, maxZ)
					),
					Quaternion.identity
				)
			);
		}

		// We could generate one less homer and randomly position this one
		// But this way is cleaner and this function will only be called once
		// per game so the performance hit is negligible
		GameObject.Destroy(homer);
	}
}
