using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GameMaster : MonoBehaviour {

	public GameObject gameOverPanel;
	public Text timeText;

	public const float stageTime = 150;
	public const float updateFrequency = 1;

	private int _buildingMask;
	private GameObject _bart;
	private SafeZone _bartZone;
	private GameObject _marge;
	private SafeZone _margeZone;
	private GameObject _moe;
	private SafeZone _moeZone;
	private Movement _moeMovement;
	private GameObject _burns;
	private SafeZone _burnsZone;
	private Movement _burnsMovement;
	private HomerFactory _homerFactory;
	private List<SafeZone> _zones;
	private List<FiniteStateMachine> _machines;

	private float _timePast;
	private float _lastUpdate;
	private bool _gameRunning = false;
	private ANNTrainer _nn = null;
	private Action<double> _callback = null;

	private void _startGame() {

		_homerFactory.CreateHomers();

		_marge.GetComponent<Marge>().initialise();

		_machines = _homerFactory.homers.ConvertAll<FiniteStateMachine>(h => h.GetComponent<FiniteStateMachine>());
		_machines.Add(_marge.GetComponent<FiniteStateMachine>());

		_zones.ForEach((SafeZone s) => s.initialise());

		_timePast = 0;
		_lastUpdate = -updateFrequency;
		_gameRunning = true;
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

		_buildingMask = 1 << LayerMask.NameToLayer("Building");

		_bart = GameObject.Find("Bart");
		_bartZone = _bart.GetComponent<Enemy>().zone;
		_marge = GameObject.Find("Marge");
		_margeZone = _marge.GetComponent<Enemy>().zone;
		_moe = GameObject.Find("Moe");
		_moeZone = _moe.GetComponent<Enemy>().zone;
		_moeMovement = _moe.GetComponent<Movement>();
		_burns = GameObject.Find("Burns");
		_burnsZone = _burns.GetComponent<Enemy>().zone;
		_burnsMovement = _burns.GetComponent<Movement>();

		_homerFactory = GameObject.Find("HomerFactory").GetComponent<HomerFactory>();

		const int populationCount = 10;
		List<int> layers = new List<int>();
		layers.Add(8); // Input layer
		layers.Add(18); // Hidden layer
		layers.Add(2); // Output layer
		const double mutateChance = .3;
		const double mutateMaxFactor = .4;
		const int generations = 1000;
		Action<ANNTrainer, Action<double>> train = (ANNTrainer t, Action<double> callback) => {
		
			_nn = t;
			_callback = callback;
			_startGame();
		};

		new EO(
			populationCount,
			layers,
			mutateChance,
			mutateMaxFactor,
			generations,
			train
		);

		Time.timeScale = 1;
	}
	
	private void _swap(ref Vector3 a, ref Vector3 b) {
		
		Vector3 t = a;
		a = b;
		b = t;
	}
	
	private void _swap(ref double a, ref double b) {
		
		double t = a;
		a = b;
		b = t;
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

			int ms = _moeZone.score;
			int bs = _burnsZone.score;

			double score = Math.Max(ms, bs) - Math.Abs(ms - bs);// / 4 - _margeZone.score / 2 - _bartZone.score / 2;

			_callback(score);
		} else {

			_bart.transform.position = new Vector3(200, 2, 200);

			_machines.ForEach(m => m.UpdateState());

			double timeRemaing = (double) (stageTime - _timePast);
			double homersInBart = (double) _bartZone.scoreChange;
			double homersInMarge = (double) _margeZone.scoreChange;
			double homersInMoe = (double) _moeZone.scoreChange;
			double homersInBurns = (double) _burnsZone.scoreChange;

			double moeClosestH1D = Mathf.Infinity;
			Vector3 moeClosestH1 = Vector3.zero;
			double moeClosestH2D = Mathf.Infinity;
			Vector3 moeClosestH2 = Vector3.zero;
			double moeClosestH3D = Mathf.Infinity;
			Vector3 moeClosestH3 = Vector3.zero;

			double burnsClosestH1D = Mathf.Infinity;
			Vector3 burnsClosestH1 = Vector3.zero;
			double burnsClosestH2D = Mathf.Infinity;
			Vector3 burnsClosestH2 = Vector3.zero;
			double burnsClosestH3D = Mathf.Infinity;
			Vector3 burnsClosestH3 = Vector3.zero;

			Vector3 mtp = _moe.transform.position;
			Vector3 btp = _burns.transform.position;

			_homerFactory.homers.ForEach(
				h => {

					if (h.capturer != _moe) {

						Vector3 mhp = new Vector3(
							h.transform.position.x,
							h.transform.position.y,
							h.transform.position.z
						);

						double mdx = mtp.x - mhp.x;
						double mdz = mtp.z - mhp.z;

						double md = mdx * mdx + mdz * mdz;

						if (md < moeClosestH1D) {

							_swap(ref md, ref moeClosestH1D);
							_swap(ref mhp, ref moeClosestH1);
						}

						if (md < moeClosestH2D) {

							_swap(ref md, ref moeClosestH2D);
							_swap(ref mhp, ref moeClosestH2);
						}

						if (md < moeClosestH3D) {

							_swap(ref md, ref moeClosestH3D);
							_swap(ref mhp, ref moeClosestH3);
						}
					}

					if (h.capturer != _burns) {

						Vector3 bhp = new Vector3(
							h.transform.position.x,
							h.transform.position.y,
							h.transform.position.z
						);

						double bdx = btp.x - bhp.x;
						double bdz = btp.z - bhp.z;

						double bd = bdx * bdx + bdz * bdz;

						if (bd < burnsClosestH1D) {

							_swap(ref bd, ref burnsClosestH1D);
							_swap(ref bhp, ref burnsClosestH1);
						}

						if (bd < burnsClosestH2D) {

							_swap(ref bd, ref burnsClosestH2D);
							_swap(ref bhp, ref burnsClosestH2);
						}

						if (bd < burnsClosestH3D) {

							_swap(ref bd, ref burnsClosestH3D);
							_swap(ref bhp, ref burnsClosestH3);
						}
					}
				}
			);

			const float raycastLength = 4;
			const float angleOffset = 40;

			Vector3 mangle = _moe.transform.eulerAngles;
			bool moeObstacleFront = Physics.Raycast(mtp, _moe.transform.forward, raycastLength, _buildingMask);
			mangle.y += angleOffset;
			_moe.transform.eulerAngles = mangle;
			bool moeObstacleFrontLeft = Physics.Raycast(mtp, _moe.transform.forward, raycastLength, _buildingMask);
			mangle.y -= 2 * angleOffset;
			_moe.transform.eulerAngles = mangle;
			bool moeObstacleFrontRight = Physics.Raycast(mtp, _moe.transform.forward, raycastLength, _buildingMask);
			mangle.y += angleOffset;
			_moe.transform.eulerAngles = mangle;

			double [] moeInput = {
				//timeRemaing,
				//mtp.x,
				//mtp.z,
				//mangle.y,
				//homersInMoe,
				(_moeZone.transform.position.x - mtp.x),
				(_moeZone.transform.position.z - mtp.z),
				//homersInMarge,
				//(_margeZone.transform.position.x - mtp.x),
				//(_margeZone.transform.position.z - mtp.z),
				//homersInBurns,
				//(_burnsZone.transform.position.x - mtp.x),
				//(_burnsZone.transform.position.z - mtp.z),
				//homersInBart,
				//(_bartZone.transform.position.x - mtp.x),
				//(_bartZone.transform.position.z - mtp.z),
				moeClosestH1.x - mtp.x,
				moeClosestH1.z - mtp.z,
				moeClosestH2.x - mtp.x,
				moeClosestH2.z - mtp.z,
				moeClosestH3.x - mtp.x,
				moeClosestH3.z - mtp.z,
				//moeObstacleFront ? 1 : 0,
				//moeObstacleFrontLeft ? 1 : 0,
				//moeObstacleFrontRight ? 1 : 0
			};

			Vector3 bangle = _burns.transform.eulerAngles;
			bool burnsObstacleFront = Physics.Raycast(btp, _burns.transform.forward, raycastLength, _buildingMask);
			bangle.y += angleOffset;
			_burns.transform.eulerAngles = bangle;
			bool burnsObstacleFrontLeft = Physics.Raycast(btp, _burns.transform.forward, raycastLength, _buildingMask);
			bangle.y -= 2 * angleOffset;
			_burns.transform.eulerAngles = bangle;
			bool burnsObstacleFrontRight = Physics.Raycast(btp, _burns.transform.forward, raycastLength, _buildingMask);
			bangle.y += angleOffset;
			_burns.transform.eulerAngles = bangle;

			double [] burnsInput = {
				//timeRemaing,
				//btp.x,
				//btp.z,
				//bangle.y,
				//homersInBurns,
				_burnsZone.transform.position.x - btp.x,
				_burnsZone.transform.position.z - btp.z,
				//homersInBart,
				//_bartZone.transform.position.x - btp.x,
				//_bartZone.transform.position.z - btp.z,
				//homersInMoe,
				//_moeZone.transform.position.x - btp.x,
				//_moeZone.transform.position.z - btp.z,
				//homersInMarge,
				//_margeZone.transform.position.x - btp.x,
				//_margeZone.transform.position.z - btp.z,
				burnsClosestH1.x - btp.x,
				burnsClosestH1.z - btp.z,
				burnsClosestH2.x - btp.x,
				burnsClosestH2.z - btp.z,
				burnsClosestH3.x - btp.x,
				burnsClosestH3.z - btp.z,
				//burnsObstacleFront ? 1 : 0,
				//burnsObstacleFrontLeft ? 1 : 0,
				//burnsObstacleFrontRight ? 1 : 0
			};
			
			Matrix moeInputM = new Matrix(moeInput.Count(), 1, moeInput);
			Matrix burnsInputM = new Matrix(burnsInput.Count(), 1, burnsInput);
			
			Matrix moeMove = _nn.nn.getResult(moeInputM);
			Matrix burnsMove = _nn.nn.getResult(burnsInputM);

			Vector3 moeMoveV = new Vector3(
				(float) moeMove.getValue(0, 0),
				0f,
				(float) moeMove.getValue(1, 0)
			);
			Vector3 burnsMoveV = new Vector3(
				(float) burnsMove.getValue(0, 0),
				0f,
				(float) burnsMove.getValue(1, 0)
			);

			_moeMovement.Move(moeMoveV);
			_burnsMovement.Move(burnsMoveV);
		}
	}
}
