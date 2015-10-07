using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SafeZone : MonoBehaviour {

	public Enemy hunter;

	public Text scoreText;

	private HomerFactory _factory;
	private int _score = 0;
	private float _left;
	private float _right;
	private float _back;
	private float _front;

	void Start () {

		_factory = GameObject.Find("HomerFactory").GetComponent<HomerFactory>();
		
		_left = transform.position.x - transform.localScale.x / 2;
		_right = transform.position.x + transform.localScale.x / 2;
		_back = transform.position.z - transform.localScale.z / 2;
		_front = transform.position.z + transform.localScale.z / 2;

		Vector3 newPosition = new Vector3(
			transform.position.x,
			hunter.transform.position.y,
			transform.position.z
		);

		Vector3 newRotation = new Vector3 (
			-transform.position.x,
			hunter.transform.position.y,
			-transform.position.z
		);

		hunter.transform.position = newPosition;

		hunter.transform.rotation = Quaternion.LookRotation(newRotation);
	}

	public void updateScore () {

		_factory.homers.ForEach(
			(Homer h) => {

				float x = h.transform.position.x;
				float z = h.transform.position.z;

				if (x >= _left && x <= _right && z >= _back && z <= _front) {

					++_score;
				}
			}
		);

		if (scoreText != null) {

			scoreText.text = "Score : " + _score;
		}
	}

	public void gameOver() {
	}
}
