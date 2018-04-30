using UnityEngine;
using System.Collections;

public class Particlerandomseed : MonoBehaviour {

    void Awake() {
        if (this.GetComponent<ParticleSystem>() == null)
            return;

        this.GetComponent<ParticleSystem>().Stop();
        this.GetComponent<ParticleSystem>().randomSeed = (uint)Random.Range(0, 9999999);
        this.GetComponent<ParticleSystem>().Play();
    }

}
