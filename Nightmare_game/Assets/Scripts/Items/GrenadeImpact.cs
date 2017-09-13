using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeImpact : MonoBehaviour {

	public LayerMask shootableMask;
	public PlayerShooting shooter;
	public ParticleSystem explosionParticles;
	public AudioSource explosionAudio;
	public float particleDuration = 1.4f;


	float explosionRadius = 2f;
	EnemyHealth targetHealth;


	void Start() 
	{
		Destroy (gameObject, 2f);
	}

	void OnTriggerEnter(Collider other) {
		Collider[] targets = Physics.OverlapSphere (transform.position, explosionRadius, shootableMask);

		for (int i = 0; i < targets.Length; i++) {
			if (targets[i].gameObject.CompareTag ("Enemy")) {
				targetHealth = targets[i].gameObject.GetComponent<EnemyHealth> ();

				if (targetHealth != null) {
					targetHealth.TakeDamage (20, targetHealth.transform.position);
				}
			}
		}
		explosionParticles.transform.parent = null;

		explosionParticles.Play ();

		explosionAudio.Play ();
		Destroy (explosionParticles.gameObject, particleDuration);


		Destroy (gameObject);

	}
}
