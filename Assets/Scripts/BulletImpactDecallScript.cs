using UnityEngine;
using System.Collections;

public class BulletImpactDecallScript : MonoBehaviour {

    private float r, g, b, a;

	// Use this for initialization
	void Start () {
        r = GetComponentInChildren<Renderer>().material.color.r;
        g = GetComponentInChildren<Renderer>().material.color.g;
        b = GetComponentInChildren<Renderer>().material.color.b;
        a = GetComponentInChildren<Renderer>().material.color.a;
    }
	
	// Update is called once per frame
	void Update () {
        Invoke("FadeAway", 2);
    }

    void FadeAway() {
        GetComponentInChildren<Renderer>().material.color = new Color(r, g, b, a);
        a -= 0.01f;
        if (a < 0)
            Destroy(this.gameObject);
    }
}
