using UnityEngine;
using System.Collections;

public class GrenadeScript : MonoBehaviour {

    public float delay;
    public GameObject explosion;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   void OnCollisionEnter(Collision coll) {
        Invoke("Explode", delay);
   }

    void Explode() {
        Instantiate(explosion, this.transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        Destroy(this.gameObject);
    }
}
