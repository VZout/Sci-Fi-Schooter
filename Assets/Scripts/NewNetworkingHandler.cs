using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NewNetworkingHandler : MonoBehaviour {
	
	public NetworkManager netManager;
	public Text hostPort;
	public Text joinIP;
	public Text joinPort;

	public void ConnectToServer() {
		GetComponent<NetworkManager>().networkPort = int.Parse(joinPort.text);
		GetComponent<NetworkManager>().networkAddress = joinIP.text;
		GetComponent<NetworkManager>().StartClient ();
	}
	
	public void HostServer() {
		GetComponent<NetworkManager>().networkPort = int.Parse(hostPort.text);
		GetComponent<NetworkManager>().StartHost ();
	}
}
