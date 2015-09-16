using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

    [Header("Stats")]
    public bool playerIsAlive = true;
    public int health = 100;
    public Text healthText;
    public int id = 1;
    public GameObject scoreCanvas;

    [Header("Weapons")]
    public GameObject assault;
    public GameObject assaultModel;
    public GameObject shotgun;
	public GameObject shotgunModel;
	public GameObject handgun;
	public GameObject handgunModel;

    [Header("Movement")]
    public bool forward;
    public bool backward;
    public bool left;
    public bool right;
    public bool running;
    public bool aiming;
    public bool jump;
    public float walkSpeed = 50;
    public float walkSpeedSideways = 4;
    public float walkSpeedBackwards = 4;
    public float maxWalkSpeed = 5;
    public float maxWalkSpeedBackwards = 4;
    public float maxWalkSpeedSideways = 4;
    public float runningSpeed = 60;
    public float maxRunningSpeed = 7;
    public float jumpForce = 100;
    public float distToGround = 1;
    public bool reloading;

    [Header("SpawnPoints")]
    public GameObject[] spawnpoints;

    [Header("Switch Times")]
    public float switchTime1 = 1;
    public float switchTime2 = 1;

    [Header("Camera")]
    public float sensetivity = 10;
    public float aimedSensetivity = 8;

    [Header("Death")]
    public GameObject deathExplosion;
    public Canvas classSelect;
    public int respawnWeaponID;

    [Header("Destructable Objects For Multiplayer")]
	public GameObject cam;
    public GameObject reflectionProbe;
	public AudioListener audioListener;

	[Header("UNET stuff")]
	public Transform myTransform;
	public float lerpRate = 15;

	[SerializeField] private Transform playerTransform;
	[SerializeField] private Transform headTransform;

	private Vector3 lastPos;
	private float tresholdPosition = 0.5f;
	private Quaternion lastPlayerRot;
	private Quaternion lastHeadRot;
	private float tresholdRotation = 5;

	[SyncVar] private Vector3 syncPos;
    [SyncVar] private Quaternion syncGunPos;
	[SyncVar] private Quaternion syncPlayerRotation;
	[SyncVar] private Quaternion syncHeadRotation;
	
    void Start() {	
		if (playerIsAlive && isLocalPlayer) {
			   Cursor.visible = false;
   				Cursor.lockState = CursorLockMode.Locked;
			spawnpoints = GameObject.FindGameObjectsWithTag ("Spawnpoint");
			this.transform.position = spawnpoints [Random.Range (0, 4)].transform.position;
			this.transform.rotation = spawnpoints [Random.Range (0, 4)].transform.rotation;
			respawnWeaponID = 0;
			id = 1;
			Debug.Log ("Connection Count " + Network.connections.Length);
			gameObject.layer = 10;
			transform.FindChild ("Head").gameObject.layer = 10;
			transform.FindChild ("Body").gameObject.layer = 10;
			Physics.IgnoreLayerCollision (10, 9);
			scoreCanvas = transform.FindChild ("PlayerListAndScore").gameObject;
		} else {
			Destroy(cam.gameObject);
			Destroy(reflectionProbe.gameObject);
			audioListener.enabled = false;
		}
    }
	
    void Update() {
        if (playerIsAlive && isLocalPlayer) {
            healthText.text = "Health: " + health + "%";

            if (Input.GetKey(KeyCode.Tab)) {
                scoreCanvas.SetActive(true);
            } else
                scoreCanvas.SetActive(false);

            if (Input.GetKey(KeyCode.W)) {
                forward = true;
                backward = false;
                if (Input.GetKey(KeyCode.LeftShift) && !left && !backward && !right && !reloading)
                    running = true;
                else
                    running = false;
            } else {
                forward = false;
                running = false;
            }

            if (Input.GetKey(KeyCode.S)) {
                forward = false;
                backward = true;
            } else
                backward = false;

            if (Input.GetKey(KeyCode.D)) {
                left = true;
                right = false;
            } else
                left = false;

            if (Input.GetKey(KeyCode.A)) {
                left = false;
                right = true;
            } else
                right = false;

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !reloading) {
                jump = true;
            }

            if (Input.GetMouseButton(1) && !running && !reloading)
                aiming = true;
            else
                aiming = false;

            if (Input.GetKey(KeyCode.Comma) || Input.GetKey(KeyCode.C))
                Death();

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                if(shotgun.activeSelf) {
                    shotgunModel.GetComponent<Animator>().SetTrigger("Switch");
                    Invoke("switchToAssault", switchTime2);
                }
				if(handgun.activeSelf) {
					handgunModel.GetComponent<Animator>().SetTrigger("Switch");
					Invoke("switchToAssault", switchTime1);
				}
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) {
				if(assault.activeSelf) {
					assaultModel.GetComponent<Animator>().SetTrigger("Switch");
					Invoke("switchToShotgun", switchTime1);
				}

				if(handgun.activeSelf) {
					handgunModel.GetComponent<Animator>().SetTrigger("Switch");
					Invoke("switchToShotgun", switchTime1);
				}
            }

			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				if(assault.activeSelf) {
					assaultModel.GetComponent<Animator>().SetTrigger("Switch");
					Invoke("switchToHandgun", switchTime1);
				}
				if(shotgun.activeSelf) {
					shotgunModel.GetComponent<Animator>().SetTrigger("Switch");
					Invoke("switchToHandgun", switchTime1);
				}
			}

            UpdatePoV();
            UpdateSensitivity();
        }
    }
	
    void FixedUpdate() {

		TransmitPosition ();
		TransmitRotation ();
		LerpPosition ();
		LerpRotations ();
		
		if (forward) {
				if (running) {
					GetComponent<Rigidbody> ().AddForce (transform.forward * walkSpeed, ForceMode.Acceleration);
				} else {
					GetComponent<Rigidbody> ().AddForce (transform.forward * runningSpeed, ForceMode.Acceleration);
				}
			}
			if (backward)
				GetComponent<Rigidbody> ().AddForce (transform.forward * -walkSpeedBackwards, ForceMode.Acceleration);
			if (right)
				GetComponent<Rigidbody> ().AddForce (transform.right * -walkSpeedSideways, ForceMode.Acceleration);
			if (left)
				GetComponent<Rigidbody> ().AddForce (transform.right * walkSpeedSideways, ForceMode.Acceleration);
			if (jump && IsGrounded ())
				Jump ();

			LimitVelocity ();
    }

    void Jump() {
        jump = false;
        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, jumpForce, GetComponent<Rigidbody>().velocity.z);
    }

    private void UpdatePoV() {
        if(running)
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 90, Time.deltaTime * 6);
        else if(aiming)
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 10);
        else
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 80, Time.deltaTime * 10);
    }

    public void UpdateSensitivity() {
        if (aiming)
            this.GetComponent<SimpleMouseRotator>().rotationSpeed = aimedSensetivity;
        else
            this.GetComponent<SimpleMouseRotator>().rotationSpeed = sensetivity;
    }

    public bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void LimitVelocity() {
        Vector3 localVelocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);

        localVelocity.x = Mathf.Clamp(localVelocity.x, -maxWalkSpeedSideways, maxWalkSpeedSideways);
        if (running) {
            localVelocity.z = Mathf.Clamp(localVelocity.z, -maxWalkSpeedBackwards, maxRunningSpeed);
        } else {
            localVelocity.z = Mathf.Clamp(localVelocity.z, -maxWalkSpeedBackwards, maxWalkSpeed);
        }
        localVelocity.y = Mathf.Clamp(localVelocity.y, -20, 20);

        GetComponent<Rigidbody>().velocity = transform.TransformDirection(localVelocity);
    }

    public void Death() {

        for(int i = 0; i < gameObject.GetComponentsInChildren<MeshRenderer>().Length; i++)
            gameObject.GetComponentsInChildren<MeshRenderer>()[i].enabled = false;

        for (int i = 0; i < gameObject.GetComponentsInChildren<BoxCollider>().Length; i++)
            gameObject.GetComponentsInChildren<BoxCollider>()[i].enabled = false;

        for (int i = 0; i < gameObject.GetComponentsInChildren<CapsuleCollider>().Length; i++)
            gameObject.GetComponentsInChildren<CapsuleCollider>()[i].enabled = false;

        for (int i = 0; i < gameObject.GetComponentsInChildren<Rigidbody>().Length; i++)
            gameObject.GetComponentsInChildren<Rigidbody>()[i].useGravity = false;

        playerIsAlive = false;
        Instantiate(deathExplosion, transform.position, transform.rotation);
        classSelect.gameObject.SetActive(true);
        Invoke("Respawn", 5);
    }

    public void Respawn() {
        classSelect.gameObject.SetActive(false);
        health = 100;
        playerIsAlive = true;

		if (isLocalPlayer) {
			this.transform.position = spawnpoints [Random.Range (0, 3)].transform.position;
			this.transform.rotation = spawnpoints [Random.Range (0, 3)].transform.rotation;
		}

        for (int i = 0; i < gameObject.GetComponentsInChildren<MeshRenderer>().Length; i++)
            gameObject.GetComponentsInChildren<MeshRenderer>()[i].enabled = true;

        for (int i = 0; i < gameObject.GetComponentsInChildren<BoxCollider>().Length; i++)
            gameObject.GetComponentsInChildren<BoxCollider>()[i].enabled = true;

        for (int i = 0; i < gameObject.GetComponentsInChildren<CapsuleCollider>().Length; i++)
            gameObject.GetComponentsInChildren<CapsuleCollider>()[i].enabled = true;

        for (int i = 0; i < gameObject.GetComponentsInChildren<Rigidbody>().Length; i++)
            gameObject.GetComponentsInChildren<Rigidbody>()[i].useGravity = true;

        switch (respawnWeaponID) {
            case 0:
                assault.gameObject.SetActive(true);
                shotgun.gameObject.SetActive(false);
                break;
            case 1: 
                assault.gameObject.SetActive(false);
                shotgun.gameObject.SetActive(true);
            break;
        }
    }

    public void switchToAssault() {
        assault.SetActive(true);
		handgun.SetActive (false);
        shotgun.SetActive(false);
    }

    public void switchToShotgun() {
        assault.SetActive(false);
		handgun.SetActive (false);
        shotgun.SetActive(true);
    }

	public void switchToHandgun() {
		assault.SetActive(false);
		shotgun.SetActive(false);
		handgun.SetActive (true);
	}
	
	void LerpPosition() {
		if(!isLocalPlayer) {
			myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
		}
	}

	void LerpRotations() {
		if (!isLocalPlayer) {
			playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
			headTransform.rotation = Quaternion.Lerp (headTransform.rotation, syncHeadRotation, Time.deltaTime * lerpRate);
		}
	}
	
	[Command]
	void CmdProvidePositionToServer(Vector3 pos) {
		syncPos = pos;
	}

	[Command]
	void CmdProvideRotationsToServer(Quaternion playerRot, Quaternion headRot) {
		syncPlayerRotation = playerRot;
		syncHeadRotation = headRot;
	}
	
	[ClientCallback]
	void TransmitPosition() {
		if (isLocalPlayer && Vector3.Distance (myTransform.position, lastPos) > tresholdPosition){
			CmdProvidePositionToServer (transform.position);
			lastPos = myTransform.position;
		}
	}

	[ClientCallback]
	void TransmitRotation() {
		if (isLocalPlayer && (Quaternion.Angle (playerTransform.rotation, lastPlayerRot) > tresholdRotation ||
		                      Quaternion.Angle(headTransform.rotation, lastHeadRot) > tresholdRotation)) {
			CmdProvideRotationsToServer (playerTransform.rotation, headTransform.rotation);
			lastPlayerRot = playerTransform.rotation;
			lastHeadRot = headTransform.rotation;
		}
	}

	[Command]
	public void CmdSpawn() {
		Debug.Log ("Spawning Bullet");
        GameObject bullet = GetComponentInChildren<GunScript>().bullet;
        GameObject bulletFlash = GetComponentInChildren<GunScript>().bulletFlash;
        Transform fireingPoint = GetComponentInChildren<GunScript>().fireingPoint;
        float vSpread = GetComponentInChildren<GunScript>().vSpread;
        float hSpread = GetComponentInChildren<GunScript>().hSpread;
		GameObject b = (GameObject)Instantiate (bullet, fireingPoint.position, Quaternion.Euler (fireingPoint.rotation.eulerAngles.x + Random.Range (-vSpread / 2, vSpread / 2), fireingPoint.rotation.eulerAngles.y + Random.Range (-hSpread / 2, hSpread / 2), fireingPoint.rotation.eulerAngles.z));
		Debug.Log ("Finished Spawning Bullet");
		b.GetComponent<BulletScript> ().owner = 1;
		NetworkServer.Spawn (b);
		GameObject f = Instantiate (bulletFlash, fireingPoint.transform.position, fireingPoint.transform.rotation) as GameObject;
		NetworkServer.Spawn (f);
	}

}
