using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringController))]
[RequireComponent(typeof(FS_Marge_FindHomer))]
[RequireComponent(typeof(Enemy))]
public class FS_Marge_ReturnHomer : FiniteState {
	
	private SteeringController _steeringController;
	private FS_Marge_FindHomer _find;
	private Enemy _marge;
	
	protected void Awake() {
		
		_steeringController = GetComponent<SteeringController>();
		_find = GetComponent<FS_Marge_FindHomer>();
		_marge = GetComponent<Enemy>();
	}
	
	public override FiniteState CheckState() {
		
		// TODO
		
		return this;
	}
}