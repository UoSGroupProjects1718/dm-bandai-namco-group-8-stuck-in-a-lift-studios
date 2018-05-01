using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public GameObject startObj;
	public GameObject instructionsButtonObj;
	public GameObject singleplayerObj;
	public GameObject multiplayerObj;
	public GameObject instructionsObj;
	public GameObject quickstartObj;
	public GameObject customiseObj;

	public int largeFontSize;
	public int smallFontSize;

	public void OnStartButton(){
		AndroidManager.HapticFeedback();
		instructionsObj.SetActive(false);
		instructionsButtonObj.GetComponentInChildren<Text>().fontSize = smallFontSize;
		startObj.GetComponentInChildren<Text>().fontSize = largeFontSize;
		singleplayerObj.SetActive(true);
		multiplayerObj.SetActive(true);
		quickstartObj.SetActive(false);
		customiseObj.SetActive(false);
	}

	public void OnInstructionsButton(){
		AndroidManager.HapticFeedback();
		singleplayerObj.SetActive(false);
		multiplayerObj.SetActive(false);
		quickstartObj.SetActive(false);
		customiseObj.SetActive(false);
		startObj.GetComponentInChildren<Text>().fontSize = smallFontSize;
		instructionsButtonObj.GetComponentInChildren<Text>().fontSize = largeFontSize;
		instructionsObj.SetActive(true);
	}

	public void OnSingleplayerButton(){
		AndroidManager.HapticFeedback();
		GameSettings.instance.playerMode = PlayerMode.COMPUTER;
		multiplayerObj.SetActive(false);
		quickstartObj.SetActive(true);
		customiseObj.SetActive(true);
	}

	public void OnMultiplayerButton(){
		AndroidManager.HapticFeedback();
		singleplayerObj.SetActive(false);
		quickstartObj.SetActive(true);
		customiseObj.SetActive(true);
		GameSettings.instance.playerMode = PlayerMode.HUMAN;

	}

	public void OnQuickstartButton(){
//		int[] roundCounts = { 1, 3 , 5 };
//		int[] roundTimes = { 30, 60, 90 };

		GameSettings.instance.roundCount = 3;//roundCounts[Random.Range(0, 3)];
		GameSettings.instance.roundTime = 60;//roundTimes[Random.Range(0,3)];

		int randomStage = Random.Range(0,3);
		Debug.Log("Random Stage = " + randomStage);
		switch (randomStage){
			case 0:
				GameSettings.instance.player1Char = PlayerCharacter.KRAB;
				GameSettings.instance.player2Char = PlayerCharacter.KRAB;
				StartCoroutine(LoadAsyncScene("DigitalForest"));
				break;
			case 1: 
				GameSettings.instance.player1Char = PlayerCharacter.BEETLE;
				GameSettings.instance.player2Char = PlayerCharacter.BEETLE;
				StartCoroutine(LoadAsyncScene("FacingWorlds"));
				break;
			case 2:
				GameSettings.instance.player1Char = PlayerCharacter.ROACH;
				GameSettings.instance.player2Char = PlayerCharacter.ROACH;
				StartCoroutine(LoadAsyncScene("IndustrialAction"));
				break;
		}
	}

	public void OnCustomiseButton(){
		StartCoroutine(LoadAsyncScene("GameMenu"));
	}

	IEnumerator LoadAsyncScene(string sceneName){
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		while (!asyncLoad.isDone){
			yield return null;
		}
    }
}
