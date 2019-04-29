using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviourSingleton<GameController>
{



	public int currentScore = 0;

	public int highScore = 0;

	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI bestText;
	public TextMeshProUGUI multText;
	public TextMeshProUGUI roundText;

	public RectTransform leftMult;
	public RectTransform rightMult;

	public int currentLevelID = 0;
	public int currentCycleID = 0;
	public List<GameObject> levels;

	public GameObject title;

	public Level currentLevel;

	public GameObject reaper;

	public int currentMultiplier = 1;
	public int multiplierCount = 0;

	public int enemyCoinChain = 0;
	public int [] coinValues = { 100, 200, 300, 500, 800, 1200, 2000};
	public int [] soulBonuses = { 0, 10000, 20000, 30000, 50000, 50000, 50000 }
;

	public GameObject powerCoinPrefab;
	
	public GameObject scoreTextPrefab;

	public List<InAudioNode> levelMusic;
	public InAudioNode coinMusic;
	public InAudioNode powerMusic;
	public InAudioNode collectCoin;
	public InAudioNode collectPower;
	public InAudioNode dead;
	public InAudioNode complete;

	public GameObject powerCoin;

	public bool powerCoinMode = false;

	public InPlayer levelMusicHandle;
	

	public RectTransform startLeft;
	public RectTransform startRight;

	public RectTransform endLeft;
	public RectTransform endRight;

	public int livesLeft = 3;

	public List<GameObject> liveIcons;
	
    // Start is called before the first frame update
    void Start()
    {
		reaper.SetActive (false);
		highScore = PlayerPrefs.GetInt ("highScore", 0);
		bestText.text = highScore.ToString ();
		ScorePoints (0);
		title.SetActive (true);

		//Invoke ("StartRound",1f);

    }

	public void UpdateLives () {

		for (int i = 0; i < 3; i++) {

			if (i < livesLeft) {
				liveIcons [i].SetActive (true);
			} else {
				liveIcons [i].SetActive (false);
			}


		}


	}


	public void PressStart () {

		Debug.Log ("What?"); 
		currentLevelID = 0;
		currentCycleID = 0;
		currentMultiplier = 1;
		multiplierCount = 0;
		enemyCoinChain = 0;
		scoreText.text = "0";
		currentScore = 0;

		title.transform.DOScale (0, 0.2f);
		livesLeft = 3;
		
		UpdateLives ();

		StartRound ();


	}

	void StartRound () {

		//currentLevel = 	

		GameObject level = Instantiate (levels [currentLevelID]);
		level.transform.SetParent (GameObject.FindWithTag ("Finish").transform);
		level.transform.localPosition = Vector3.zero;
		


		currentLevel = level.GetComponent<Level> ();
		;
		//currentMultiplier = 1;
		roundText.text = "ROUND " + (currentLevelID + 1 + (currentCycleID * levels.Count)).ToString();
		enemyCoinChain = 0;
		powerCoinMode = false;
		
		
		ResetRound ();

		
	
	}

	public void ReaperDied () {

		if (reaper.GetComponent<ControlReaper> ().isDead == false) {

			livesLeft = livesLeft - 1;
			UpdateLives ();

			currentLevel.FreezeEnemies ();

			InAudio.StopAll ();

			InAudio.Play (Camera.main.gameObject, dead);

			if (livesLeft > 0) {

				Invoke ("ResetRound", 2f);
			} else {
				GameOver ();

			}

			if (powerCoin != null) {
				Destroy (powerCoin);
				powerCoin = null;
			}
		}


	}

	void GameOver () {

		endLeft.DOScaleX (1f, 0);
		endLeft.DOAnchorPosX (-750, 0f);
		endLeft.gameObject.SetActive (true);
		endLeft.DOAnchorPosX (0, currentLevel.firstRobotSpawn * 1.25f).OnComplete (() => HideObject(endLeft.gameObject));;
		
		endRight.DOScaleX (1f, 0);
		endRight.DOAnchorPosX (750, 0f);
		endRight.gameObject.SetActive (true);
		endRight.DOAnchorPosX (0, currentLevel.firstRobotSpawn * 1.25f).OnComplete (() => HideObject(endRight.gameObject));

		PlayerPrefs.SetInt ("highScore", highScore);
		
		
		Invoke ("BackToTitle", 3.5f);

	}

	void BackToTitle () {
		
		currentLevel.RemoveEnemies ();
		Destroy (currentLevel.gameObject);
		title.transform.DOScale (1, 0.2f);
		

	}

	public void CollectedPowerCoin () {

		if (reaper.GetComponent<ControlReaper> ().isDead == false) {

			currentLevel.EnemiesToCoins ();

			if (currentMultiplier < 5) {
				SetMultiplier (currentMultiplier + 1);
			}

			multiplierCount -= 20;

			Invoke ("EndPowerCoin", 6f);
			powerCoinMode = true;

			InAudio.Stop (Camera.main.gameObject, powerMusic);
			InAudio.Play (Camera.main.gameObject, coinMusic);


			InAudio.Play (Camera.main.gameObject, collectPower);

		}
	


	}

	public void CollectEnemyCoin () {

		if (reaper.GetComponent<ControlReaper> ().isDead == false) {

			ScorePoints (coinValues [Mathf.Min (6, enemyCoinChain)]);
			enemyCoinChain++;
			InAudio.Play (Camera.main.gameObject, collectCoin);

		}
		

	}
	

	public void AddToMultiplierCount (int value) {

		if (powerCoinMode == false) {

			int prevMult = multiplierCount;

			if (multiplierCount < 20) {
				multiplierCount += value;
				if (multiplierCount > 20) {
					multiplierCount = 20;
				}

				leftMult.sizeDelta = new Vector2 (20f + 60f / 20f * (float)multiplierCount, 80f);
				rightMult.sizeDelta = new Vector2 (20f + 60f / 20f * (float)multiplierCount, 80f);
			}

			if (prevMult < 20 && multiplierCount >= 20) {
				TriggerPowerCoin ();
			}

		}
		
		
		
		

	}

	public void EndPowerCoin () {
	
		InAudio.Stop (Camera.main.gameObject, coinMusic);
		powerCoinMode = false;

		if (reaper.GetComponent<ControlReaper> ().isDead == false) {

			currentLevel.RestoreEnemies ();
			InAudio.Play (Camera.main.gameObject, levelMusic [0]);
		}

	}


	public void TriggerPowerCoin () {

		if (reaper.GetComponent<ControlReaper> ().isDead == false) {

			GameObject go = Instantiate (powerCoinPrefab);
			go.transform.position = currentLevel.robotSpawnPoint;

			InAudio.Stop (Camera.main.gameObject, levelMusic [0]);

			InAudio.Play (Camera.main.gameObject, powerMusic);

			powerCoin = go;

		}

		
	}

	public void RoundComplete (int soulBonus) {
		
		InAudio.StopAll ();

		InAudio.Play (Camera.main.gameObject, complete);
			
				if (powerCoin != null) {
					Destroy (powerCoin);
					powerCoin = null;
				}

		ScorePoints (soulBonuses [soulBonus], false);

		currentLevel.FreezeEnemies ();
		

		reaper.GetComponent<ControlReaper> ().isDead = true;
		reaper.GetComponent<ControlReaper> ().animator.SetBool ("Celebrating", true);

		Invoke ("NextLevel", 4f);
		
		
		
		

	}


	void NextLevel () {
				
		reaper.GetComponent<ControlReaper> ().animator.SetBool ("Celebrating", false);

		currentLevel.RemoveEnemies ();

		if (currentLevelID < levels.Count - 1) {
			currentLevelID++;

		} else {
			currentLevelID = 0;
			currentCycleID++;
		}

		Destroy (currentLevel.gameObject);
		
		StartRound ();
		

	}

	void SetMultiplier (int value) {

		currentMultiplier = value;
		multText.text = "x" + value;

	}

	public void ResetRound () {
		
		currentLevel.ResetLevel ();

		reaper.SetActive (false);
		//reaper.GetComponent<ControlReaper> ().rigidbody.isKinematic = false;
		reaper.GetComponent<ControlReaper> ().animator.SetBool ("IsDead", false);
		reaper.GetComponent<ControlReaper> ().Reset ();
		
		
		

		//reaper.transform.position = currentLevel.reaperSpawnPoint.transform.position;

		reaper.transform.DOScale (Vector3.zero, 0f);

		reaper.transform.DOScale (Vector3.one, 0.2f).SetDelay (currentLevel.firstRobotSpawn).OnComplete(ActivateReaper);
		
		currentLevel.levelReady = true;

		InAudio.StopAll ();

		levelMusicHandle = InAudio.Play (Camera.main.gameObject,levelMusic [0]);

		startLeft.DOScaleX (1f, 0);
		startLeft.DOAnchorPosX (-750, 0f);
		startLeft.gameObject.SetActive (true);
		startLeft.DOAnchorPosX (0, currentLevel.firstRobotSpawn * 0.75f).OnComplete (() => HideObject(startLeft.gameObject));;
		
		startRight.DOScaleX (1f, 0);
		startRight.DOAnchorPosX (750, 0f);
		startRight.gameObject.SetActive (true);
		startRight.DOAnchorPosX (0, currentLevel.firstRobotSpawn * 0.75f).OnComplete (() => HideObject(startRight.gameObject));

		
			
		
	}

	void HideObject (GameObject go) {

		go.transform.DOScaleX (0f, 0.4f).SetDelay(0.6f);
		//go.SetActive (false);

	}
	
	void ActivateReaper () {


		reaper.SetActive (true);
	}

	// Update is called once per frame
	void Update()
    {
        
    }


	public void ScorePoints (int points, bool useMultipler = true) {

		int score = points;

		if (useMultipler) {
			score = score * currentMultiplier;
		}

		currentScore = currentScore + score;
		scoreText.text = currentScore.ToString ();
		
		if (currentScore >= highScore) {

			highScore = currentScore;
			bestText.text = highScore.ToString ();
		}
		
			
		GameObject scoreText2 = SimplePool.Spawn (scoreTextPrefab, Vector3.zero, Quaternion.identity);
			scoreText2.transform.GetComponentInChildren<TextMeshProUGUI> ().text = score.ToString();
			scoreText2.GetComponent<RectTransform> ().position = reaper.transform.position;
			scoreText2.transform.DOScale (Vector3.zero, 0f);
			scoreText2.transform.DOScale (Vector3.one, 0.1f).SetDelay (0.05f);
			scoreText2.transform.DOScale (Vector3.zero, 0.1f).SetDelay (0.7f).OnComplete (() => DespawnText (scoreText2));;
		
		
	}
	
	
	void DespawnText ( GameObject go) {

		SimplePool.Despawn (go);


	}
}
