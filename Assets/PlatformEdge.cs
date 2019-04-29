using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEdge: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    
    
    
        
    }



	void OnTriggerEnter (Collider other) {

		if (other.gameObject.tag == "Robot") {

			//other.gameObject.GetComponent<ControlReaper> ().SetOnGround (true);
			//Debug.Log ("HIT ROBOT");
			other.gameObject.GetComponent<Robot> ().TurnAround ();

		}


	}


}


