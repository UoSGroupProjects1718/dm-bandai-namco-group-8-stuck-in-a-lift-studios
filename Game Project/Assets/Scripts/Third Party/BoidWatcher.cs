using UnityEngine;
using System.Collections;

public class BoidWatcher : MonoBehaviour
{
    public Transform boidController;

    void LateUpdate()
    {
        if (boidController)
        {
            Vector2 watchPoint = boidController.GetComponent<BoidController>().flockCenter;
            transform.LookAt(watchPoint + (Vector2)boidController.transform.position);
        }
    }
}