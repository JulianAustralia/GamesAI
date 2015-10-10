using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GameMaster : MonoBehaviour {

	public GameObject gameOverPanel;
	public Text timeText;

	public const float stageTime = 120;
	public const float updateFrequency = 1;

	private EO _eo;

	private List<SafeZone> _zones;

	private float _timePast;
	private float _lastUpdate;

	public GameMaster() {

		const int populationCount = 16;
		List<int> layers = new List<int>();
		layers.Add(22); // Input layer
		layers.Add(16); // Hidden layer
		layers.Add(2); // Output layer
		const double crossOverChance = .05;
		const double mutateChance = .3;
		const double mutateMaxFactor = .4;
		const int generations = 100;
		Func<ANNTrainer, ANNTrainer> train = (ANNTrainer t) => {

			// TODO

			return t;
		};


		_eo = new EO(
			populationCount,
			layers,
			crossOverChance,
			mutateChance,
			mutateMaxFactor,
			generations,
			train
		);
	}

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
