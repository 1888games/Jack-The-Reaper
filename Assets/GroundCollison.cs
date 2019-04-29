using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollison : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }



	void OnTriggerEnter (Collider other) {

		if (other.gameObject.tag == "Player") {

			other.gameObject.GetComponent<ControlReaper> ().SetOnGround (true);

		}

		if (other.gameObject.tag == "Robot") {

			if (gameObject.name == "ActualGround") {
				other.gameObject.GetComponent<Robot> ().HitGround();
			} else {
				other.gameObject.GetComponent<Robot> ().TriggerWalk ();
			}

		}


	}

	private void OnTriggerExit (Collider other) {

		if (other.gameObject.tag == "Player") {

			other.gameObject.GetComponent<ControlReaper> ().SetOnGround (false);
		}
		
		if (other.gameObject.tag == "Robot") {

			other.gameObject.GetComponent<Robot> ().rigidbody.velocity = Vector3.zero;
			if (other.gameObject.GetComponent<Robot> ().turnsBeforeFall == 0) {
				other.gameObject.GetComponent<Robot> ().fallenOnce = true;
			}

		}
	}
}


