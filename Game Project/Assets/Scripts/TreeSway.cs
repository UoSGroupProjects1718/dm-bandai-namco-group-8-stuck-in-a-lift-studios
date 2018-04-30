using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSway : MonoBehaviour {

	public float maxSway = 1;

	private float angle = -90f;
	private float toDegrees = Mathf.PI/180f;
	private float startWidth;
	private Vector3 newPosition;

	void Start () {
		startWidth = transform.localPosition.x;
		newPosition = transform.localPosition;
	}

	void Update () {
		angle += newPosition.z * Time.deltaTime;
		if (angle > 270){
			angle -= 360;
		}
		newPosition.x = startWidth + maxSway * (2 + Mathf.Sin(angle * toDegrees));
		transform.localPosition = newPosition;
	}
}
