using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FiniteStateMachine))]
public class FiniteState : MonoBehaviour {

	public abstract FiniteState update();
}