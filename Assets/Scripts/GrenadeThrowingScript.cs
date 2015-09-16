using UnityEngine;
using System.Collections;

public class GrenadeThrowingScript : MonoBehaviour {

    public GameObject granade;
    public float grenadeForce;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.G)) {
            GameObject g = Instantiate(granade, this.transform.position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))) as GameObject;
            g.GetComponent<Rigidbody>().AddForce(transform.forward * grenadeForce, ForceMode.VelocityChange);
            g.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5));
        }
	}
}
