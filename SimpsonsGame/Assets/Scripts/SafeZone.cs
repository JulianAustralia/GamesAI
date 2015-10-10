using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SafeZone : MonoBehaviour {

	public Enemy hunter;

	public Text scoreText;

	private HomerFactory _factory;
	private float _left;
	private float _right;
	private float _back;
	private float _front;

	public int score;

	public void initialise() {

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

		score = 0;
	}

	void Start () {

		_factory = GameObject.Find("HomerFactory").GetComponent<HomerFactory>();

		Vector3 p = transform.position;
		Vector3 s = transform.localScale;

		_left = p.x - s.x / 2;
		_right = p.x + s.x / 2;
		_back = p.z - s.z / 2;
		_front = p.z + s.z / 2;
	}

	public void updateScore () {

		_factory.homers.ForEach(
			(Homer h) => {

				float x = h.transform.position.x;
				float z = h.transform.position.z;

				if (x >= _left && x <= _right && z >= _back && z <= _front) {

					++score;
				}
			}
		);

		if (scoreText != null) {

			scoreText.text = "Score : " + score;
		}
	}

	public void gameOver() {
	}
}
