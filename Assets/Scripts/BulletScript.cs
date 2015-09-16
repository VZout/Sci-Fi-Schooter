using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BulletScript : NetworkBehaviour {

    public GameObject decal;
    public int damage = 30;
    public int owner = 1;

	// Use this for initialization
	void Start () {
		Invoke ("Destroyer", 10);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0, 0, 2));

        RaycastHit hit;
        Ray r = new Ray(transform.position, transform.forward);

        if(Physics.Raycast(r, out hit, 2)) {
            Debug.DrawRay(transform.position, transform.forward);
            if(hit.collider.tag == "Solid Object") {
				Spawn(decal, hit);
				Destroyer();
            }
            else if (hit.collider.tag == "Player") {
                hit.collider.GetComponent<PlayerScript>().health -= damage;
                if(hit.collider.GetComponent<PlayerScript>().health <= 0) {
                    hit.collider.GetComponent<PlayerScript>().Death();
                    GameObject.Find("PlayerManager").GetComponent<GameManager>().addScore(owner);
                }
            }
        }
	}
	
	void Spawn(GameObject decal, RaycastHit hit) {
		Instantiate(decal, hit.point, hit.transform.rotation);
	}
	
	public void Destroyer() {
		Destroy (this.gameObject);
	}
}
