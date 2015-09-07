using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Marge_ReturnHomer))]
[RequireComponent(typeof(Marge))]
public class FS_Marge_CatchHomer : FiniteState {

	public Homer target;

	private SteeringController _steeringController;
	private FS_Marge_ReturnHomer _return;
	private Marge _marge;
	private PathFinder _pathFinder;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_return = GetComponent<FS_Marge_ReturnHomer>();
		_marge = GetComponent<Marge>();

		_pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
	}
	
	public override FiniteState CheckState() {

		List<Vector3> path = _pathFinder.FindPath (
			this.gameObject.transform.position,
			target.gameObject.transform.position
		);

		if (path.Count == 0) {

			_return.CreateNewPath(target);

			return _return;
		}

		_steeringController.SetBehaviour(new SteerAlongPath(this.gameObject.transform, path));
		
		_steeringController.Steer();
		
		return this;
	}
}