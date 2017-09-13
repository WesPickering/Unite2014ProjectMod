using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;            // The speed that the player will move at.

	Vector3 movement;                   // The vector to store the direction of the player's movement.
	Animator anim;                      // Reference to the animator component.
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	float camRayLength = 100f;          // The length of the ray from the camera into the scene.

	void Awake ()
	{
		floorMask = LayerMask.GetMask ("Floor");

		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	}


	void FixedUpdate ()
	{
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		Move (h, v);

		Turning ();

		Animating (h, v);
	}

	void Move (float h, float v)
	{
		movement.Set (h, 0f, v);

		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turning ()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;

		bool raycasted = Physics.Raycast (camRay, out floorHit, camRayLength, floorMask);
		if(raycasted)
		{
			Vector3 playerToMouse = floorHit.point - transform.position;

			playerToMouse.y = 0f;

			Quaternion newRotatation = Quaternion.LookRotation (playerToMouse);

			playerRigidbody.MoveRotation (newRotatation);
		}
	}

	void Animating (float h, float v)
	{
		bool walking = h != 0f || v != 0f;
		anim.SetBool ("IsWalking", walking);
	}
}