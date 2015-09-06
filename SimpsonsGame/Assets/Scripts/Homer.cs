using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Homer : MonoBehaviour {

	public List<Homer> otherHomers;

	public bool WithinRange(Homer otherHomer, float min, float max) {

		float sMag = (this.gameObject.transform.position - otherHomer.gameObject.transform.position).sqrMagnitude;

		return min * min <= sMag && sMag <= max * max;
	}
}
