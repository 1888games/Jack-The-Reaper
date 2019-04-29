using UnityEngine;

public class Robot : MonoBehaviour
{


	public int turnsBeforeFall = 3;
	public int totalTurnsBeforeFall = 3;
	public float delayBeforeWalking = 1f;
	public float initialDirection = 1f;
	public float walkSpeed = 2f;
	public Rigidbody rigidbody;
	public bool fallenOnce = false;

	public Level level;

	public bool flying = false;

	public float speed = 400f;
	
    // Start is called before the first frame update
    void Awake()
    {

		rigidbody = GetComponent<Rigidbody> ();
		//ResetRobot ();
		
		
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ResetRobot () {

		flying = false;
		fallenOnce = false;
		turnsBeforeFall = totalTurnsBeforeFall;
		rigidbody.useGravity = true;
		rigidbody.velocity = Vector3.zero;
		rigidbody.isKinematic = false;
		TriggerWalk ();
		initialDirection = 1f;
		

	}


	public void TriggerWalk () {

		Invoke ("StartWalking", delayBeforeWalking);
	}
	
	public void StartWalking () {
		
		turnsBeforeFall = totalTurnsBeforeFall;
		rigidbody.velocity = new Vector3 (initialDirection * walkSpeed, 0f, 0f);
		
	}
	

	public void HitSpawnTrigger () {

		Debug.Log ("HIT SPAWN " + name + " " + turnsBeforeFall + " " + fallenOnce);
		if (turnsBeforeFall == 0 && fallenOnce == false) {
			level.SpawnRobot ();
		}

	}


	public void TurnAround () {

		Debug.Log ("Turn around mate!");

		if (turnsBeforeFall > 0 && flying == false) {

			rigidbody.velocity = -rigidbody.velocity;
			turnsBeforeFall--;


		}

	}
	
	private void FixedUpdate () {
		ControlSpeedOfBall ();
	}
	
	void ControlSpeedOfBall () {

		if (flying && rigidbody.velocity.magnitude == 0f) {

			if (Random.Range (0, 2) == 0) {

				rigidbody.velocity = new Vector3 (1f, 1f, 0f);

			} else {
				rigidbody.velocity = new Vector3 (-1f, 1f, 0f);
			}

		}

		if (rigidbody.velocity.magnitude > 0f && flying) {

			rigidbody.velocity = Vector3.ClampMagnitude (rigidbody.velocity * 100f, speed);

		}
	}

	public void HitGround () {

		if (flying == false) {

			//rigidbody.isKinematic = true;
			Debug.Log ("Hit ground....");
			rigidbody.useGravity = false;

			if (Random.Range (0, 2) == 0) {

				rigidbody.velocity = new Vector3 (1f, 1f, 0f);

			} else {
				rigidbody.velocity = new Vector3 (-1f, 1f, 0f);
			}

			flying = true;

		}
	
		

	}
}
