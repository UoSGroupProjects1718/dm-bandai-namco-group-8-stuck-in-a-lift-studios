using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public Color playerOneColour;
	public Color playerTwoColour;
	public GameObject pickupFlockStandard;
	public GameObject pickupFlockSingle;
	public Text time;
	public Text ready;
	public GameObject lasers;
	public ParticleSystem roundTransition;

	public GameObject player_one_A;
	public GameObject player_one_B;
	public GameObject player_one_C;
	public GameObject player_two_A;
	public GameObject player_two_B;
	public GameObject player_two_C;

	public AudioSource intro;
	public AudioSource timeRemaining;
	public AudioSource outro;
	public AudioSource player1Win;
	public AudioSource player2Win;

	public GameObject multiplayerControls;
	public GameObject singleplayerControls;
	public GameObject gameOverMenu;

	//PRIVATE VARIABLES
	private Player player1Script;
	private Player player2Script;

	private bool gameRunning;
	private bool introRunning;
	private bool playerOneReady;
	private bool playerTwoReady;
	private int playerOneSize;
	private int playerTwoSize;
	private int playerOneScore;
	private int playerTwoScore;

	private int minutes;
	private int seconds;
	private string niceTime;

	private bool readyToRestart;
	private int currentRound;
	private int roundCount;
	private float timer;
	private float maxTime;
	private GameMode gameMode;
	private GameObject instantiatedPickupFlock;
	private PlayerMode player1Mode;
	private PlayerMode player2Mode;

	public void StartGame(int roundCount, float roundTime, GameMode gameMode, PlayerMode player1Mode, PlayerMode player2Mode){
		this.roundCount = roundCount;

		this.maxTime = roundTime;
		timer = maxTime;
		minutes = Mathf.FloorToInt(timer/60f);
		seconds = Mathf.FloorToInt(timer - minutes * 60);
		niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
		time.text = niceTime;

		this.gameMode = gameMode;
		this.player1Mode = player1Mode;
		this.player2Mode = player2Mode;

		player1Script.SetPlayerMode(player1Mode);
		player2Script.SetPlayerMode(player2Mode);
	}

	void Awake(){
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy(gameObject);
		}
		readyToRestart = false;

		if (GameSettings.instance.playerMode == PlayerMode.COMPUTER){
			multiplayerControls.SetActive(false);
			singleplayerControls.SetActive(true);
		} else {
			multiplayerControls.SetActive(true);
			singleplayerControls.SetActive(false);
		}

		switch (GameSettings.instance.player1Char){
			case PlayerCharacter.KRAB:
				Debug.Log("I'm player 1 and I'm a KRAB!");
				player_one_A.SetActive(true);
				player_one_B.SetActive(false);
				player_one_C.SetActive(false);
				player1Script = player_one_A.GetComponent<Player>();
				break;
			case PlayerCharacter.BEETLE:
				Debug.Log("I'm player 1 and I'm a Beetle!");
				player_one_A.SetActive(false);
				player_one_B.SetActive(true);
				player_one_C.SetActive(false);
				player1Script = player_one_B.GetComponent<Player>();
				break;
			case PlayerCharacter.ROACH:
				Debug.Log("I'm player 1 and I'm a Roach!");
				player_one_A.SetActive(false);
				player_one_B.SetActive(false);
				player_one_C.SetActive(true);
				player1Script = player_one_C.GetComponent<Player>();
				break;
				break;
		}
		switch (GameSettings.instance.player2Char){
			case PlayerCharacter.KRAB:
				Debug.Log("I'm player 2 and I'm a KRAB!");
				player_two_A.SetActive(true);
				player_two_B.SetActive(false);
				player_two_C.SetActive(false);
				player2Script = player_two_A.GetComponent<Player>();
				break;
			case PlayerCharacter.BEETLE:
				Debug.Log("I'm player 2 and I'm a Beetle!");
				player_two_A.SetActive(false);
				player_two_B.SetActive(true);
				player_two_C.SetActive(false);
				player2Script = player_two_B.GetComponent<Player>();
				break;
			case PlayerCharacter.ROACH:
				Debug.Log("I'm player 2 and I'm a Roach!");
				player_two_A.SetActive(false);
				player_two_B.SetActive(false);
				player_two_C.SetActive(true);
				player2Script = player_two_C.GetComponent<Player>();
				break;
		}

		StartGame(GameSettings.instance.roundCount, GameSettings.instance.roundTime, GameSettings.instance.gameMode, PlayerMode.HUMAN, GameSettings.instance.playerMode);

	}

	void Start() {
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy(gameObject);
		}

		Restart();
	}

	public void Restart(){
		readyToRestart = false;
		ready.color = Color.white;
		gameRunning = false;
		introRunning = false;
		playerOneReady = false;
		playerTwoReady = false;

		playerOneSize = 0;
		playerTwoSize = 0;

		timer = maxTime;
		minutes = Mathf.FloorToInt(timer/60f);
		seconds = Mathf.FloorToInt(timer - minutes * 60);
		niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
		time.text = niceTime;

		player1Script.Restart();
		player2Script.Restart();
	}

	public void setPlayerReady(int player, bool ready){
		if (gameRunning){
			return;
		}
		switch (player){
			case 1:
				playerOneReady = ready;
				break;
			case 2:
				playerTwoReady = ready;
				break;
			}
	}

	public PlayerMode getPlayerMode(int playerID){
		switch (playerID){
			case 1:
				return player1Mode;
			case 2:
				return player2Mode;
		}
		Debug.Log("ERROR! INVALID PLAYER ID");
		return PlayerMode.ERROR;
	}

	public void setPlayerScore(int playerID, int score){
		switch (playerID){
			case 1:
				playerOneSize = score;
				break;
			case 2:
				playerTwoSize = score;
				break;
		}
	}

	public int getOtherPlayerScore(int player){
		switch (player){
			case 1:
				return playerTwoSize;
			case 2:
				return playerOneSize;
		}
		//Shouldn't be here
		Debug.Log("ERROR! INVALID PLAYER ID");
		return -1;
	}

	public int getOtherPlayerID(int player){
		switch (player){
			case 1:
				return 2;
			case 2:
				return 1;
		}
		//Shouldn't be here
		Debug.Log("ERROR! INVALID PLAYER ID");
		return -1;
	}

	void Update() {
		if(Input.GetKeyDown("escape")) {
//			Application.Quit(); 
			//TODO Create and show a pause screen.
		}

		switch (gameMode){
			case GameMode.DEFAULT:
				lasers.SetActive(false);
				defaultGameMode();
				break;
			case GameMode.SINGLE_PICKUP:
				lasers.SetActive(true);
				singlePickupGameMode();
				break;
			case GameMode.BATTLE:
				lasers.SetActive(true);
				battleGameMode();
				break;
		}
		if (readyToRestart){
			if (currentRound == roundCount || playerOneScore > roundCount/2f || playerTwoScore > roundCount/2f){
				//TODO Bring up gameover menu
				gameOverMenu.SetActive(true);
			} else {
				roundTransition.Play();
				Restart();
			}
		}
	}

	public GameMode getGameMode(){
		return gameMode;
	}

	public bool hasGameStarted(){
		return gameRunning;
	}

	public void setGamePaused(bool isPaused){
		gameRunning = isPaused;
	}

	public void OnRematchButtonPressed(){
		gameOverMenu.SetActive(false);
		playerOneScore = 0;
		playerTwoScore = 0;
		currentRound = 0;
		Restart();
		StartGame(GameSettings.instance.roundCount, GameSettings.instance.roundTime, GameMode.DEFAULT, PlayerMode.HUMAN, GameSettings.instance.playerMode);
	}

	public void OnChangeSettingsButtonPressed(){
		StartCoroutine(LoadAsyncScene("GameMenu"));
	}

	public void OnQuitToMenuButtonPressed(){
		Debug.Log("QuitToMenu Pressed");
//		gameOverMenu.SetActive(false);
		StartCoroutine(LoadAsyncScene("MainMenu"));
	}

	private IEnumerator CountdownDefault(int timeCount){
		int introTime = 5;
		introRunning = true;
		intro.Play();
		cleanup();
		while (introTime > 0){
			introTime --;
			switch (introTime){
				case 5:
				case 4:
					ready.text = "Starting";
					break;
				case 3:
				case 2:
				case 1:
					ready.text = introTime.ToString();
					break;
			}
			yield return new WaitForSeconds(1);
		}
		ready.text = "";
		introRunning = false;
		gameRunning = true;

		while(timeCount > 0) {
			timeCount --;
			if (timeCount == 10){
				Debug.Log("10 Seconds Remaining!");
				timeRemaining.Play();
			}
			if (timeCount == 5){
				Debug.Log("5 Seconds Remaining!");
				outro.Play();
			}
			yield return new WaitForSeconds(1);
		}

		gameRunning = false;
		introRunning = true;
		if (playerOneSize > playerTwoSize){
			player1Win.Play();
			playerOneScore ++;
			ready.text = "Player One Wins!";
			ready.color = playerOneColour;
		} else if (playerTwoSize > playerOneSize){
			player2Win.Play();
			playerTwoScore ++;
			ready.text = "Player Two Wins!";
			ready.color = playerTwoColour;
		} else {
			playerOneScore ++;
			playerTwoScore ++;
			ready.text = "DRAW!";
		}
		int restartTime = 5;
		while( restartTime > 0) {
			restartTime --;
			yield return new WaitForSeconds(1);
		}
		int scoreTime = 5;
		while( scoreTime > 0) {
			if (playerOneScore > playerTwoScore){
				ready.color = playerOneColour;
			} else if (playerTwoScore > playerOneScore){
				ready.color = playerTwoColour;
			} else if (playerOneScore == playerTwoScore){
				ready.color = Color.white;
			}
			ready.text = playerOneScore+" - "+playerTwoScore;
			scoreTime --;
			yield return new WaitForSeconds(1);
		}
		currentRound++;
		readyToRestart = true;
		ready.text = "Ready?";
	}

	private void cleanup(){
		GameObject[] pickupSpawners = GameObject.FindGameObjectsWithTag("spawn");
		for (int i=0; i<pickupSpawners.Length; i++){
			Destroy(pickupSpawners[i]);
		}
		GameObject[] leftoverProjectilesOne = GameObject.FindGameObjectsWithTag("1");
		for (int i=0; i<leftoverProjectilesOne.Length; i++){
			Destroy(leftoverProjectilesOne[i]);
		}
		GameObject[] leftoverProjectilesTwo = GameObject.FindGameObjectsWithTag("2");
		for (int i=0; i<leftoverProjectilesTwo.Length; i++){
			Destroy(leftoverProjectilesTwo[i]);
		}
		switch (gameMode){
			case GameMode.DEFAULT:
				instantiatedPickupFlock = Instantiate(pickupFlockStandard);
				break;
			case GameMode.SINGLE_PICKUP:
				instantiatedPickupFlock = Instantiate(pickupFlockSingle);
				break;
			case GameMode.BATTLE:
				instantiatedPickupFlock = Instantiate(pickupFlockStandard);
				return;
		}
	}

	private void defaultGameMode(){
		if (playerOneReady && playerTwoReady && !introRunning && !gameRunning){
			ready.text = "";
			playerOneReady = false;
			playerTwoReady = false;
			StartCoroutine ("CountdownDefault", timer);
		}
		if (!gameRunning) {
			return;
		}
		if (timer > 0){
			timer -= Time.deltaTime;
		} else {
			timer = 0;
		}
		minutes = Mathf.FloorToInt(timer/60f);
		seconds = Mathf.FloorToInt(timer - minutes * 60);
		niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
		time.text = niceTime;
	}

	private void singlePickupGameMode(){
		defaultGameMode();
	}

	private void battleGameMode(){
		defaultGameMode();
	}

	IEnumerator LoadAsyncScene(string sceneName){
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		while (!asyncLoad.isDone){
			yield return null;
		}
    }
}
