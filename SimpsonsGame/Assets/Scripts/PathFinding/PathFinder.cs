using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathFinder : MonoBehaviour {
	
	private List<PathNode> _nodes = new List<PathNode>();
	private Dictionary<int, Dictionary<int, PathNode>> _xzNodeDictionary = new Dictionary<int, Dictionary<int, PathNode>>();

	private int _buildingLayer;
	private bool _colliding = false;
	private GameObject _cube;

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

	private class _PathNodeBuilder {
		public PathNode parent;
		public PathNode newPathNode;

		public _PathNodeBuilder(PathNode p, PathNode n) {

			this.parent = p;
			this.newPathNode = n;
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
	
		Stack<_PathNodeBuilder> coords = new Stack<_PathNodeBuilder>();

		// Assume there are no obstacles at origin
		coords.Push(new _PathNodeBuilder(null, new PathNode(0, 0)));

		// Generate a graph of 1x 1z wide spaces which do not have any obstacles starting from the center and spreading outwards
		// Keep a map of the nodes by x and then z coordinates so that we can lookup in O(1) time
		while (coords.Count > 0) {

			_PathNodeBuilder builder = coords.Pop();
			PathNode newNode = builder.newPathNode;

			if (Mathf.Min(newNode.x, newNode.z) < -100 || Mathf.Max(newNode.x, newNode.z) > 100) {

				Debug.Log("Error: " + newNode.x + " " + newNode.z + " out of range");
				continue;
			}

			if (_collision(newNode.x, newNode.z)) {

				continue;
			}

			if (_xzNodeDictionary.ContainsKey(newNode.x)) {

				Dictionary<int, PathNode> zDict = _xzNodeDictionary[newNode.x];

				if (zDict.ContainsKey(newNode.z)) {

					if (builder.parent != null) {
						
						builder.parent.neighbours.Add(zDict[newNode.z]);
					}

					continue;
				} else {

					zDict[newNode.z] = newNode;
				}
			} else {

				Dictionary<int, PathNode> zDict = new Dictionary<int, PathNode>();

				zDict[newNode.z] = newNode;

				_xzNodeDictionary[newNode.x] = zDict;
			}

			if (builder.parent != null) {
				
				builder.parent.neighbours.Add(newNode);
			}
			
			_nodes.Add(newNode);

			// Using Von-Neuman neighbour rules (top, right, bottom, left (no diagonals))
			coords.Push(new _PathNodeBuilder(newNode, new PathNode(newNode.x - 1, newNode.z)));
			coords.Push(new _PathNodeBuilder(newNode, new PathNode(newNode.x + 1, newNode.z)));
			coords.Push(new _PathNodeBuilder(newNode, new PathNode(newNode.x, newNode.z - 1)));
			coords.Push(new _PathNodeBuilder(newNode, new PathNode(newNode.x, newNode.z + 1)));
		}

		buildingsWithCollisions.ForEach (
			(GameObject g) => Destroy(g.GetComponent<Rigidbody>())
		);
	}

	private bool _collision(int x, int z) {

		_colliding = false;

		_cube.transform.position = new Vector3((float) x, .5f, (float) z);
		Debug.Log("_collision " + x + " " + z + " " + _colliding);
		return _colliding;
	}
}
