using UnityEngine;
using System.Collections;

public class FiniteStateMachine : MonoBehaviour {
	
	public FiniteState initialState;
	public FiniteState currentState;

	protected void Awake () {
	
		currentState = initialState;
	}

	public void UpdateState () {
	
		currentState = currentState.CheckState();
	}
}