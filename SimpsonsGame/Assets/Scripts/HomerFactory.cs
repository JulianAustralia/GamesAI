using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomerFactory : MonoBehaviour {

	public int numberOfHomers;

	private List<GameObject> _homers;
	private List<GameObjectEdge> _edges;

	void Start() {
	
		GameObject homer = GameObject.Find("Homer");
		GameObject dome = GameObject.Find("Dome");

		float radX = dome.transform.localScale.x / 2f;
		float minX = dome.transform.position.x - radX;
		float maxX = dome.transform.position.x + radX;

		float radZ = dome.transform.localScale.z / 2f;
		float minZ = dome.transform.position.z - radZ;
		float maxZ = dome.transform.position.z + radZ;

		_homers = new List<GameObject>(numberOfHomers);
		_edges = new List<GameObjectEdge>(numberOfHomers*(numberOfHomers - 1)/2);

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

			float radians = UnityEngine.Random.Range(0, 360);

			newHomer.transform.eulerAngles = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians));

			_homers.Add(newHomer);
		}
		
		// We could generate one less homer and randomly position this one
		// But this way is cleaner and this function will only be called once
		// per game so the performance hit is negligible
		GameObject.Destroy(homer);

		for (int i = 0; i < numberOfHomers - 1; ++i) {

			GameObject hi = _homers[i];
			GameObjectEdgeReader eri = hi.GetComponent<GameObjectEdgeReader>();

			for (int j = i + 1; j < numberOfHomers; ++j) {

				GameObject hj = _homers[j];
				GameObjectEdgeReader erj = hj.GetComponent<GameObjectEdgeReader>();

				GameObjectEdge newEdge = new GameObjectEdge(hi, hj);

				eri.AddEdge(newEdge);
				erj.AddEdge(newEdge);

				_edges.Add(newEdge);
			}
		}
	}
}
