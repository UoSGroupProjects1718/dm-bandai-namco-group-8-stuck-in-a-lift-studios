using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

	public Text roundCountText;
	public Slider roundSlider;
	public Button roundNegative;
	public Button roundPositive;

	public Text timeLimitCountText;
	public Slider timeLimitSlider;
	public Button timeLimitNegative;
	public Button timeLimitPositive;

	public Text gameModeSelectionText;
	public Button gameModeNegative;
	public Button gameModePositive;

	public Button player1Button;
	public Text player1ButtonText;
	public Sprite player1HumanSprite;
	public Sprite player1ComputerSprite;

	public Button player2Button;
	public Text player2ButtonText;
	public Sprite player2HumanSprite;
	public Sprite player2ComputerSprite;

	public Button instructionsButton;
	public Button startGameButton;

	private int roundCount;
	private int timeCount;
	private GameMode gameModeSelected;
	private string gameModeString;
	private PlayerMode player1Mode;
	private string player1String;
	private PlayerMode player2Mode;
	private string player2String;

	void Start () {
		roundCount = 3;
		roundNegative.onClick.AddListener(decrementRoundCount);
		roundPositive.onClick.AddListener(incrementRoundCount);

		timeCount = 2;
		timeLimitNegative.onClick.AddListener(decrementTimeCount);
		timeLimitPositive.onClick.AddListener(incrementTimeCount);

		gameModeSelected = GameMode.DEFAULT;
		gameModeNegative.onClick.AddListener(cycleGameModeBack);
		gameModePositive.onClick.AddListener(cycleGameModeForward);

		player1Mode = PlayerMode.HUMAN;
		player2Mode = PlayerMode.HUMAN;
		player1Button.onClick.AddListener(togglePlayer1);
		player2Button.onClick.AddListener(togglePlayer2);

		instructionsButton.onClick.AddListener(displayInstructions);
		startGameButton.onClick.AddListener(StartGame);
	}

	void Update () {
		roundSlider.value = roundCount;
		roundCountText.text = roundCount.ToString();

		timeLimitSlider.value = timeCount;
		timeLimitCountText.text = (timeCount*30).ToString();

		switch (gameModeSelected){
			case GameMode.DEFAULT:
				gameModeString = "Standard";
				break;
			case GameMode.SINGLE_PICKUP:
				gameModeString = "You Only Get One";
				break;
			case GameMode.BATTLE:
				gameModeString = "Battle!";
				break;
		}
		gameModeSelectionText.text = gameModeString;

		switch (player1Mode){
			case PlayerMode.HUMAN:
				player1String = "Human";
				player1Button.GetComponent<Image>().sprite = player1HumanSprite;
				break;
			case PlayerMode.COMPUTER:
				player1String = "Computer";
				player1Button.GetComponent<Image>().sprite = player1ComputerSprite;
				break;
		}
		switch (player2Mode){
			case PlayerMode.HUMAN:
				player2String = "Human";
				player2Button.GetComponent<Image>().sprite = player2HumanSprite;
				break;
			case PlayerMode.COMPUTER:
				player2String = "Computer";
				player2Button.GetComponent<Image>().sprite = player2ComputerSprite;
				break;
		}
		player1ButtonText.text = player1String;
		player2ButtonText.text = player2String;
	}

	private void decrementRoundCount(){
		if (roundCount > 1){
			roundCount --;
		}
	}
	private void incrementRoundCount(){
		if (roundCount < 10){
			roundCount ++;
		}
	}

	private void decrementTimeCount(){
		if (timeCount > 1){
			timeCount -- ;
		}
	}
	private void incrementTimeCount(){
		if (timeCount < 6){
			timeCount ++;
		}
	}

	private void cycleGameModeBack(){
		switch (gameModeSelected){
			case GameMode.DEFAULT:
				gameModeSelected = GameMode.BATTLE;
				break;
			case GameMode.SINGLE_PICKUP:
				gameModeSelected = GameMode.DEFAULT;
				break;
			case GameMode.BATTLE:
				gameModeSelected = GameMode.SINGLE_PICKUP;
				break;
		}
	}

	private void cycleGameModeForward(){
		switch (gameModeSelected){
			case GameMode.DEFAULT:
				gameModeSelected = GameMode.SINGLE_PICKUP;
				break;
			case GameMode.SINGLE_PICKUP:
				gameModeSelected = GameMode.BATTLE;
				break;
			case GameMode.BATTLE:
				gameModeSelected = GameMode.DEFAULT;
				break;
		}
	}

	private void togglePlayer1(){
		switch(player1Mode){
			case PlayerMode.HUMAN:
				player1Mode = PlayerMode.COMPUTER;
				break;
			case PlayerMode.COMPUTER:
				player1Mode = PlayerMode.HUMAN;
				break;
		}
	}

	private void togglePlayer2(){
		switch(player2Mode){
			case PlayerMode.HUMAN:
				player2Mode = PlayerMode.COMPUTER;
				break;
			case PlayerMode.COMPUTER:
				player2Mode = PlayerMode.HUMAN;
				break;
		}
	}

	private void displayInstructions(){
		switch (gameModeSelected){
			case GameMode.DEFAULT:
				break;
			case GameMode.SINGLE_PICKUP:
				break;
			case GameMode.BATTLE:
				break;
		}
	}

	private void StartGame(){
		//TODO change scene
		GameManager.instance.StartGame(roundCount, timeCount*30f, gameModeSelected, player1Mode, player2Mode);
	}

}
