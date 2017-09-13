using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelClearedManager : MonoBehaviour {

	public Text levelText;
	public Text scoreBox;
	public Image lindaPic;

	int score;
	int level;

	void Awake() {
		score = GameManager.score;
		level = GameManager.currentlevel;


		levelText.text = "Level " + level.ToString () + " Cleared!";
		scoreBox.text = "Score: " + score.ToString ();
	}

	public void NextLevel() {
		GameManager.instance.IncreaseLevel ();
	}

	void Update() {
		if (Input.GetKey (KeyCode.L) &&
			Input.GetKey (KeyCode.I) &&
			Input.GetKey (KeyCode.N) &&
			Input.GetKey (KeyCode.D) &&
			Input.GetKey (KeyCode.A)) {
			SceneManager.LoadScene ("BdayScene");
		}
	}

}
