using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSFlock))]
public class FSWander : FiniteState {

	public float minFlockDistance;
	public float maxFlockDistance;

	private float _minSqr;
	private float _maxSqr;

	private SteeringController _steeringController;
	private FSFlock _flock;

	private List<GameObject> _homers;
	
	public FSWander() {
		
		_maxSqr = maxFlockDistance * maxFlockDistance;
		_minSqr = minFlockDistance * minFlockDistance;
	}

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_flock = GetComponent<FSFlock>();
		
		GameObject[] homers = GameObject.FindGameObjectsWithTag("Homer");
		
		_homers = new List<GameObject>(homers.Length - 1);
		
		for (int i = 0; i < homers.Length; ++i) {
			
			if (this.gameObject != homers[i]) {
				
				_homers.Add(homers[i]);
			}
		}
	}

	public override FiniteState CheckState() {

		if (
			_homers.Exists (
				(GameObject homer) => {

					float sMag = (this.gameObject.transform.position - homer.transform.position).sqrMagnitude;

					return _minSqr <= sMag && sMag <= _maxSqr;
				}
			)
		) {

			return _flock;
		}

		List<SteeringBehaviour> behvaiours = new List<SteeringBehaviour>();

		behvaiours.Add(new SteerWanderXZ(this.gameObject.transform));
		
		_steeringController.SetBehaviours(behvaiours);

		_steeringController.Steer();

		return this;
	}
}