using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseMenu;

	private bool isPaused = false;

	void Start () {
		
	}

	void Update () {
		if (Input.GetButtonDown("Pause")){
			isPaused = !isPaused;
			GameManager.instance.setGamePaused(isPaused);
			if (isPaused){
				Time.timeScale = 0.0f;
				pauseMenu.SetActive(true);
			} else {
				Time.timeScale = 1.0f;
				pauseMenu.SetActive(false);
			}
		}
	}

	public void ResumeButtonPressed(){
		isPaused = false;
		GameManager.instance.setGamePaused(false);
		Time.timeScale = 1.0f;
		pauseMenu.SetActive(false);
	}

	public void QuitToMenuButtonPressed(){
		isPaused = false;
		GameManager.instance.setGamePaused(false);
		Time.timeScale = 1.0f;
		pauseMenu.SetActive(false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void QuitToDesktopButtonPressed(){
		Application.Quit();
	}
}
