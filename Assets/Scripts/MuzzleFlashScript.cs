using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MuzzleFlashScript : NetworkBehaviour {

    public float aliveTime = 0.1f;

	// Use this for initialization
	void Start () {
	    Invoke("DestroyMuzzleFlash", aliveTime);
	}

    void DestroyMuzzleFlash() {
		NetworkServer.Destroy (this.gameObject);
    }
}
