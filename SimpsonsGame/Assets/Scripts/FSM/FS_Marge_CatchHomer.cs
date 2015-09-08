using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Marge_ReturnHomer))]
public class FS_Marge_CatchHomer : FiniteState {

	public const float distanceCatchHomer = 2f;
	public const float distanceToSteerToHomer = 3f;
	public Homer target;

	private SteeringController _steeringController;
	private FS_Marge_ReturnHomer _return;
	private PathFinder _pathFinder;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_return = GetComponent<FS_Marge_ReturnHomer>();

		_pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
	}
	
	public override FiniteState CheckState() {

		float sqrMag = (target.transform.position - this.transform.position).sqrMagnitude;

		if (sqrMag < distanceCatchHomer * distanceCatchHomer) {

			_return.CreateNewPath(target);

			return _return;
		} else if (sqrMag < distanceToSteerToHomer * distanceToSteerToHomer) {

			_steeringController.SetBehaviours(
				new List<SteeringBehaviour>() {
					new SteerAvoidBuildings(this.transform),
					new SteerToTarget(this.transform, target.transform)
				}
			);
		} else {

			List<Vector3> path = _pathFinder.FindPath (
				this.transform.position,
				target.transform.position// TODO try to move ahead of homer homervelocitynormalized * distancebetweenhomerandmarge / margespeed
			);

			_steeringController.SetBehaviours(
				new List<SteeringBehaviour>() {
					new SteerAvoidBuildings(this.transform),
					new SteerAlongPath(this.transform, path)
				}
			);
		}
		
		_steeringController.Steer();
		
		return this;
	}
}