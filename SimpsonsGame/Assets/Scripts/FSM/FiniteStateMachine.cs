using UnityEngine;
using System.Collections;

public class FiniteStateMachine : MonoBehaviour {
	
	public FiniteState initialState;
	public FiniteState currentState;

	public void initialise() {

		currentState = initialState;
	}

	protected void Awake () {
	
		initialise();
	}

	public void UpdateState () {
	
		currentState = currentState.CheckState();
	}
}