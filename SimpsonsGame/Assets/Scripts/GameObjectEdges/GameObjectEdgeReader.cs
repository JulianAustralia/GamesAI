using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GameObject))]
public class GameObjectEdgeReader : MonoBehaviour {

	private List<GameObjectEdge> _edges = new List<GameObjectEdge>();

	public void AddEdge(GameObjectEdge e) {

		if (_edges.Count == 0 || _edges[_edges.Count - 1].distSqr < e.distSqr) {

			_edges.Add(e);
		} else if (_edges[0].distSqr > e.distSqr) {

			_edges.Insert(0, e);
		} else {

			_edges.Insert(_BinSearch(e), e);
		}
	}
	
	public List<GameObject> GetEdges(GameObject beginObject, float min, float max) {
		
		// The edges should always be sorted or almost sorted as the GameObjects only move a small amount every update
		// I chose to use TimSort here as it has a O(1) for already sorted arrays like Bubble Sort, but also a O(n log n)
		// for unsorted arrays like quick, merge, and heap sort.
		TimSort.ListTimSort<GameObjectEdge>.Sort(_edges, (GameObjectEdge e1, GameObjectEdge e2) => e1.CompareTo(e2));

		// All the distances are stored as magnitude squared to save on computations, therefore lookup is down with a squared value
		int minIndex = _BinSearch(min * min);
		int maxIndex = _BinSearch(max * max);

		return _edges.GetRange(
			minIndex,
			maxIndex - minIndex + 1
		).ConvertAll<GameObject>(
			(GameObjectEdge e) => e.o1 == beginObject ? e.o2 : e.o1
		);
	}

	private int _BinComplementNeg(int i) { return i < 0 ? ~i : i; }

	private int _BinSearch(GameObjectEdge e) { return _BinComplementNeg(_edges.BinarySearch(e)); }

	private int _BinSearch(float f) { return _BinSearch(new GameObjectEdge(null, null, f)); }
}