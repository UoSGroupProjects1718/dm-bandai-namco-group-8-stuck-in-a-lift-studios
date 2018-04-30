using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

	public float size;
	public float min = 0.15f;
	public float max = 0.17f;

	private float randomScale_x;
	private float randomScale_y;
	private float rotationSpeed;
	private Quaternion targetRotation;

	public float getSize(){
		return this.size;
	}

	public void Start(){
		randomScale_x = Random.Range(min, max);
		randomScale_y = Random.Range(min, max);
		rotationSpeed = Random.Range(-1f, 1f);
		targetRotation = transform.rotation;
	}

	public void Update(){
		float x = Mathf.PingPong((Time.time * randomScale_x), randomScale_x - size) + size;
		float y = Mathf.PingPong((Time.time * randomScale_y), randomScale_y - size) + size;
		transform.localScale = new Vector3(x, y, transform.localScale.z);
		targetRotation *=  Quaternion.AngleAxis(10f, Vector3.forward);
		transform.rotation= Quaternion.Lerp (transform.rotation, targetRotation , rotationSpeed * Time.deltaTime);
	}

}
