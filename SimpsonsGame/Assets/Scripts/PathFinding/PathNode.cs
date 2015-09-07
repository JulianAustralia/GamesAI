using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode {
	
	public int x;
	public int z;
	public List<PathNode> neighbours = new List<PathNode>();

	public PathNode(int x, int z) {

		this.x = x;
		this.z = z;
	}
}