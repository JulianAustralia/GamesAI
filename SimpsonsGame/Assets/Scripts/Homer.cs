using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Homer : MonoBehaviour {

	public float minDistanceToFlock;
	public float maxDistanceToFlock;
	public float minDistanceToAvoidHomers;
	public float maxDistanceToAvoidHomers;
	public float minDistanceToAvoidEnemies;
	public float maxDistanceToAvoidEnemies;

	public List<Homer> otherHomers;
	public List<Enemy> enemies;

	public bool HomerTooClose() {

		return otherHomers.Exists((h) => WithinRange(h, minDistanceToAvoidHomers, maxDistanceToAvoidHomers));
	}

	public List<Homer> GetTooCloseHomers() {

		return otherHomers.FindAll((h) => WithinRange(h, minDistanceToAvoidHomers, maxDistanceToAvoidHomers));
	}
	
	public bool HomerCloseEnoughToFlock() {

		return otherHomers.Exists((h) => WithinRange(h, minDistanceToFlock, maxDistanceToFlock));
	}

	public List<Homer> GetHomersCloseEnoughToFlock() {

		return otherHomers.FindAll((h) => WithinRange(h, minDistanceToFlock, maxDistanceToFlock));
	}

	public bool EnemyTooClose() {

		return enemies.Exists((e) => WithinRange(e, minDistanceToAvoidEnemies, maxDistanceToAvoidEnemies));
	}
	
	public List<Enemy> GetTooCloseEnemies() {
		
		return enemies.FindAll((e) => WithinRange(e, minDistanceToAvoidEnemies, maxDistanceToAvoidEnemies));
	}

	public bool WithinRange(Homer h, float min, float max) { return WithinRange(h.gameObject, min, max); }
	public bool WithinRange(Enemy e, float min, float max) { return WithinRange(e.gameObject, min, max); }
	public bool WithinRange(GameObject g, float min, float max) {

		return WithinRange(
			(this.gameObject.transform.position - g.transform.position).sqrMagnitude,
			min * min,
			max * max
		);
	}
	public bool WithinRange(float x, float min, float max) { return min <= x && x <= max; }
}
