using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSWander))]
public class FSFlock : FiniteState {

	public float minFlockDistance;
	public float maxFlockDistance;

	private float _maxSqr;
	private float _minSqr;

	private SteeringController _steeringController;
	private FSWander _wander;

	private List<GameObject> _homers;

	public FSFlock() {

		_maxSqr = maxFlockDistance * maxFlockDistance;
		_minSqr = minFlockDistance * minFlockDistance;
	}

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_wander = GetComponent<FSWander>();
		
		GameObject[] homers = GameObject.FindGameObjectsWithTag("Homer");
		
		_homers = new List<GameObject>(homers.Length - 1);
		
		for (int i = 0; i < homers.Length; ++i) {
			
			if (this.gameObject != homers[i]) {
				
				_homers.Add(homers[i]);
			}
		}
	}
	
	public override FiniteState CheckState() {

		List<GameObject> homersInRange = _homers.FindAll(
			(GameObject homer) => {

				float sMag = (this.gameObject.transform.position - homer.transform.position).sqrMagnitude;

				return _minSqr <= sMag && sMag <= _maxSqr;
			}
		);

		if (homersInRange.Count == 0) {

			return _wander;
		}

		_steeringController.SetBehaviours (
			homersInRange.ConvertAll<SteeringBehaviour>(
				(GameObject homer) => new SteerToTarget(this.gameObject.transform, homer.transform)
			)
		);

		_steeringController.Steer();

		return this;
	}
}
