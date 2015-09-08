using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Homer_Wander))]
[RequireComponent(typeof(Homer))]
public class FS_Homer_Escape : FiniteState {

	private SteeringController _steeringController;
	private FS_Homer_Wander _wander;
	private Homer _homer;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_wander = GetComponent<FS_Homer_Wander>();
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

		_steeringController.AddBehaviours(new SteerAvoidBuildings(this.transform));

		_steeringController.Steer();
		
		return this;
	}
}
