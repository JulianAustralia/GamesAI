using UnityEngine;
using System.Collections;

public class FiniteStateMachine : MonoBehaviour {
	
	public FiniteState initialState;
	public FiniteState currentState;

	protected void Awake () {
	
		currentState = initialState;
	}

	void Update () {
	
		currentState = currentState.CheckState();
	}
}