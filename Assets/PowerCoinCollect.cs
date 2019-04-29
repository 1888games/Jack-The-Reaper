using UnityEngine;
using DG.Tweening;

public class PowerCoinCollect : MonoBehaviour
{	

	public bool collected = false;

	
    // Start is called before the first frame update
    void Start()
    {
        
    }

	

	void OnCollisionEnter (Collision other) {

		if (other.other.gameObject.tag == "Player" && collected == false) {

			GetComponent<Rigidbody> ().isKinematic = true;
			collected = true;
			GameController.Instance.CollectedPowerCoin ();
			transform.DOScale (Vector3.zero, 0.3f);
			Invoke ("Destroy", 0.31f);
			
		
		}


	}

	void Destroy () {

		Destroy (this.gameObject);
	}

}
