using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public static int score = 0;
	public static int currentlevel = 1;
	int numLevels = 4;
	public int numBaddies = 0;
	public Text itemText;
	[HideInInspector] public bool noMoreEnemies = false;
	[HideInInspector] public int gunLevel = 1;
	public PlayerShooting playerShooting;

	private bool loaded = false;





	void Awake() {
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (gameObject);
		}
		gunLevel = 1;

//		playerShooting = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<PlayerShooting> ();
//		if (playerShooting != null) {
//			playerShooting.setGunLevel (gunLevel);
//		}
		DontDestroyOnLoad (gameObject);
		noMoreEnemies = false;
	}

	void Update() {
		if (noMoreEnemies == true && numBaddies == 0) {
			if (GameObject.FindGameObjectsWithTag("GunLevelUp").Length == 0 && 
				GameObject.FindGameObjectsWithTag ("GunPickUp").Length == 0 &&
				GameObject.FindGameObjectsWithTag ("HealthPickUp").Length == 0) {
				if (!loaded) {
					StartCoroutine (LoadAfterDelay());
					loaded = true;
				}
			} else {
				itemText.text = "Collect items to continue";
			}
		}
	}

	IEnumerator LoadAfterDelay() {
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("LevelCleared");
	}
			

	public static void ResetGame() {
		score = 0;
		currentlevel = 1;
		SceneManager.LoadScene ("HomeScreen");
	}

	public void IncreaseLevel (){
		if (currentlevel == numLevels) {
			currentlevel = 1;
			SceneManager.LoadScene ("GameOverScreen");
		} else {
			currentlevel += 1;
			SceneManager.LoadScene ("Level0" + currentlevel);
		}
		noMoreEnemies = false;
		loaded = false;
	}

}
