using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

	public LayerMask objectsToTouch;

	private bool[] hasCaptured;
	private RaycastHit2D[] hit;
	private Vector2[] offset;

	void Start () {
		hasCaptured = new bool[10];
		hit = new RaycastHit2D[10];
		offset = new Vector2[10];
	}

	void Update () {
//		if (Input.touchCount == 0){
			return;
/*		}
		for (int i=0; i<Input.touchCount; i++){
			if (Input.GetTouch(i).phase == TouchPhase.Began){
				Vector2 touchPos = new Vector2(
					Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position).x,
					Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position).y);
				hit[i] = Physics2D.Raycast(touchPos, Vector2.up, 0, objectsToTouch);
				Player currentPlayer = hit[i].collider.gameObject.GetComponent<Player>();
				if (currentPlayer.GetPlayerMode() == PlayerMode.HUMAN && !currentPlayer.IsTakingDamage()){
					if (hit[i].collider != null){
						hasCaptured[i] = true;
						offset[i].x = touchPos.x - hit[i].collider.transform.position.x;
						offset[i].y = touchPos.y - hit[i].collider.transform.position.y;
					}
				}
			}

			if (hasCaptured[i] == true){
				hit[i].collider.transform.position = new Vector2(
					Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position).x,
					Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position).y) + offset[i];
			}

			if (Input.GetTouch(0).phase == TouchPhase.Ended){
				hasCaptured[i] = false;
			}
		}
*/
	}
}
