using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Marge_Wander))]
public class FS_Marge_ReturnHomer : FiniteState {

	private SteeringController _steeringController;
	private FS_Marge_Wander _wander;
	private Transform _dropPoint;
	private PathFinder _pathFinder;
	private List<Vector3> _path;
	private Homer _caughtHomer;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_wander = GetComponent<FS_Marge_Wander>();
		_dropPoint = GetComponent<Enemy>().zone.transform;
		_pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
	}

	public void CreateNewPath(Homer homer) {

		_caughtHomer = homer;
		homer.SetCaught(this.gameObject);

		_path = _pathFinder.FindPath(
			this.gameObject.transform.position,
			_dropPoint.position
		);

		_steeringController.SetBehaviours(
			new List<SteeringBehaviour>() {
				new SteerAvoidBuildings(this.transform),
				new SteerAlongPath(this.gameObject.transform, _path)
			}
		);
	}
	
	public override FiniteState CheckState() {

		if (_path.Count == 0) {

			_caughtHomer.SetWaiting(_dropPoint);

			return _wander;
		}

		if ((_caughtHomer.transform.position - this.transform.position).sqrMagnitude < 9) {

			_steeringController.Steer ();
		}
		
		return this;
	}
}