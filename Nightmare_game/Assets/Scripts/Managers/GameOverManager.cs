using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
	public float restartDelay = 3f;
	private float restartTimer = 0;


    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
			restartTimer += Time.deltaTime;
			if (restartTimer > restartDelay) {
				SceneManager.LoadScene ("GameOverScreen");
			}
        }
    }
}
