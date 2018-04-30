using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Setting {
	CHARACTER_1,
	CHARACTER_2,
	LEVEL,
	ROUND_COUNT,
	ROUND_LENGTH,
	CONFIRM
}

public enum LevelSelected {
	FOREST,
	WORLDS,
	INDUSTRY
}

public class GameMenuNew : MonoBehaviour {

	public Text titleText;
	public GameObject background;
	public GameObject summaryBackground;
	public Text roundCountSummaryText;
	public Text roundTimeSummaryText;
	public GameObject player1SpriteObj;
	public GameObject player2SpriteObj;
	public Image selectedImage;
	public Text roundText;
	public Text selectionText;

	public string characterTitle;
	public string player1Text;
	public string player2Text;
	public string playerCpuText;
	public string levelTitle;
	public string[] levelNames;
	public string roundsTitle;
	public string roundLengthTitle;
	public string ready;
	public int[] numberOfRounds;
	public int[] lengthOfRounds;

	public Sprite[] player1Sprites;
	public Sprite[] player2Sprites;
	public Sprite[] levelSprites;
	public GameObject[] environments;

	public Scene[] scenes;

	private Setting currentSetting = Setting.CHARACTER_1;
	private LevelSelected currentLevel;
	private int roundCount = 0;
	private int roundLength = 0;

	private int selectionIndex = 0;

	void Update(){
		if(Input.GetKeyDown("escape")) {
			OnBackButtonPressed();
		}
	}

	public void OnLeftButtonPressed(){
		AndroidManager.HapticFeedback();
		selectionIndex--;
		switch (currentSetting){
			case Setting.CHARACTER_1:
				if (selectionIndex < 0){
					selectionIndex = player1Sprites.Length-1;
				}
				selectedImage.sprite = player1Sprites[selectionIndex];
				player1SpriteObj.GetComponent<SpriteRenderer>().sprite = player1Sprites[selectionIndex];
				break;
			case Setting.CHARACTER_2:
				if (selectionIndex < 0){
					selectionIndex = player2Sprites.Length-1;
				}
				selectedImage.sprite = player2Sprites[selectionIndex];
				player2SpriteObj.GetComponent<SpriteRenderer>().sprite = player2Sprites[selectionIndex];
				break;
			case Setting.LEVEL:
				if (selectionIndex < 0){
					selectionIndex = levelSprites.Length-1;
				}
				for (int i=0; i<environments.Length; i++){
					if (i != selectionIndex){
						environments[i].SetActive(false);
					}
				}
				environments[selectionIndex].SetActive(true);
				selectedImage.sprite = levelSprites[selectionIndex];
				selectionText.text = levelNames[selectionIndex];
				break;
			case Setting.ROUND_COUNT:
				if (selectionIndex < 0){
					selectionIndex = numberOfRounds.Length-1;
				}
				roundText.text = numberOfRounds[selectionIndex].ToString();
				break;
			case Setting.ROUND_LENGTH:
				if (selectionIndex < 0){
					selectionIndex = lengthOfRounds.Length-1;
				}
				roundText.text = lengthOfRounds[selectionIndex].ToString();
				break;
			case Setting.CONFIRM:
			default:
				break;
		}
	}

	public void OnRightButtonPressed(){
		AndroidManager.HapticFeedback();
		selectionIndex++;
		switch (currentSetting){
			case Setting.CHARACTER_1:
				if (selectionIndex >= player1Sprites.Length){
					selectionIndex = 0;
				}
				selectedImage.sprite = player1Sprites[selectionIndex];
				player1SpriteObj.GetComponent<SpriteRenderer>().sprite = player1Sprites[selectionIndex];
				break;
			case Setting.CHARACTER_2:
				if (selectionIndex >= player2Sprites.Length){
					selectionIndex = 0;
				}
				selectedImage.sprite = player2Sprites[selectionIndex];
				player2SpriteObj.GetComponent<SpriteRenderer>().sprite = player2Sprites[selectionIndex];
				break;
			case Setting.LEVEL:
				if (selectionIndex >= levelSprites.Length){
					selectionIndex = 0;
				}
				for (int i=0; i<environments.Length; i++){
					if (i != selectionIndex){
						environments[i].SetActive(false);
					}
				}
				environments[selectionIndex].SetActive(true);
				selectedImage.sprite = levelSprites[selectionIndex];
				selectionText.text = levelNames[selectionIndex];
				break;
			case Setting.ROUND_COUNT:
				if (selectionIndex >= numberOfRounds.Length){
					selectionIndex = 0;
				}
				roundText.text = numberOfRounds[selectionIndex].ToString();
				break;
			case Setting.ROUND_LENGTH:
				if (selectionIndex >= lengthOfRounds.Length){
					selectionIndex = 0;
				}
				roundText.text = lengthOfRounds[selectionIndex].ToString();
				break;
			case Setting.CONFIRM:
			default:
				break;
		}
	}

