using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlReaper : MonoBehaviour
{

	public bool onGround = true;
	public float yRotationTarget = 0f;
	public bool fireButtonDown = false;

	public List<string> jumpButtonNames;

	public float fireButtonTime = 0f;

	public float xAxisValue = 0f;

	public float runSpeed = 0.1f;
	public float flySpeed = 0.15f;
	public float turnSpeed = 1f;
	public float yRotationMax = 60f;

	public float jumpMinVelocity = 50f;
	public float jumpMaxVelocity = 80f;
	public float fireButtonMaxTime = 50f;
	public float fireButtonMinTime = 0.04f;

	public bool jumping = true;
	public bool isDead = false;

	public Animator animator;
	
	
	public Rigidbody rigidbody;

	public InAudioNode jumpSound;
	public InAudioNode walkSound;

	float walkCoolDown = 0;
	public float walkGap = 0.4f;

	public Vector3 spawnPoint;
	
    // Start is called before the first frame update
    void Awake()
    {

		rigidbody = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();

		spawnPoint = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
		if (isDead == false) {
			CheckFireButton ();
			CheckMovement ();
			CheckRotation ();

			if (walkCoolDown > 0f) {
				walkCoolDown -= Time.deltaTime;
			}

		}
        
    }

	public void Reset () {

		onGround = false;
		jumping = false;
		walkCoolDown = 0f;

		transform.position = spawnPoint;
		transform.eulerAngles = Vector3.zero;
		isDead = false;


	}


	void CheckRotation () {

		

	}


	void OnCollisionEnter (Collision collision) {

		if (collision.other.gameObject.layer == 9 && isDead == false) {

			//rigidbody.isKinematic = true;
			
			animator.SetBool ("IsDead", true);
			GameController.Instance.ReaperDied ();
			isDead = true;

		}




	}
	
	void CheckMovement () {

		float previousxAxis = xAxisValue;
		
		xAxisValue = Input.GetAxis ("Horizontal");

		if (xAxisValue > 0f && xAxisValue < 1f) {
			xAxisValue = 1f;
		} else {
			if (xAxisValue > -1f && xAxisValue <0f) {
				xAxisValue = -1f;
			}
		}
	
		

		if (xAxisValue != 0f) {

			if (previousxAxis != 1f && previousxAxis != -1f) {

				transform.eulerAngles = new Vector3 (0f, yRotationMax * -xAxisValue, 0f);
				walkCoolDown = walkGap;

			}

		} else {
			transform.eulerAngles = Vector3.zero;
		}
		
		
		

	}

	void FixedUpdate () {

		if (onGround) {
			rigidbody.velocity = new Vector3 ((runSpeed * xAxisValue) / Time.deltaTime, rigidbody.velocity.y, 0f);

			if (walkCoolDown <= 0f && xAxisValue != 0f ) {

				InAudio.Play (Camera.main.gameObject, walkSound);
				walkCoolDown = walkGap;
			}
			
			
			
		} else {
			rigidbody.velocity = new Vector3 ((flySpeed * xAxisValue) / Time.deltaTime, rigidbody.velocity.y, 0f);
		}
		
			

	}

	public void SetOnGround (bool setting) {

		onGround = setting;

		animator.SetBool ("OnGround", setting);

		if (onGround) {
			jumping = false;
		}
		

	}
	
	void CheckFireButton () {

		if (Input.GetButtonDown ("Fire1")) {
			fireButtonDown = true;
			ProcessFireButton ();
			fireButtonTime = 0f;
		}

		if (Input.GetButton ("Fire1") && jumping) {

			if (fireButtonTime > fireButtonMinTime && fireButtonTime < fireButtonMaxTime) {
				rigidbody.velocity = new Vector3 (rigidbody.velocity.x, rigidbody.velocity.y + 8f * Time.deltaTime, 0f);
			}

			
		}

		if (Input.GetButtonUp ("Fire1")) {

			jumping = false;
			fireButtonDown = false;
			fireButtonTime = 0f;
		}


		if (fireButtonDown) {
			fireButtonTime += Time.deltaTime;
		}

	}


	void ProcessFireButton () {

		if (onGround) {

			rigidbody.velocity = new Vector3 (rigidbody.velocity.x, jumpMinVelocity, 0f);
			SetOnGround(false);
			InAudio.Play (Camera.main.gameObject, jumpSound);
			jumping = true;

		} else {
			rigidbody.velocity = new Vector3 (rigidbody.velocity.x, 0.0f, 0f);
		}
	}
    
}
