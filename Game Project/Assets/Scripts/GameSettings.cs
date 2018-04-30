using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerCharacter {
	KRAB,
	BEETLE,
	ROACH
}

public enum PlayerMode {
	HUMAN,
	COMPUTER,
	ERROR
}

public enum GameMode {
	DEFAULT,
	SINGLE_PICKUP,
	BATTLE,
	RANDOM
}

public class GameSettings : MonoBehaviour {

	public Sprite audioOnSprite;
	public Sprite audioOffSprite;

	public static GameSettings instance = null;

	public bool soundOff { get; set; }
	public GameMode gameMode { get; set; }
	public PlayerMode playerMode { get; set; }
	public PlayerCharacter player1Char { get; set; }
	public PlayerCharacter player2Char { get; set; }
	public int roundCount { get; set; }
	public int roundTime { get; set; }

	private AudioSource[] audioSources;
	private Button audioButton;
	private GameObject audioButtonObj;

	void Awake() {
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy(gameObject);
		}
		soundOff = false;
		DontDestroyOnLoad(transform.gameObject);

	}

	void Update(){
		audioSources = FindObjectsOfType<AudioSource>();
		foreach(AudioSource audio in audioSources){
			audio.mute = soundOff;
		}
		audioButtonObj = GameObject.FindGameObjectWithTag("AudioToggle");
		if (audioButtonObj == null){
			return;
		} else {
			audioButton = audioButtonObj.GetComponent<Button>();
		}
		if (soundOff){
			audioButton.GetComponent<Image>().sprite = audioOffSprite;
		} else {
			audioButton.GetComponent<Image>().sprite = audioOnSprite;
		}
	}

	public void ToggleSound(){
		soundOff = !soundOff;
	}
}
