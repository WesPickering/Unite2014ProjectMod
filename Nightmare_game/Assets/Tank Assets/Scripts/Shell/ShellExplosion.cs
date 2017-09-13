using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
		Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
		for (int i = 0; i < colliders.Length; i += 1) {
			Rigidbody target = colliders [i].GetComponent<Rigidbody> ();
			if (target) {
				target.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);

				TankHealth targetHP = target.GetComponent<TankHealth> ();
				if (!targetHP)
					continue;

				targetHP.TakeDamage (CalculateDamage (target.position));

			}
		}

		m_ExplosionParticles.transform.parent = null;
		m_ExplosionParticles.Play ();
		m_ExplosionAudio.Play ();
		Destroy (m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);
		Destroy (gameObject);
		 
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
		Vector3 diff = targetPosition -  transform.position;
		float holy = diff.magnitude;
		holy = m_MaxDamage * holy / m_ExplosionRadius;
		holy = Mathf.Max (holy, 0f);

        return holy;
    }
}