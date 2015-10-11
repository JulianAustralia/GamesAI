using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Marge_CatchHomer))]
[RequireComponent(typeof(Marge))]
public class FS_Marge_Wander : FiniteState {
	
	private SteeringController _steeringController;
	private FS_Marge_CatchHomer _catch;
	private Marge _marge;
	private List<Vector3> _path = null;
	private PathFinder _pathFinder;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_catch = GetComponent<FS_Marge_CatchHomer>();
		_marge = GetComponent<Marge>();

		_pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
	}

	public void CreateNewPath() {

		PathNode positionPN = _pathFinder.GetRandomPosition();

		_path = _pathFinder.FindPath(
			transform.position,
			new Vector3(
				positionPN.x,
				transform.position.y,
				positionPN.z
			)
		);
	}
	
	public override FiniteState CheckState() {

		Homer h = _marge.GetClosestHomer();

		if (h != null) {

			_catch.target = h;

			return _catch;
		}

		if (_path == null || _path.Count == 0) {
			
			CreateNewPath();

			// If is possible Marge is in a position where a path cannot be determined.
			// If this happens move Marge a little bit and try again
			if (_path == null || _path.Count == 0) {
			
				_steeringController.SetBehaviours (
					new List<SteeringBehaviour> () {
						new SteerAvoidBuildings(this.transform),
						new SteerWanderXZ(this.transform)
					}
				);

				_steeringController.Steer();

				return this;
			}
		}

		_steeringController.SetBehaviours(
			new List<SteeringBehaviour>() {
				new SteerAvoidBuildings(this.transform),
				new SteerAlongPath(this.gameObject.transform, _path)
			}
		);

		_steeringController.Steer();
		
		return this;
	}
}