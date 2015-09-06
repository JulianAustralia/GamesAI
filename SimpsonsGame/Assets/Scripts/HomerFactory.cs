using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HomerFactory : MonoBehaviour {

	public int numberOfHomers;

	private List<Homer> _homers;

	void Start() {
	
		List<Enemy> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>().ConvertAll<Enemy>((go) => go.GetComponent<Enemy>());
		GameObject homer = GameObject.Find("Homer");
		GameObject dome = GameObject.Find("Dome");

		float radX = dome.transform.localScale.x / 2f;
		float minX = dome.transform.position.x - radX;
		float maxX = dome.transform.position.x + radX;

		float radZ = dome.transform.localScale.z / 2f;
		float minZ = dome.transform.position.z - radZ;
		float maxZ = dome.transform.position.z + radZ;

		_homers = new List<Homer>(numberOfHomers);

		for (int i = 0; i < numberOfHomers; ++i) {

			GameObject newHomer = (GameObject) Instantiate(
				homer,
				new Vector3(
					UnityEngine.Random.Range(minX, maxX),
					homer.transform.position.y,
					UnityEngine.Random.Range(minZ, maxZ)
				),
				Quaternion.identity
			);

			Vector3 euler = newHomer.transform.eulerAngles;
			euler.y = UnityEngine.Random.Range(0f, 360f);
			newHomer.transform.eulerAngles = euler;

			_homers.Add(newHomer.GetComponent<Homer>());
		}
		
		// We could generate one less homer and randomly position this one
		// But this way is cleaner and this function will only be called once
		// per game so the performance hit is negligible
		GameObject.Destroy(homer);

		for (int i = 0; i < numberOfHomers; ++i) {

			List<Homer> otherHomers = new List<Homer>(_homers);

			otherHomers.RemoveAt(i);

			_homers[i].otherHomers = otherHomers;
			_homers[i].enemies = enemies;
		}
	}
}
