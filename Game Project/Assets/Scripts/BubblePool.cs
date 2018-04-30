using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePool : MonoBehaviour {

	public GameObject bubble;
	public GameObject bubbleHolder;
	public int pooledAmount = 10;
	public float frequency = 2f;

	private float random;
	private List<GameObject> bubbles;

	void Start () {
		bubbles = new List<GameObject>();
		for (int i=0; i<pooledAmount; i++){
			GameObject obj = (GameObject)Instantiate(bubble);
			obj.transform.parent = bubbleHolder.transform;
			obj.SetActive(false);
			bubbles.Add(obj);
		}
	}

	void Update(){
		random = Random.Range(0, 100);
		if (random <= frequency){
			CreateBubble();
		}
	}

	private void CreateBubble () {
		for (int i=0; i<bubbles.Count; i++){
			if (!bubbles[i].activeInHierarchy){
				bubbles[i].transform.position = transform.position;
				bubbles[i].transform.rotation = transform.rotation;
				bubbles[i].SetActive(true);
				break;
			}
		}
	}
}
