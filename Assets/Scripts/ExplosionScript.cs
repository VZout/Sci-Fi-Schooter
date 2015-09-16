using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

    public float time = 5;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, time);
	}
}
