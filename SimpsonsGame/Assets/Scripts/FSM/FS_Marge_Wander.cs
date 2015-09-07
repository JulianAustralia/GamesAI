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
	private List<Vector3> _path;
	private float _spawnRadius;
	private PathFinder _pathFinder;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_catch = GetComponent<FS_Marge_CatchHomer>();
		_marge = GetComponent<Marge>();

		GameObject dome = GameObject.Find("Dome");
		_spawnRadius = Mathf.Min(dome.transform.localScale.x / 2f, dome.transform.localScale.z / 2f) - 2;

		_pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
	}

	public void CreateNewPath() {

		do {
			float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
			float radius = UnityEngine.Random.Range(0, _spawnRadius);

			Vector3 to = new Vector3(Mathf.Sin(angle) * radius, 0, Mathf.Cos(angle) * radius);

			if (_pathFinder.ValidPosition(to) == false) {

				_path = null;

				continue;
			}

			_path = _pathFinder.FindPath(this.gameObject.transform.position, to);
		} while (_path == null || _path.Count == 0);
	}
	
	public override FiniteState CheckState() {

		Homer h = _marge.GetClosestHomer();

		if (h != null) {

			_catch.target = h;

			return _catch;
		}

		_steeringController.SetBehaviour(new SteerAlongPath(this.gameObject.transform, _path));
		
		_steeringController.Steer();
		
		return this;
	}
}