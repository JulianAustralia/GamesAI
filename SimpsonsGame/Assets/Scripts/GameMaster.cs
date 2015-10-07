using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameMaster : MonoBehaviour {

	public GameObject gameOverPanel;
	public Text timeText;

	public const float stageTime = 120;
	public const float updateFrequency = 1;

	private List<SafeZone> _zones;

	private float _timePast;
	private float _lastUpdate;

	// Use this for initialization
	void Start () {

		if (gameOverPanel != null) {

			gameOverPanel.SetActive (false);
		}

		_zones = GameObject.FindGameObjectsWithTag(
			"ScoreZone"
		).ToList<GameObject>(
		).ConvertAll<SafeZone>(
			(go) => go.GetComponent<SafeZone>()
		);

		_timePast = 0;
		_lastUpdate = -updateFrequency;
	}
	
	// Update is called once per frame
	void Update () {

		_timePast += Time.deltaTime;

		if (_lastUpdate + updateFrequency <= _timePast) {

			_lastUpdate += updateFrequency;

			_zones.ForEach((SafeZone zone) => zone.updateScore());

			if (timeText != null) {

				timeText.text = "Time Left: " + (int) (stageTime - _timePast);
			}
		}
		
		if (_timePast >= stageTime) {

			_zones.ForEach((SafeZone zone) => zone.gameOver());

			if (gameOverPanel != null) {

				gameOverPanel.SetActive(true);
			}
		}
	}
}
