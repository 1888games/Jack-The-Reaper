using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SoulsController : MonoBehaviour
{

	public List<GameObject> souls;
	public GameObject activeSoul;

	public float rotationSpeed;
	public int soulScore = 200;

	public InAudioNode soulCollectedSound;

	public GameObject scoreTextPrefab;

	public int soulMultipier = 0;
	public int freshSouls = 0;
	
	

	// Start is called before the first frame update
	void Start()
    {

		
		souls = new List<GameObject> ();
		

		foreach (Transform child in transform) {

			souls.Add (child.gameObject);

			child.GetComponent<SoulCollision> ().soulsController = this;

		}
		
		activeSoul = souls [0];
		   
    }

    // Update is called once per frame
    void Update()
    {

		if (activeSoul != null) {

			activeSoul.transform.eulerAngles = new Vector3 (0f, activeSoul.transform.eulerAngles.y + rotationSpeed, 0f);

		}
		
		
    }


	public void SoulCollected (GameObject go) {

		
		
		InAudio.Play (Camera.main.gameObject, soulCollectedSound);

		if (souls.Contains (go)) {
			souls.Remove (go);



			int score = soulScore;
			int soulMult = 1;
			soulMultipier++;
			if (activeSoul == go) {
				score = score * 2;
				soulMultipier++;
				soulMult++;
				freshSouls++;
			}

			GameController.Instance.AddToMultiplierCount (soulMult);


			GameController.Instance.ScorePoints (score);
			
			
			
			
			
			if (souls.Count > 0) {
				activeSoul = souls [0];
			} else {
				RoundComplete ();
			}
			go.transform.DOScale (Vector3.zero, 0.3f).OnComplete (() => DestroySoulGameObject (go));
			
		}
		
		
		
		
		
		
		
		

	}



	void DestroySoulGameObject ( GameObject go) {

		Destroy (go);


	}
	
	void RoundComplete() {

		Debug.Log ("ROUND COMPLETE!");

		if (freshSouls >= 20) {
			freshSouls = freshSouls - 19;
		} else {
			freshSouls = 0;
		}

		GameController.Instance.RoundComplete (freshSouls);
	}
}
