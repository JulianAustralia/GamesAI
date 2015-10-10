using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GameMaster : MonoBehaviour {

	public GameObject gameOverPanel;
	public Text timeText;

	public const float stageTime = 6;
	public const float updateFrequency = 1;

	private HomerFactory _homerFactory;
	private List<SafeZone> _zones;

	private float _timePast;
	private float _lastUpdate;
	private bool _gameRunning = false;
	private ANNTrainer _nn = null;
	private Action<double> _callback = null;

	private void _startGame() {

		_homerFactory.CreateHomers();
		_gameRunning = false;
	}

	public void Start() {

		if (gameOverPanel != null) {
			
			gameOverPanel.SetActive(false);
		}
		
		_zones = GameObject.FindGameObjectsWithTag(
			"ScoreZone"
			).ToList<GameObject>(
			).ConvertAll<SafeZone>(
			(go) => go.GetComponent<SafeZone>()
			);
		
		_timePast = 0;
		_lastUpdate = -updateFrequency;

		_homerFactory = GameObject.Find("HomerFactory").GetComponent<HomerFactory>();

		const int populationCount = 16;
		List<int> layers = new List<int>();
		layers.Add(22); // Input layer
		layers.Add(16); // Hidden layer
		layers.Add(2); // Output layer
		const double crossOverChance = .05;
		const double mutateChance = .3;
		const double mutateMaxFactor = .4;
		const int generations = 100;
		Action<ANNTrainer, Action<double>> train = (ANNTrainer t, Action<double> callback) => {
		
			_nn = t;
			_callback = callback;
			_startGame();
		};

		new EO(
			populationCount,
			layers,
			crossOverChance,
			mutateChance,
			mutateMaxFactor,
			generations,
			train
		);
	}

	void Update () {

		if (!_gameRunning) return;

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

			_gameRunning = false;

			// Get Moes score
			// Get Burns score
			// result = max(moes, burns) - abs(moes - burns) / 4
			_callback(0);
		}
	}
}
