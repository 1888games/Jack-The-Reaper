using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{

	public List<GameObject> robotSpawnPoints;
	public List<GameObject> angelSpawnPoints;
	
	
	public Vector3 robotSpawnPoint;
	public  GameObject reaperSpawnPoint;

	public float robotSpawnInterval = 5f;
	public int robotsToSpawn = 4;
	public int robotsLeft = 4;
	public float firstRobotSpawn = 0f;
	public float robotTimer = 0f;
	public int robotSpawnCycles = 1;
	public int currentSpawnID = 0;
	public int robotSpawnCyclesLeft = 1;

	public int angelsToSpawn = 1;
	public int angelsLeft = 1;

	public GameObject robotPrefab;
	public GameObject angelPrefab;
	public GameObject enemyCoinPrefab;

	public bool levelReady = false;

	public List<GameObject> enemies;

	public List<GameObject> enemyCoins;
	

	public int currentMultiplier;

	public static T[] Shuffle<T> (T[] array)
	    {
	        int n = array.Length;
	        while (n > 1) {
	            int k = Random.Range(0, (n-- - 1));
	            T temp = array[n];
	            array[n] = array[k];
	            array[k] = temp;
	        }
	
	        return array;
	    }


	public void RemoveEnemies () {
		
		
		for (int i = enemies.Count - 1; i >= 0; i--) {

			GameObject enemy = enemies [i];

			enemies.Remove (enemy);
			SimplePool.Despawn (enemy);
			

		}
		
		
		for (int i = enemyCoins.Count - 1; i >= 0; i--) {

			GameObject enemy = enemyCoins [i];

			enemyCoins.Remove (enemy);
			Destroy (enemy);
			

		}

	}
	    
    public void ResetLevel () {

		RemoveEnemies ();

		robotSpawnCycles = Mathf.Min (robotSpawnCycles, robotSpawnPoints.Count);
		robotTimer = firstRobotSpawn;
		robotsLeft = robotsToSpawn;
		robotSpawnCyclesLeft = robotSpawnCycles;
		angelsLeft = angelsToSpawn;

		currentSpawnID = 0;

		robotSpawnPoints = robotSpawnPoints.OrderBy( x => Random.value ).ToList( );
		
		robotSpawnPoint = robotSpawnPoints [currentSpawnID].transform.position;

		Invoke ("SpawnRobot", firstRobotSpawn);
		Invoke ("SpawnAngels", firstRobotSpawn);
	
	
	
	}

	public void EnemiesToCoins () {

		foreach (GameObject enemy in enemies) {

			enemy.SetActive (false);

			GameObject enemyCoin = Instantiate (enemyCoinPrefab);

			enemyCoin.transform.position = enemy.transform.position;
			enemyCoin.GetComponent<CoinCollision> ().enemy = enemy;
			enemyCoin.GetComponent<CoinCollision> ().level = this;

			enemyCoins.Add (enemyCoin);
			
		}


	}
	
	public void RestoreEnemies () {

		foreach (GameObject enemy in enemies) {

			enemy.SetActive (true);
		
		}

		foreach (GameObject enemyCoin in enemyCoins) {

			Destroy (enemyCoin);

		}

		enemyCoins.Clear ();

		SpawnAngels ();
		SpawnRobot ();
	
	}



	public void EnemyCoinCollected (GameObject enemy, GameObject coin) {

		if (enemies.Contains (enemy)) {
		
			enemies.Remove (enemy);
			SimplePool.Despawn (enemy);
		}

		if (enemyCoins.Contains (coin)) {
			enemyCoins.Remove (coin);
		}

		if (enemy.name.Contains ("Robot")) {
			Debug.Log ("NEw robot");
			robotsLeft++;
		}

		if (enemy.name.Contains ("Angel")) {
			angelsLeft++;
			Debug.Log ("NEw angel");
		}
		
		

	}

	public void FreezeEnemies () {

		foreach (GameObject enemy in enemies) {

			enemy.GetComponent<Rigidbody> ().isKinematic = true;

		}



	}

    // Update is called once per frame
    void Update()
    {
		if (levelReady) {

			//CheckRobotSpawn ();
		}
        
    }

	void SpawnAngels () {

		//Debug.Log ("Spawning angels..");

		for (int i = 0; i < angelsLeft; i++) {

			//Debug.Log ("Spawn angel");

			GameObject go = SimplePool.Spawn (angelPrefab, Vector3.zero, Quaternion.identity);
			go.GetComponent<Angel> ().level = this;
			go.GetComponent<Rigidbody> ().isKinematic = false;

			go.transform.position = angelSpawnPoints [i].transform.position;
			enemies.Add (go);
			
		



		}

		angelsLeft = 0;
		


	}


	void CheckRobotSpawn () {

			if (robotsLeft > 0) {

				robotTimer -= Time.deltaTime;

				if (robotTimer <= 0f) {

					SpawnRobot ();

				}
			}
	
	}

	public void SpawnRobot () {

		if (robotsLeft > 0) {


			robotsLeft--;
			robotTimer = robotSpawnInterval;

			GameObject go = SimplePool.Spawn (robotPrefab, Vector3.zero, Quaternion.identity);
			go.GetComponent<Robot> ().level = this;
			go.transform.position = robotSpawnPoint;
			go.GetComponent<Robot> ().ResetRobot ();
	
			enemies.Add (go);
		} else {
			
			
			robotSpawnCyclesLeft--;
			if (robotSpawnCyclesLeft > 0) {
				currentSpawnID++;

				robotSpawnPoint = robotSpawnPoints [currentSpawnID].transform.position;
				robotsLeft = robotsToSpawn;
				SpawnRobot ();
			}
			


		}

	

	}
}
