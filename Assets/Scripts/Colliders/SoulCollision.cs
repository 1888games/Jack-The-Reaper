using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCollision : MonoBehaviour
{

	public SoulsController soulsController;


	void OnTriggerEnter (Collider other) {

		if (other.gameObject.tag == "Player") {

			soulsController.SoulCollected (this.gameObject);

		}


	}


}
