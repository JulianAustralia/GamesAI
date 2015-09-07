using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Marge_Wander))]
public class FS_Marge_ReturnHomer : FiniteState {

	private SteeringController _steeringController;
	private FS_Marge_Wander _wander;
	private Vector3 _dropPoint;
	private PathFinder _pathFinder;
	private List<Vector3> _path;
	private Homer _caughtHomer;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_wander = GetComponent<FS_Marge_Wander>();
		_dropPoint = GameObject.Find("MargeDropPoint").transform.position;
		_pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
	}

	public void CreateNewPath(Homer homer) {

		_caughtHomer = homer;
		homer.SetCaught(this.gameObject);

		_path = _pathFinder.FindPath(
			this.gameObject.transform.position,
			_dropPoint
		);

		_steeringController.SetBehaviour(new SteerAlongPath(this.gameObject.transform, _path));
	}
	
	public override FiniteState CheckState() {

		if (_path.Count == 0) {

			// TODO Change Homer's state to wait

			return _wander;
		}

		if ((_caughtHomer.transform.position - this.transform.position).sqrMagnitude < 9) {

			_steeringController.Steer ();
		}
		
		return this;
	}
}