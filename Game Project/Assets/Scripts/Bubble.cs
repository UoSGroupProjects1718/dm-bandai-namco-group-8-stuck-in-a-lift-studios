using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

	public float minSpeedStart = 0.7f;
	public float maxSpeedStart = 1.2f;
	public float minSpeedEnd = 3.0f;
	public float maxSpeedEnd = 5.0f;

	public float minSizeStart = 0.1f;
	public float maxSizeStart = 0.15f;
	public float minSizeEnd = 0.7f;
	public float maxSizeEnd = 1f;

	public float frequency = 20.0f;  // Speed of sine movement
	public float magnitude = 0.5f;   // Size of sine movement

	private float speed;
	private float size;
	private float maxSpeed;
	private float maxSize;
	private Vector3 axis;
	private Vector3 pos;
	private Vector3 scale;
	private Vector3 startScale;

	void OnEnable () {
		pos = transform.position;
//		DestroyObject(gameObject, 1.0f);
		axis = transform.right;
		speed = Random.Range(minSpeedStart, maxSpeedStart);
		maxSpeed = Random.Range(minSpeedEnd, maxSpeedEnd);
		size = Random.Range(minSizeStart, maxSizeStart);
		maxSize = Random.Range(minSizeEnd, maxSizeEnd);
		scale = transform.localScale;
		startScale = transform.localScale;

		frequency = size*5f;
		magnitude = speed;
	}

	void Update () {
		size += (maxSize - size)*0.01f;
		scale.x = size * startScale.x;
		scale.y = size * startScale.y;
		transform.localScale = scale;
		speed += (maxSpeed - speed)*0.01f;
		pos += transform.up * Time.deltaTime * speed;
		transform.position = pos + axis * Mathf.Sin (Time.time * frequency) * magnitude;
		if (transform.position.y > 14){
			gameObject.SetActive(false);
		}
	}
}
