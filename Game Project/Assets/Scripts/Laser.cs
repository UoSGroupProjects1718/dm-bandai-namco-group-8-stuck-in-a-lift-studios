using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	public Transform startPoint;
	public Transform endPoint;

	private LineRenderer lineRenderer;

	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = true;
		lineRenderer.SetWidth(0.3f, 0.3f);
	}

	void Update () {
		lineRenderer.SetPosition(0, startPoint.position);
		lineRenderer.SetPosition(1, endPoint.position);
	}
}
