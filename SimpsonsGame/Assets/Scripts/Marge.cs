using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(FS_Marge_Wander))]
[RequireComponent(typeof(FS_Marge_CatchHomer))]
[RequireComponent(typeof(FS_Marge_ReturnHomer))]
public class Marge : Enemy {

	public int distToCatchHomer;
	private HomerFactory _factory;

	public void initialise() {

		GetComponent<FiniteStateMachine>().initialise();
	}

	protected void Awake() {

		_factory = GameObject.Find("HomerFactory").GetComponent<HomerFactory>();
	}

	public Homer GetClosestHomer() {

		Homer curHomer = null;
		float curDistance = 0;

		_factory.homers.ForEach(
			(Homer h) => {

				if (h.capturer == this.gameObject) return;

				Vector3 vec = this.gameObject.transform.position - h.gameObject.transform.position;
				float distance = vec.sqrMagnitude;

				if (distance <= distToCatchHomer * distToCatchHomer) {

					if (curHomer == null || distance < curDistance) {

						curHomer = h;
						curDistance = distance;
					}
				}
			}
		);

		return curHomer;
	}
}