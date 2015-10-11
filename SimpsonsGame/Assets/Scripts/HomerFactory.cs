using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HomerFactory : MonoBehaviour {

	public int numberOfHomers;
	public List<Homer> homers;

	private int _buildingMask;
	private List<Enemy> _enemies;
	private GameObject _original;
	private float _originalY;
	private float _spawnRadius;
	private PathFinder _pathFinder;

	private bool _initialized = false;

	private void _initialise() {

		if (_initialized) {

			homers.ForEach(h => GameObject.Destroy(h.gameObject));

			return;
		}

		_initialized = true;

		_buildingMask = 1 << LayerMask.NameToLayer("Building");

		_enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>().ConvertAll<Enemy>(e => e.GetComponent<Enemy>());

		_original = GameObject.Find("HomerOriginal");

		_originalY = (float) _original.transform.position.y;

		Vector3 dt = GameObject.Find("Dome").transform.localScale;
		Vector3 ot = _original.transform.localScale;
		_spawnRadius = (float) (Mathf.Min(dt.x, dt.z) / 2f - 2 * Mathf.Max(ot.x, ot.z));

		_pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();

		// Move the Homer out of the world
		_original.transform.position = new Vector3(0, -1000, 0);
	}

	public void CreateHomers() {

		_initialise();

		homers = new List<Homer>(numberOfHomers);

		for (int i = 0; i < numberOfHomers; ++i) {

			PathNode positionPN = _pathFinder.GetRandomPosition();

			GameObject newHomer = (GameObject) Instantiate(
				_original,
				new Vector3(positionPN.x, _originalY, positionPN.z),
				Quaternion.AngleAxis(
					UnityEngine.Random.Range(0f, 360f),
					Vector3.up
				)
			);
			
			homers.Add(newHomer.GetComponent<Homer>());
		}

		for (int i = 0; i < numberOfHomers; ++i) {

			List<Homer> otherHomers = new List<Homer>(homers);

			otherHomers.RemoveAt(i);

			homers[i].otherHomers = otherHomers;
			homers[i].enemies = _enemies;
		}
	}
}
