using UnityEngine;
using System;

public class GameObjectEdge : IComparable<GameObjectEdge> {
	
	public GameObject o1;
	public GameObject o2;
	public float distSqr;
	
	public GameObjectEdge(GameObject o1, GameObject o2) {
		
		this.o1 = o1;
		this.o2 = o2;
		
		this.Update();
	}

	public GameObjectEdge(GameObject o1, GameObject o2, float distSqr) {
		
		this.o1 = o1;
		this.o2 = o2;

		this.distSqr = distSqr;
	}
	
	public void Update() {
		
		distSqr = (o1.transform.position - o2.transform.position).sqrMagnitude;
	}
	
	public int CompareTo(GameObjectEdge x) {

		return this.distSqr.CompareTo(x.distSqr);
	}
}