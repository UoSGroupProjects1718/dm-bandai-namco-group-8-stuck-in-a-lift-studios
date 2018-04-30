using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnImpact : MonoBehaviour {

	public void OnCollisionEnter2D(Collision2D c){
		if (c.gameObject.tag == "1" || c.gameObject.tag == "2"){
			Destroy(c.gameObject);
		} else if (c.gameObject.tag == "bubble"){
			c.gameObject.SetActive(false);
		} 
	}
}
