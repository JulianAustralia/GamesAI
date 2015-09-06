﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Homer : MonoBehaviour {

	public float distFlock;
	public float distAvoidHomers;
	public float distAvoidEnemies;

	public List<Homer> otherHomers;
	public List<Enemy> enemies;

	public bool HomerTooClose() {

		return otherHomers.Exists((h) => WithinRange(h, 0, distAvoidHomers));
	}

	public List<Homer> GetTooCloseHomers() {

		return otherHomers.FindAll((h) => WithinRange(h, 0, distAvoidHomers));
	}
	
	public bool HomerCloseEnoughToFlock() {

		return otherHomers.Exists((h) => WithinRange(h, distAvoidHomers, distFlock));
	}

	public List<Homer> GetHomersCloseEnoughToFlock() {

		return otherHomers.FindAll((h) => WithinRange(h, distAvoidHomers, distFlock));
	}

	public bool EnemyTooClose() {

		return enemies.Exists((e) => WithinRange(e, 0, distAvoidEnemies));
	}
	
	public List<Enemy> GetTooCloseEnemies() {
		
		return enemies.FindAll((e) => WithinRange(e, 0, distAvoidEnemies));
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
