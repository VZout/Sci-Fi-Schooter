using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GunScript : MonoBehaviour {

    public bool isShotgun = false;
    public GameObject modelWithAnimator;

    public float shootTime = 1f;
    public float reloadTime = 1.2f;
    public bool readyToShoot = true;
    public int shotgunPallets = 5;

    public GameObject bullet;
	public Transform fireingPoint;
	public GameObject bulletFlash;
	public float horiztonalSpreadHip = 2;
	public float verticalSpreadHip = 2;
	public float horiztonalSpreadAim = 1;
	public float verticalSpreadAim = 1;
	public float walkAdjustor = 1;
    public int bullets = 31;
    public int maxBullets = 31;
    public Text bulletCount;

    public Vector3 normalPos;
    public Vector3 zoomedPos = new Vector3(0,0,0);
    public float zoomSpeed = 1;

    public float fireRateDelay;
    public float timer = 0;

	[SerializeField] public float hSpread;
	[SerializeField] public float vSpread;
	
    void Start() {
		normalPos = transform.localPosition;

		if(!transform.parent.parent.GetComponent<NetworkIdentity>().isLocalPlayer) {
			this.enabled = false;
		}
    }
	
    void Update() {
		if (transform.parent.parent.GetComponent<PlayerScript>().playerIsAlive) {
            bulletCount.text = "Ammo: " + bullets + "/" + maxBullets;
            timer += 1 * Time.deltaTime;

            if (transform.parent.parent.GetComponent<PlayerScript>().aiming) {
                vSpread = verticalSpreadAim;
                hSpread = horiztonalSpreadAim;
                if (transform.parent.parent.GetComponent<PlayerScript>().forward || transform.parent.parent.GetComponent<PlayerScript>().backward || transform.parent.parent.GetComponent<PlayerScript>().left || transform.parent.parent.GetComponent<PlayerScript>().right) {
                    vSpread = verticalSpreadAim + walkAdjustor;
                    hSpread = horiztonalSpreadAim + walkAdjustor;
                }
            } else {
                vSpread = verticalSpreadHip;
                hSpread = horiztonalSpreadHip;
                if (transform.parent.parent.GetComponent<PlayerScript>().forward || transform.parent.parent.GetComponent<PlayerScript>().backward || transform.parent.parent.GetComponent<PlayerScript>().left || transform.parent.parent.GetComponent<PlayerScript>().right) {
                    vSpread = verticalSpreadHip + walkAdjustor;
                    hSpread = horiztonalSpreadHip + walkAdjustor;
                }
            }

            if(Input.GetKeyDown(KeyCode.R) && !transform.parent.parent.GetComponent<PlayerScript>().reloading) {
                Reload();
            }

            if (Input.GetMouseButton(0) && !GetComponentInParent<PlayerScript>().running && timer > fireRateDelay) {
				Shoot();
                timer = 0;
            }


            if (GetComponentInParent<PlayerScript>().forward || GetComponentInParent<PlayerScript>().backward || GetComponentInParent<PlayerScript>().left || GetComponentInParent<PlayerScript>().right)
                modelWithAnimator.GetComponent<Animator>().SetBool("Walking", true);
            else
                modelWithAnimator.GetComponent<Animator>().SetBool("Walking", false);

            if (GetComponentInParent<PlayerScript>().running) {
                modelWithAnimator.GetComponent<Animator>().SetBool("Walking", false);
                modelWithAnimator.GetComponent<Animator>().SetBool("Running", true);
            }
            else
                modelWithAnimator.GetComponent<Animator>().SetBool("Running", false);

            if (GetComponentInParent<PlayerScript>().aiming) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, zoomedPos, zoomSpeed * Time.deltaTime);
                modelWithAnimator.GetComponent<Animator>().SetBool("Aiming", true);
            } else {
                transform.localPosition = Vector3.Lerp(transform.localPosition, normalPos, zoomSpeed * Time.deltaTime);
                modelWithAnimator.GetComponent<Animator>().SetBool("Aiming", false); 
            }
        }
    }
	
    void Shoot() {
			if (bullets > 0 && !transform.parent.parent.GetComponent<PlayerScript> ().reloading) {
				if (!isShotgun && readyToShoot) {
					transform.parent.parent.GetComponent<PlayerScript>().CmdSpawn();
					modelWithAnimator.GetComponent<Animator> ().SetTrigger ("Fire");
					bullets--;
				}
				if (isShotgun && readyToShoot) {
					for (int i = 0; i < shotgunPallets; i++) {
						transform.parent.parent.GetComponent<PlayerScript>().CmdSpawn();
					}
					readyToShoot = false;
					modelWithAnimator.GetComponent<Animator> ().SetTrigger ("Fire");
					bullets--;
					Invoke ("CompleteShootReload", shootTime);
				}
			} else {
				Reload ();
			}
    }

    public void Reload() {
        if(!transform.parent.parent.GetComponent<PlayerScript>().reloading && bullets != maxBullets && transform.parent.parent.GetComponent<PlayerScript>().IsGrounded()) {
            modelWithAnimator.GetComponent<Animator>().SetTrigger("Reload");

            transform.parent.parent.GetComponent<PlayerScript>().reloading = true;
            Invoke("CompleteReload", reloadTime);
        }
    }

    public void CompleteReload() {
        transform.parent.parent.GetComponent<PlayerScript>().reloading = false;
        bullets = maxBullets;
    }

    public void CompleteShootReload() {
        readyToShoot = true;
    }
}
