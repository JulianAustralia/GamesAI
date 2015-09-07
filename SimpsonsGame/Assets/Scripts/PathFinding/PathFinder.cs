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

		const float y = .5f;

		float left = (float) x - .5f;
		float right = (float) x + .5f;
		float back = (float) z - .5f;
		float front = (float) z + .5f;

		Vector3 backLeft = new Vector3(left, y, back);
		Vector3 backRight = new Vector3(right, y, back);
		Vector3 frontLeft = new Vector3(left, y, front);
		Vector3 frontRight = new Vector3(right, y, front);

		return Physics.Raycast(backLeft, Vector3.right, 1, _buildingMask) || Physics.Raycast(frontLeft, Vector3.right, 1, _buildingMask) ||
			Physics.Raycast(backRight, Vector3.left, 1, _buildingMask) || Physics.Raycast(frontRight, Vector3.left, 1, _buildingMask) ||
			Physics.Raycast(backLeft, Vector3.forward, 1, _buildingMask) || Physics.Raycast(backRight, Vector3.forward, 1, _buildingMask) ||
			Physics.Raycast(frontLeft, Vector3.back, 1, _buildingMask) || Physics.Raycast(frontRight, Vector3.back, 1, _buildingMask);
	}
	
	public List<Vector3> FindPath(Vector3 from, Vector3 to) {

		PathNode fromNode = _xzNodeDictionary[(int) Mathf.Round(from.x)][(int) Mathf.Round(from.z)];
		PathNode toNode = _xzNodeDictionary[(int) Mathf.Round(to.x)][(int) Mathf.Round(to.z)];

		if (fromNode.x == toNode.x && fromNode.z == toNode.z) return new List<Vector3>();
Debug.Log("From " + fromNode.x + "," + fromNode.z + " To " + toNode.x + "," + toNode.z);
		MinHeap<PathNode> heap = new MinHeap<PathNode>();
		PathNode node = fromNode;
		node.costTo = 0;
		node.totalCost = 0;
		node.heuristicCost = 0;
		node.beenChecked = true;
		node.previousNode = null;

		while (node != toNode) {

			node.neighbours.ForEach(
				(PathNode neighbour) => {

					float newCost = node.costTo + 1;

					if (neighbour.beenChecked == false || newCost < neighbour.costTo) {

						if (neighbour.beenChecked == false) {

							neighbour.heuristicCost = _heuristic(neighbour, toNode);
							neighbour.beenChecked = true;
						}

						neighbour.costTo = newCost;
						neighbour.totalCost = neighbour.costTo + neighbour.heuristicCost;
						neighbour.previousNode = node;
						heap.Add(neighbour);
					}
				}
			);

			if (heap.Count == 0) {

				return new List<Vector3>();
			}

			node = heap.ExtractDominating();
		}

		List<Vector3> resultingList = new List<Vector3>();

		while (node != null) {

			resultingList.Insert(0, new Vector3(node.x, 0, node.z));

			node = node.previousNode;
		}

		_resetNodes();

		return resultingList;
	}
	
	private float _heuristic(PathNode location, PathNode goal) {
		
		return _manhattan(location, goal);
	}
	
	private float _manhattan(PathNode location, PathNode goal) {
		
		return Mathf.Abs(location.x - goal.x) + Mathf.Abs(location.z - goal.z);
	}

	private void _resetNodes() {

		_nodes.ForEach((PathNode pn) => pn.Reset());
	}
	
	public bool ValidPosition(Vector3 v) {
		
		return ValidPosition((int) v.x, (int) v.z);
	}

	public bool ValidPosition(int x, int z) {

		return _xzNodeDictionary.ContainsKey (x) && _xzNodeDictionary[x].ContainsKey(z);
	}
}