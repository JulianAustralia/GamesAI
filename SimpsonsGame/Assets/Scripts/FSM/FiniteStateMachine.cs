using UnityEngine;
using System.Collections;

public class FiniteStateMachine : MonoBehaviour {
	
	public FiniteState initialState;

	private FiniteState _currentState;

	protected void Awake () {
	
		_currentState = initialState;
	}

	void Update () {
	
		_currentState = _currentState.CheckState();
	}
}