	public void OnConfirmButtonPressed(){
		AndroidManager.HapticFeedback();
		switch (currentSetting){
			case Setting.CHARACTER_1:
				switch (selectionIndex){
					case 0:
						GameSettings.instance.player1Char = PlayerCharacter.KRAB;
						break;
					case 1:
						GameSettings.instance.player1Char = PlayerCharacter.BEETLE;
						break;
					case 2:
						GameSettings.instance.player1Char = PlayerCharacter.ROACH;
						break;
				}
				currentSetting = Setting.CHARACTER_2;
				switch (GameSettings.instance.playerMode){
					case PlayerMode.COMPUTER:
						selectionText.text = playerCpuText;
					break;
					case PlayerMode.HUMAN:
						selectionText.text = selectionText.text = player2Text;
					break;
				}
				selectionIndex = 0;
				selectedImage.sprite = player2Sprites[selectionIndex];
				player2SpriteObj.GetComponent<SpriteRenderer>().sprite = player2Sprites[selectionIndex];
				break;
			case Setting.CHARACTER_2:
				switch (selectionIndex){
					case 0:
						GameSettings.instance.player2Char = PlayerCharacter.KRAB;
						break;
					case 1:
						GameSettings.instance.player2Char = PlayerCharacter.BEETLE;
						break;
					case 2:
						GameSettings.instance.player2Char = PlayerCharacter.ROACH;
						break;
				}
				currentSetting = Setting.LEVEL;
				titleText.text = levelTitle;
				selectionIndex = 0;
				selectionText.text = levelNames[selectionIndex];
				selectedImage.sprite = levelSprites[selectionIndex];
				for (int i=0; i<environments.Length; i++){
					if (i != selectionIndex){
						environments[i].SetActive(false);
					}
				}
				environments[selectionIndex].SetActive(true);
				break;
			case Setting.LEVEL:
				switch (selectionIndex){
					case 0:
						currentLevel = LevelSelected.FOREST;
						break;
					case 1:
						currentLevel = LevelSelected.WORLDS;
						GameSettings.instance.gameMode = (GameSettings.instance.playerMode == PlayerMode.HUMAN) ? GameMode.BATTLE : GameMode.DEFAULT;
						break;
					case 2:
						currentLevel = LevelSelected.INDUSTRY;
						break;
				}
				currentSetting = Setting.ROUND_COUNT;
				titleText.text = roundsTitle;
				selectionIndex = 0;
				selectionText.gameObject.SetActive(false);
				selectedImage.gameObject.SetActive(false);
				roundText.gameObject.SetActive(true);
				roundText.text = numberOfRounds[selectionIndex].ToString();
				break;
			case Setting.ROUND_COUNT:
				GameSettings.instance.roundCount = (selectionIndex * 2) + 1;
				currentSetting = Setting.ROUND_LENGTH;
				titleText.text = roundLengthTitle;
				selectionIndex = 0;
				roundText.text = lengthOfRounds[selectionIndex].ToString();
				break;
			case Setting.ROUND_LENGTH:
				GameSettings.instance.roundTime = (selectionIndex + 1) * 30;
				currentSetting = Setting.CONFIRM;
				titleText.text = ready;
				background.SetActive(false);
				summaryBackground.SetActive(true);
				roundCountSummaryText.text = "Best of " + GameSettings.instance.roundCount;
				roundTimeSummaryText.text = GameSettings.instance.roundTime + " second rounds";
				break;
			case Setting.CONFIRM:
				switch (currentLevel){
					case LevelSelected.FOREST:
						StartCoroutine(LoadAsyncScene("DigitalForest"));
						break;
					case LevelSelected.WORLDS:
						StartCoroutine(LoadAsyncScene("FacingWorlds"));
						break;
					case LevelSelected.INDUSTRY:
						StartCoroutine(LoadAsyncScene("IndustrialAction"));
						break;
				}
				break;
		}
	}

	public void OnBackButtonPressed(){
		AndroidManager.HapticFeedback();
		switch (currentSetting){
			case Setting.CHARACTER_1:
				StartCoroutine(LoadAsyncScene("MainMenu"));
			break;
			case Setting.CHARACTER_2:
				currentSetting = Setting.CHARACTER_1;
				selectionIndex = 0;
				selectedImage.sprite = player1Sprites[selectionIndex];
				selectionText.text = selectionText.text = player1Text;
				player1SpriteObj.GetComponent<SpriteRenderer>().sprite = player1Sprites[selectionIndex];
				break;
			case Setting.LEVEL:
				currentSetting = Setting.CHARACTER_2;
				titleText.text = characterTitle;
				switch (GameSettings.instance.playerMode){
					case PlayerMode.COMPUTER:
						selectionText.text = playerCpuText;
					break;
					case PlayerMode.HUMAN:
						selectionText.text = selectionText.text = player2Text;
					break;
				}
				for (int i=0; i<environments.Length; i++){
						environments[i].SetActive(false);
				}
				selectionIndex = 0;
				selectedImage.sprite = player2Sprites[selectionIndex];
				player2SpriteObj.GetComponent<SpriteRenderer>().sprite = player2Sprites[selectionIndex];
				break;
			case Setting.ROUND_COUNT:
				currentSetting = Setting.LEVEL;
				titleText.text = levelTitle;
				selectionText.gameObject.SetActive(true);
				selectedImage.gameObject.SetActive(true);
				roundText.gameObject.SetActive(false);
				selectionIndex = 0;
				selectionText.text = levelNames[selectionIndex];
				selectedImage.sprite = levelSprites[selectionIndex];
				for (int i=0; i<environments.Length; i++){
					if (i != selectionIndex){
						environments[i].SetActive(false);
					}
				}
				environments[selectionIndex].SetActive(true);
				break;
			case Setting.ROUND_LENGTH:
				currentSetting = Setting.ROUND_COUNT;
				titleText.text = roundsTitle;
				selectionIndex = 0;
				roundText.text = numberOfRounds[selectionIndex].ToString();
				break;
			case Setting.CONFIRM:
				summaryBackground.SetActive(false);
				background.SetActive(true);
				currentSetting = Setting.ROUND_LENGTH;
				roundText.gameObject.SetActive(true);
				selectionText.gameObject.SetActive(false);
				selectedImage.gameObject.SetActive(false);
				titleText.text = roundLengthTitle;
				selectionIndex = 0;
				roundText.text = lengthOfRounds[selectionIndex].ToString();
				break;
		}
	}

	IEnumerator LoadAsyncScene(string sceneName){
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		while (!asyncLoad.isDone){
			yield return null;
		}
    }

}
