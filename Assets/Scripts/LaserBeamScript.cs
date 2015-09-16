using UnityEngine;
using System.Collections;

public class LaserBeamScript : MonoBehaviour {

    private Vector2 offset;

	// Use this for initialization
	void Start () {
	    offset = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
        offset.x += 0.1f;
	 this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);
    }
}
