using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HomerFactory : MonoBehaviour {

	public int numberOfHomers;

	private List<Homer> _homers;

	public float stageTime = 120;
	public int points = 0;

	public GameObject s;

	void Start() {
	
		List<Enemy> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>().ConvertAll<Enemy>((go) => go.GetComponent<Enemy>());
		GameObject homer = GameObject.Find("Homer");
		GameObject dome = GameObject.Find("Dome");

		// Have a buffer of half (position will be centered at max) a Homer width between the edge of the dome and the max spawn position
		float spawnRadius = Mathf.Min(dome.transform.localScale.x / 2f, dome.transform.localScale.z / 2f) - 2 * Mathf.Max(homer.transform.localScale.x, homer.transform.localScale.z);

		_homers = new List<Homer>(numberOfHomers);

		for (int i = 0; i < numberOfHomers; ++i) {

			float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
			float radius = UnityEngine.Random.Range(0, spawnRadius);

			GameObject newHomer = (GameObject) Instantiate(
				homer,
				new Vector3(
					Mathf.Sin(angle) * radius,
					homer.transform.position.y,
					Mathf.Cos(angle) * radius
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

	void Update () {
		if (stageTime > 0)
			stageTime -= Time.deltaTime;
		
		foreach(Homer h in _homers){
			if ((h.gameObject.transform.position.x <= s.transform.position.x +5) && 
			    (h.gameObject.transform.position.x >= s.transform.position.x -5) &&
			    (h.gameObject.transform.position.z <= s.transform.position.z +5) && 
			    (h.gameObject.transform.position.z >= s.transform.position.z -5))
				if (stageTime > 0)
					points++;
		}
		
		if (stageTime > 0) {
			Debug.Log(points);
			Debug.Log(stageTime);
		} else { 
			Debug.Log("Time is over");
			Debug.Log("Final points = " + points);
		}
	}
}
