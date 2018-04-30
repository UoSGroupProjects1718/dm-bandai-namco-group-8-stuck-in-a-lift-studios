using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

	public GameObject[] laserbeams;
	public int wallActiveCount;
	public float wallActiveTime;

	private bool wallSelect;

	void Start () {
		wallSelect = true;
	}

	void Update () {
		if (wallSelect){
			StartCoroutine(wallSelector());
		}
	}

	private IEnumerator wallSelector(){
		wallSelect = false;
		float elapsed = 0.0f;

		for (int i=0; i<laserbeams.Length; i++){
			laserbeams[i].SetActive(false);
		}
		for (int j=0; j<wallActiveCount; j++){
			laserbeams[Random.Range(0, laserbeams.Length)].SetActive(true);
		}

		while (elapsed < wallActiveTime){
			elapsed += Time.deltaTime;
			yield return null;
		}
		wallSelect = true;
	}
}
