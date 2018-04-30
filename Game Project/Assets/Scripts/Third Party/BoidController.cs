using UnityEngine;
using System.Collections;

public class BoidController : MonoBehaviour
{
    public float minVelocity = 5;
    public float maxVelocity = 20;
    public float randomness = 1;
    public int flockSize = 20;
    public GameObject prefab;
    public GameObject chasee;

    public Vector2 flockCenter;
    public Vector2 flockVelocity;

    private GameObject[] boids;
    private int livingCount;
    private bool isAlive;

    void Start() {
        isAlive = true;
        boids = new GameObject[flockSize];
        for (var i = 0; i < flockSize; i++)
        {
            Vector2 position = new Vector2(
                Random.value * GetComponent<Collider2D>().bounds.size.x,
                Random.value * GetComponent<Collider2D>().bounds.size.y
            ) - (Vector2)GetComponent<Collider2D>().bounds.extents;

            GameObject boid = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            boid.transform.parent = transform;
            boid.transform.localPosition = position;
            boid.GetComponent<BoidFlocking>().SetController(gameObject);
            boids[i] = boid;
        }
    }

    void Update() {
        if (!isAlive) {
            Destroy(gameObject);
        }
        Vector2 theCenter = Vector2.zero;
        Vector2 theVelocity = Vector2.zero;

        livingCount = 0;
        foreach (GameObject boid in boids)
        {
            theCenter += (Vector2)boid.transform.localPosition;
            theVelocity = theVelocity + boid.GetComponent<Rigidbody2D>().velocity;
            if (boid.activeSelf) {
                livingCount++;
            }
        }
        if (livingCount == 0){
			isAlive = false;
		}

        flockCenter = theCenter / (flockSize);
        flockVelocity = theVelocity / (flockSize);
    }

	public void SetFlockSize(int size){
		this.flockSize = size;
	}
}
