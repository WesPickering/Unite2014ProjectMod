using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject[] enemies;
    public float spawnTime = 2f;
    public Transform[] spawnPoints;

	public int numBunnies = 5;
	public int numBears = 3;
	public int numHells = 0;

	private int bunnySpawns;
	private int bearSpawns;
	private int HellSpawns;



    void Start ()
    {
		bunnySpawns = numBunnies;
		bearSpawns = numBears;
		HellSpawns = numHells;
		InvokeRepeating ("Spawn", 1f, spawnTime);
		GameManager.instance.numBaddies = numBunnies + numBears + numHells;
    }


    void Spawn ()
    {
        if(playerHealth.currentHealth <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		if (bunnySpawns > 0) {
			Instantiate (enemies [0], spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
			bunnySpawns -= 1;
		} else if (bearSpawns > 0) {
			Instantiate (enemies [1], spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
			bearSpawns -= 1;
		} else if (HellSpawns > 0) {
			Instantiate (enemies [2], spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
			HellSpawns -= 1;
		} else {
			GameManager.instance.noMoreEnemies = true;
		}
	
    }
}
