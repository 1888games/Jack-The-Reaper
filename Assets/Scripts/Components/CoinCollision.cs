using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinCollision : MonoBehaviour
{	

	public bool collected = false;
	public GameObject enemy;
	public Level level;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

	

	void OnTriggerEnter (Collider other) {

		if (other.gameObject.tag == "Player" && collected == false) {

			collected = true;
			GameController.Instance.CollectEnemyCoin ();
			level.EnemyCoinCollected (enemy, this.gameObject);
			transform.DOScale (Vector3.zero, 0.3f);
			Invoke ("Destroy", 0.31f);
			
		
		}


	}

	void Destroy () {

		Destroy (this.gameObject);
	}

}

