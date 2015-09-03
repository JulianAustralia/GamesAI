using UnityEngine;
using System.Collections;

public class FiniteStateMachine : MonoBehaviour {
	
	public FiniteState initialState;

	private FiniteState _currentState;

	void Awake () {
	
		_currentState = initialState;
	}

	void Update () {
	
		_currentState = _currentState.update();
	}
}