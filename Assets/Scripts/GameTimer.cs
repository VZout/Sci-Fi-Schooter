using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameTimer : NetworkBehaviour {

	// Use this for iisnitialization
	void Start () {
		Invoke ("FinishGame", 330);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FinishGame() {
		Debug.Log (":asdsad");
		NetworkServer.DisconnectAll ();
		NetworkServer.Shutdown ();
		NetworkClient.ShutdownAll ();
		Application.LoadLevel (2);
		   Cursor.visible = true;
   		Cursor.lockState = CursorLockMode.None;
	}
}
