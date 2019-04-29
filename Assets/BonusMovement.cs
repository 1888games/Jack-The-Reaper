using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusMovement : MonoBehaviour
{

	public float speed;
	public Rigidbody rigidbody;
	
    // Start is called before the first frame update
    void Start()
    {

		int dirChoice = Random.Range (0, 4);

		rigidbody = GetComponent<Rigidbody> ();
	

		if (dirChoice == 0) {

			rigidbody.velocity = new Vector3 (1f, 1f, 0f);

		}
		
		if (dirChoice == 1) {

			rigidbody.velocity = new Vector3 (1f, -1f, 0f);

		}
		
		if (dirChoice == 2) {

			rigidbody.velocity = new Vector3 (-1f, 1f, 0f);

		}
		
		if (dirChoice == 3) {

			rigidbody.velocity = new Vector3 (-1f, -1f, 0f);

		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	private void FixedUpdate () {
		ControlSpeedOfBall ();
	}
	
	void ControlSpeedOfBall () {

		if (rigidbody.velocity.magnitude > 0f) {

			rigidbody.velocity = Vector3.ClampMagnitude (rigidbody.velocity * 100f, speed);

		}
	}
		
}
