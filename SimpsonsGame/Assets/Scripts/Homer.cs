using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Homer : MonoBehaviour {

	public List<Homer> otherHomers;
	public List<Enemy> enemies;

	public bool WithinRange(Homer h, float min, float max) {

		return WithinRange(h.gameObject, min, max);
	}

	public bool WithinRange(Enemy e, float min, float max) {

		return WithinRange(e.gameObject, min, max);
	}

	public bool WithinRange(GameObject g, float min, float max) {

		return WithinRange(
			(this.gameObject.transform.position - g.transform.position).sqrMagnitude,
			min * min,
			max * max
		);
	}

	public bool WithinRange(float x, float min, float max) {

		return min <= x && x <= max;
	}
}
