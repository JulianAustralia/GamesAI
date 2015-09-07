using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathNode : IComparable<PathNode> {
	
	public int x;
	public int z;
	public List<PathNode> neighbours = new List<PathNode>();

	// For path finding
	public float costTo;
	public float heuristicCost;
	public float totalCost;
	public bool beenChecked;
	public PathNode previousNode;

	public PathNode(int x, int z) {

		this.x = x;
		this.z = z;

		this.Reset();
	}

	public void Reset() {

		costTo = Mathf.Infinity;
		totalCost = Mathf.Infinity;
		heuristicCost = 0;
		beenChecked = false;
		previousNode = null;
	}
	
	public int CompareTo(PathNode otherNode) {

		if (totalCost >= otherNode.totalCost) {

			return 1;
		} else {

			return -1;
		}
	}
}