using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameObjectEdgeReader))]
[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FSFlock))]
public class FSWander : FiniteState {

	public float minFlockDistance;
	public float maxFlockDistance;
	private SteeringController _steeringController;
	private GameObjectEdgeReader _edgeReader;
	private FSFlock _flock;

	protected void Awake() {

		_steeringController = GetComponent<SteeringController>();
		_edgeReader = GetComponent<GameObjectEdgeReader>();
		_flock = GetComponent<FSFlock>();
	}

	public override FiniteState CheckState() {

		if (_edgeReader.GetEdges(this.gameObject, minFlockDistance, maxFlockDistance).Count > 0) {

			return _flock;
		}

		List<SteeringBehaviour> behvaiours = new List<SteeringBehaviour>();

		behvaiours.Add(new SteerWanderXZ(this.gameObject.transform));
		
		_steeringController.SetBehaviours(behvaiours);

		_steeringController.Steer();

		return this;
	}
}