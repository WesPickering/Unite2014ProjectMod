using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
	public static PlayerShooting instance = null;
    public int damagePerShot = 20;
	public float timeBetweenSwitching = .6f;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
	public float grenadeLaunchForce = 13f;
	public Rigidbody grenada;
	public LineRenderer gunLine2;
	public LineRenderer gunLine3;
	public Text gunText;
	[HideInInspector] public int gunLevel = 1;


	int gunSelect = 0;
	int numGuns = 3;
    float timer;
	float effectsDisplayTime = 0.2f;
	float reloadGun1 = .25f;
	float reloadGun2 = .5f;
	float reloadGun3 = .75f;

	string[] gunNames = { "Machine Gun", "Shotgun", "Grenade Launcher" };

	int shootableMask;
	Light gunLight;
	ParticleSystem gunParticles;
	AudioSource gunAudio;
	Ray bullet1 = new Ray();
	Ray bullet2 = new Ray();
	Ray bullet3 = new Ray();
    RaycastHit bullet1Info;
	RaycastHit bullet2Info;
	RaycastHit bullet3Info;
    LineRenderer gunLine1;




    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
		gunLine1 = GetComponent<LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
		gunText.text = gunNames [gunSelect];


    }


    void Update ()
    {
        timer += Time.deltaTime;

		//If player wants to switch guns
		if (Input.GetButton ("SwitchPrev") && timer >= timeBetweenSwitching && Time.timeScale != 0) {
			SwitchPrev ();
		} else if (Input.GetButton ("SwitchNext") && timer >= timeBetweenSwitching && Time.timeScale != 0) {
			SwitchNext();
		}

		//if player wants to fire selected gun
		if (Input.GetButton("Fire1")) {
			if (gunSelect == 0 && timer >= reloadGun1 && Time.timeScale != 0) {
				ShootGun1 ();
			} else if (gunSelect == 1 && timer >= reloadGun2 && Time.timeScale != 0) {
				ShootGun2 ();
			} else if (gunSelect == 2 && timer >= reloadGun3 && Time.timeScale != 0) {
				ShootGun3 ();
			}
		}

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine1.enabled = false;
		gunLine2.enabled = false;
		gunLine3.enabled = false;
        gunLight.enabled = false;
    }

	public void setGunLevel(int x){
		gunLevel = x;
	}


    void ShootGun1 ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine1.enabled = true;
        gunLine1.SetPosition (0, transform.position);

		bullet1.origin = transform.position;
		bullet1.direction = transform.forward;
		if(Physics.Raycast (bullet1, out bullet1Info, range, shootableMask))
        {
            EnemyHealth enemyHealth = bullet1Info.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage (damagePerShot, bullet1Info.point);
            }
            gunLine1.SetPosition (1, bullet1Info.point);
        }
        else
        {
			gunLine1.SetPosition (1, bullet1.origin + bullet1.direction * range);
        }

		if (gunLevel >= 2) {
			gunLine2.enabled = true;
			gunLine2.SetPosition (0, transform.position);

			bullet2.origin = transform.position;
			bullet2.direction = Quaternion.Euler (0, 10, 0) * transform.forward;

			if (Physics.Raycast (bullet2, out bullet2Info, range, shootableMask)) {
				EnemyHealth enemyHealth2 = bullet2Info.collider.GetComponent<EnemyHealth> ();
				if (enemyHealth2 != null) {
					enemyHealth2.TakeDamage (damagePerShot, bullet2Info.point);
				}
				gunLine2.SetPosition (1, bullet2Info.point);
			} else {
				gunLine2.SetPosition (1, bullet2.origin + bullet2.direction * range);
			}
		}

		if (gunLevel >= 3) {
			gunLine3.enabled = true;
			gunLine3.SetPosition (0, transform.position);

			bullet3.origin = transform.position;
			bullet3.direction = Quaternion.Euler (0, -10, 0) * transform.forward;

			if (Physics.Raycast (bullet3, out bullet3Info, range, shootableMask)) {
				EnemyHealth enemyHealth3 = bullet3Info.collider.GetComponent<EnemyHealth> ();
				if (enemyHealth3 != null) {
					enemyHealth3.TakeDamage (damagePerShot, bullet3Info.point);
				}
				gunLine3.SetPosition (1, bullet3Info.point);
			} else {
				gunLine3.SetPosition (1, bullet3.origin + bullet3.direction * range);
			}
		}
    }

	void ShootGun2 ()
	{
		timer = 0f;

		gunAudio.Play ();

		gunLight.enabled = true;

		gunParticles.Stop ();
		gunParticles.Play ();

		Collider[] shotgunTargets = Physics.OverlapSphere (transform.position + new Vector3 (0, 0, 3),
			                            4.5f, shootableMask);
		EnemyHealth shotgunHit;
		for (int i = 0; i < shotgunTargets.Length; i++) {
			shotgunHit = shotgunTargets [i].gameObject.GetComponent<EnemyHealth> ();
			if (shotgunHit != null) {
				shotgunHit.TakeDamage (damagePerShot, shotgunHit.transform.position);
			}
		}
	}


	void ShootGun3() 
	{
		timer = 0f;

		gunAudio.Play ();

		gunLight.enabled = true;
		gunParticles.Stop ();
		gunParticles.Play ();

		Rigidbody grenadeInstance = Instantiate (grenada, transform.position, transform.rotation) as Rigidbody;
		grenadeInstance.position += new Vector3 (0, 1f, 0);

		grenadeInstance.velocity = grenadeLaunchForce * grenadeInstance.transform.forward;

	}

	void SwitchPrev	() {
		timer = 0f;
		if (gunSelect == 0) {
			gunSelect = numGuns - 1;
		} else {
			gunSelect -= 1;
		}
		gunText.text = gunNames [gunSelect];

	}

	void SwitchNext() {
		timer = 0f;
		if (gunSelect == numGuns - 1) {
			gunSelect = 0;
		} else {
			gunSelect += 1;
		}
		gunText.text = gunNames [gunSelect];

	}

}
