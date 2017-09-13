using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
	public Text pickUpText;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	public Color white1 = new Color (1f, 1f, 1f, .8f);


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;
	bool pickedUp = false;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

		if (pickedUp) {
			pickUpText.color = white1;
		} else {
			pickUpText.color = Color.Lerp (pickUpText.color, Color.clear, 2f * Time.deltaTime);
		}
		pickedUp = false;
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("HealthPickUp")) {
			if (currentHealth > 90) {
				currentHealth = 100;
			} else {
				currentHealth += 10;
			}
			healthSlider.value = currentHealth;
			pickUpText.text = "+10 hp";
			pickedUp = true;
			other.gameObject.SetActive (false);
			Destroy (other.gameObject);
		} 

		else if (other.gameObject.CompareTag ("GunPickUp")) {
			playerShooting.damagePerShot += 5;
			pickUpText.text = "+5 Damage";
			other.gameObject.SetActive (false);
			Destroy (other.gameObject);
			pickedUp = true;
		} 

		else if (other.gameObject.CompareTag ("GunLevelUp")) 
		{
			if (playerShooting.gunLevel < 3) {
				playerShooting.gunLevel += 1;
				pickUpText.text = "Gun Upgrade!";
				other.gameObject.SetActive (false);
				Destroy (other.gameObject);
				pickedUp = true;
			} else {
				pickUpText.text = "Gun Already Upgraded";
				other.gameObject.SetActive (false);
				Destroy (other.gameObject);
				pickedUp = true;
			}


		}
	}

    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects ();

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
		SceneManager.LoadScene ("GameOverScreen");

	}

}
