using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }



	void OnTriggerEnter (Collider other) {

		if (other.gameObject.tag == "Angel") {

			other.transform.parent.GetComponent<Angel> ().HitPlatform (other.gameObject.name);

		}


	}

	private void OnTriggerExit (Collider other) {

		if (other.gameObject.tag == "Angel") {

			other.transform.parent.GetComponent<Angel> ().LeftPlatform (other.gameObject.name);

		}
	}
}
