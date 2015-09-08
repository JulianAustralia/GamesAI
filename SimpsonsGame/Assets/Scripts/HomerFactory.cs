using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HomerFactory : MonoBehaviour {

	public int numberOfHomers;
	public List<Homer> homers;

	public float stageTime = 120;
	private int second = 1000;
	public int points = 0;

	public GameObject scoringArea;

	public GameObject scoreUI;
	public GameObject timerUI;
	public GameObject endGamePanel;
	public GameObject finalScoreUI;

	private Text scoreTxt;
	private Text timerTxt;
	private Text finalScore;

	void Start() {

		endGamePanel.SetActive (false);
	
		List<Enemy> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>().ConvertAll<Enemy>((go) => go.GetComponent<Enemy>());
		GameObject homer = GameObject.Find("Homer");
		GameObject dome = GameObject.Find("Dome");

		// Have a buffer of half (position will be centered at max) a Homer width between the edge of the dome and the max spawn position
		float spawnRadius = Mathf.Min(dome.transform.localScale.x / 2f, dome.transform.localScale.z / 2f) - 2 * Mathf.Max(homer.transform.localScale.x, homer.transform.localScale.z);

		homers = new List<Homer>(numberOfHomers);

		for (int i = 0; i < numberOfHomers; ++i) {

			float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
			float radius = UnityEngine.Random.Range(0, spawnRadius);

			GameObject newHomer = (GameObject) Instantiate(
				homer,
				new Vector3(
					Mathf.Sin(angle) * radius,
					homer.transform.position.y,
					Mathf.Cos(angle) * radius
				),
				Quaternion.identity
			);

			Vector3 euler = newHomer.transform.eulerAngles;
			euler.y = UnityEngine.Random.Range(0f, 360f);
			newHomer.transform.eulerAngles = euler;

			homers.Add(newHomer.GetComponent<Homer>());
		}
		
		// We could generate one less homer and randomly position this one
		// But this way is cleaner and this function will only be called once
		// per game so the performance hit is negligible
		GameObject.Destroy(homer);

		for (int i = 0; i < numberOfHomers; ++i) {

			List<Homer> otherHomers = new List<Homer>(homers);

			otherHomers.RemoveAt(i);

			homers[i].otherHomers = otherHomers;
			homers[i].enemies = enemies;
		}

		scoreTxt = scoreUI.GetComponent<Text>();
		timerTxt = timerUI.GetComponent<Text>();
		finalScore = finalScoreUI.GetComponent<Text>();
	}
	
	void Update () {
		if (stageTime > 0) {
			stageTime -= Time.deltaTime;
		} else {
			endGamePanel.SetActive (true);
			finalScore.text="Your score: " + points;
		}

		if (second > stageTime) {
			second = (int)stageTime;

			foreach(Homer homer in homers){
				if ((homer.gameObject.transform.position.x <= scoringArea.transform.position.x +5) && 
				    (homer.gameObject.transform.position.x >= scoringArea.transform.position.x -5) &&
				    (homer.gameObject.transform.position.z <= scoringArea.transform.position.z +5) && 
				    (homer.gameObject.transform.position.z >= scoringArea.transform.position.z -5))
					if (stageTime > 0)
						points++;
			}
		}

		scoreTxt.text="Score: " + points;
		timerTxt.text="Time Left: " + (int)stageTime;

		/*if (stageTime > 0) {
			Debug.Log(points);
			Debug.Log(stageTime);
		} else { 
			Debug.Log("Time is over");
			Debug.Log("Final points = " + points);
		}*/
	}
}
