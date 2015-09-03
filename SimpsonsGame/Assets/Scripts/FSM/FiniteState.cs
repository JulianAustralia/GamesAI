using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FiniteStateMachine))]
public abstract class FiniteState : MonoBehaviour {

	public abstract FiniteState CheckState();
}