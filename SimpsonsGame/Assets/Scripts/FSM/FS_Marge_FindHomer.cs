using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Marge_ReturnHomer))]
[RequireComponent(typeof(Enemy))]
public class FS_Marge_FindHomer : FiniteState {
	
	private SteeringController _steeringController;
	private FS_Marge_ReturnHomer _return;
	private Enemy _marge;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_return = GetComponent<FS_Marge_ReturnHomer>();
		_marge = GetComponent<Enemy>();
	}
	
	public override FiniteState CheckState() {

		// TODO

		return this;
	}
}