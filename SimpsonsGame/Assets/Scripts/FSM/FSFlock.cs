using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameObjectEdgeReader))]
[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSWander))]
public class FSFlock : FiniteState {

	public float minFlockDistance;
	public float maxFlockDistance;
	private SteeringController _steeringController;
	private GameObjectEdgeReader _edgeReader;
	private FSWander _wander;
	
	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_edgeReader = GetComponent<GameObjectEdgeReader>();
		_wander = GetComponent<FSWander>();
	}
	
	public override FiniteState CheckState() {

		List<GameObject> closeHomers = _edgeReader.GetEdges(this.gameObject, minFlockDistance, maxFlockDistance);

		if (closeHomers.Count == 0) {

			return _wander;
		}

		_steeringController.SetBehaviours(
			closeHomers.ConvertAll<SteeringBehaviour>(
				(GameObject homer) => new SteerToTarget(this.gameObject.transform, homer.transform)
			)
		);

		_steeringController.Steer();

		return this;
	}
}
