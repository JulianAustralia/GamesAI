using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathFinder : MonoBehaviour {
	
	private int _buildingLayer;
	private bool _colliding = false;
	private GameObject _cube;

	private List<PathNode> _nodes;
	private Dictionary<int, Dictionary<int, PathNode>> _xzNodeDictionary;

	private class _Cube : MonoBehaviour {

		public PathFinder pf;

		void OnTriggerEnter(Collider other) {
			Debug.Log(LayerMask.LayerToName(other.gameObject.layer));
			Debug.Log(other.gameObject.tag);
			if (other.gameObject.layer == pf._buildingLayer) {

				pf._colliding = true;
			}
		}
	}

	void Start () {

		_buildingLayer = LayerMask.NameToLayer("Building");

		_cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		_cube.AddComponent<_Cube>();
		_cube.GetComponent<_Cube>().pf = this;

		BoxCollider bc = _cube.GetComponent<BoxCollider>();
		bc.isTrigger = true;

		List<GameObject> buildingsWithCollisions = FindObjectsOfType<GameObject>().ToList<GameObject>().FindAll(
			(g) => g.layer == _buildingLayer && g.GetComponent<Collider>() != null && g.GetComponent<Collider>().enabled
		);

		buildingsWithCollisions.ForEach (
			(g) => {

				g.AddComponent<Rigidbody>();
				g.GetComponent<Rigidbody>().isKinematic = true;
			}
		);
	
		// TODO create grid
		Debug.Log("0 0 : " + (collisionExists(0, 0) ? "true" : "false"));
		Debug.Log("-3 10 : " + (collisionExists(-3, 10) ? "true" : "false"));
		Debug.Log("-8 10 : " + (collisionExists(-8, 10) ? "true" : "false"));

		buildingsWithCollisions.ForEach (
			(GameObject g) => Destroy(g.GetComponent<Rigidbody>())
		);
	}

	private bool collisionExists(int x, int z) {

		_colliding = false;

		_cube.transform.position = new Vector3((float) x, .5f, (float) z);

		return _colliding;
	}
}
