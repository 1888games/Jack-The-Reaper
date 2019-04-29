using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }



	void OnTriggerEnter (Collider other) {

		//Debug.Log ("HIT SPAWN POINT!!!");

		if (other.gameObject.tag == "Robot") {

			//other.gameObject.GetComponent<ControlReaper> ().SetOnGround (true);
	
			other.gameObject.GetComponent<Robot> ().HitSpawnTrigger ();

		}


	}


}

