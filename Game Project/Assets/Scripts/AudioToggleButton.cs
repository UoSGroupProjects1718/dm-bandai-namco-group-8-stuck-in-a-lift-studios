using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioToggleButton : MonoBehaviour {

	void Start () {
		Button button = GetComponent<Button>();
		button.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick() {
		GameSettings.instance.ToggleSound();
    }
}
