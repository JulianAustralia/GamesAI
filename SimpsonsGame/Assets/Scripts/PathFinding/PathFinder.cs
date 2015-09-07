using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {
	
	private List<PathNode> _nodes = new List<PathNode>();
	private Dictionary<int, Dictionary<int, PathNode>> _xzNodeDictionary = new Dictionary<int, Dictionary<int, PathNode>>();

	private int _buildingLayer;
	private int _buildingMask;

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
		_buildingMask = 1 << _buildingLayer;

		Stack<_PathNodeBuilder> coords = new Stack<_PathNodeBuilder>();

		// Assume there are no obstacles at origin
		coords.Push(new _PathNodeBuilder(null, new PathNode(0, 0)));

		// Generate a graph of 1x 1z wide spaces which do not have any obstacles starting from the center and spreading outwards
		// Keep a map of the nodes by x and then z coordinates so that we can lookup in O(1) time
		while (coords.Count > 0) {

			_PathNodeBuilder builder = coords.Pop();
			PathNode newNode = builder.newPathNode;

			if (Mathf.Min(newNode.x, newNode.z) < -100 || Mathf.Max(newNode.x, newNode.z) > 100) {

				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.localScale = new Vector3(1f, 20f, 1f);
				cube.transform.position = new Vector3((float) newNode.x, 10f, (float) newNode.z);

				continue;
			}

			if (_collision(newNode.x, newNode.z)) {

				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.position = new Vector3((float) newNode.x, .5f, (float) newNode.z);

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

			// Using Von-Neuman neighbour rules (top, bottom, left, right (no diagonals))
			coords.Push(new _PathNodeBuilder(newNode, new PathNode(newNode.x - 1, newNode.z)));
			coords.Push(new _PathNodeBuilder(newNode, new PathNode(newNode.x + 1, newNode.z)));
			coords.Push(new _PathNodeBuilder(newNode, new PathNode(newNode.x, newNode.z - 1)));
			coords.Push(new _PathNodeBuilder(newNode, new PathNode(newNode.x, newNode.z + 1)));
		}
	}

	private bool _collision(int x, int z) {

		Vector3 toFrontRight = new Vector3(1, 0, 1);
		Vector3 toFrontLeft = new Vector3(-1, 0, 1);
		const float y = .5f;

		float left = (float) x - .5f;
		float right = (float) x + .5f;
		float back = (float) z - .5f;
		float front = (float) z + .5f;

		Vector3 backLeft = new Vector3(left, y, back);
		Vector3 backRight = new Vector3(right, y, back);
		Vector3 frontLeft = new Vector3(left, y, front);

		return Physics.Raycast(backLeft, Vector3.right, 1, _buildingMask) || Physics.Raycast(frontLeft, Vector3.right, 1, _buildingMask) ||
			Physics.Raycast(backLeft, Vector3.forward, 1, _buildingMask) || Physics.Raycast(backRight, Vector3.forward, 1, _buildingMask) ||
			Physics.Raycast(backLeft, toFrontRight, Mathf.Sqrt(2), _buildingMask) || Physics.Raycast(backRight, toFrontLeft, Mathf.Sqrt(2), _buildingMask);
	}
}
