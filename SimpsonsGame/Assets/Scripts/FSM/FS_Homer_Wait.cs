using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Homer_Escape))]
[RequireComponent(typeof(Homer))]
public class FS_Homer_Wait : FiniteState {

	private SteeringController _steeringController;
	private Marge _marge;
	private Homer _homer;
	private FS_Homer_Escape _escape;
	private bool _avoidingMarge;
	private Transform _waitPoint;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_marge = GameObject.Find("Marge").GetComponent<Marge>();
		_homer = this.GetComponent<Homer>();
		_escape = this.GetComponent<FS_Homer_Escape>();
	}

	public void StartWaiting(Transform waitPoint) {

		_avoidingMarge = false;
		_waitPoint = waitPoint;
	}
	
	public override FiniteState CheckState() {

		if (_homer.GetTooCloseEnemies().Exists((Enemy e) => e.gameObject != _marge.gameObject)) {

			_homer.disabled = false;
			return _escape;
		}

		if ((_marge.transform.position - this.transform.position).sqrMagnitude < 9) {

			if (_avoidingMarge == false) {

				_steeringController.SetBehaviour(new SteerFromTarget(this.transform, _marge.transform));
				_avoidingMarge = true;
			}
		} else {

			if (_avoidingMarge == true) {

				_steeringController.SetBehaviour(new SteerToTarget(this.transform, _waitPoint));
				_avoidingMarge = false;
			}
		}
		
		_steeringController.Steer();
		
		return this;
	}
}
