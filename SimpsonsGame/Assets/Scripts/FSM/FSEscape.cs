using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSWander))]
public class FSEscape : FiniteState {

	private SteeringController _steeringController;
	private FSWander _wander;
	private Homer _homer;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_wander = GetComponent<FSWander>();
		_homer = GetComponent<Homer>();
	}
	
	public override FiniteState CheckState() {
		
		List<Enemy> tooCloseEnemies = _homer.GetTooCloseEnemies();
		
		if (tooCloseEnemies.Count == 0) return _wander;
		
		_steeringController.SetBehaviours(
			tooCloseEnemies.ConvertAll<SteeringBehaviour>(
				(e) => new SteerFromTarget(this.gameObject.transform, e.transform)
			)
		);

		_steeringController.Steer();
		
		return this;
	}
}
