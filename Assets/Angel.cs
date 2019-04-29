using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{

	public Level level;
	public Rigidbody rigidbody;
	public GameObject reaper;

	public float lastDistance;

	public bool choosingDirection = false;

	public float flySpeed = 1f;
	public float turnTime = 0.9f;
	public float assessInterval = 2f;

	public string nextTurn = "";

	public List<string> platformsInWay;
	
    // Start is called before the first frame update
    void Awake()
    {	
    
    	reaper = GameObject.FindWithTag ("Player");
        rigidbody = GetComponent<Rigidbody> ();
		platformsInWay = new List<string> ();
		
        
        lastDistance = 0f;

		Invoke ("AssessDirection2", 1f);
    }

    // Update is called once per frame
    void Update()
    {

		if (reaper != null) {
			//AssessDirection ();
		} else {
		reaper = GameObject.FindWithTag ("Player");
		}
        
    }

	void AssessDirection2 () {

		if (reaper == null) {
			reaper = GameObject.FindWithTag ("Player");
			Invoke ("AssessDirection2", Random.Range (1f, 1.5f));
			return;
		}
			

		ChooseNewDirection ();

		Invoke ("AssessDirection2", Random.Range (1f,1.5f));
		

	}
	

	public void HitPlatform (string direction) {

		if (platformsInWay.Contains (direction) == false) {
			platformsInWay.Add (direction);
			ChooseNewDirection ();
		}

	}
	
	public void LeftPlatform (string direction) {

		if (platformsInWay.Contains (direction)) {
			platformsInWay.Remove(direction);
			ChooseNewDirection ();
		}

	}


	void ChooseNewDirection (string blocked = "") {

		if (reaper == null) {
			reaper = GameObject.FindWithTag ("Player");
		}

		if (choosingDirection == false && reaper != null) {
			
		

		
			Vector3 diff = (transform.position - reaper.transform.position);

			List<string> availableDirections = new List<string> { "Left", "Right", "Up", "Down" };

			availableDirections.Remove (nextTurn);

			foreach (string dir in platformsInWay) {
				availableDirections.Remove (dir);
			}

			if (availableDirections.Contains (blocked)) {
				availableDirections.Remove (blocked);
			}
		
			if (diff.x < 0f) {
				availableDirections.Remove ("Left");
			}

			if (diff.x > 0f) {
				availableDirections.Remove ("Right");
			}

			if (diff.y > 0f) {
				availableDirections.Remove ("Up");
			}

			if (diff.y < 0f) {
				availableDirections.Remove ("Down");
			}

			if (availableDirections.Count > 0) {

				string thisTurn = availableDirections [Random.Range (0, availableDirections.Count)];

				if (nextTurn != thisTurn) {
				
					choosingDirection = true;
					rigidbody.velocity = Vector3.zero;
					nextTurn = availableDirections [Random.Range (0, availableDirections.Count)];

					Invoke ("MoveAngel", turnTime);
				}
			}

		}
		
		

	}

	void MoveAngel () {

		if (nextTurn == "Left") {
			rigidbody.velocity = new Vector3 (-flySpeed, 0f, 0f);
		}

		if (nextTurn == "Right") {
			rigidbody.velocity = new Vector3 (flySpeed, 0f, 0f);
		}

		if (nextTurn == "Down") {
			rigidbody.velocity = new Vector3 (0f, -flySpeed, 0f);
		}

		if (nextTurn == "Up") {
			rigidbody.velocity = new Vector3 (0f, flySpeed, 0f);
		}

		Invoke ("AllowedToTurn", 1f);
	}

	void AllowedToTurn () {

		choosingDirection = false;
	}
	
	
	void AssessDirection () {


		
		float distance = Mathf.Abs(Vector3.Distance(transform.position, reaper.transform.position));

		Debug.Log (lastDistance + " " + distance);

		if (distance < lastDistance) {
		
			//return;
		}
		
	
		ChooseNewDirection ();
		
		lastDistance = distance;
		

	}
}
