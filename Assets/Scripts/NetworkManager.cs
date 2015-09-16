using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

    public InputField hostServerNameInput;
    public InputField hostServerPortInput;
    public GameObject serverListPanel;
    public GameObject serverListButtonPrefab;

    private string serverTypeName = "TitanDeathmatch";
    private string hostServerName;
    private int hostServerPort;
    private int hostServerPlayerAmount;

    private HostData[] hostList;

    public GameObject player;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        GetComponent<NetworkView>().group = 1;
    }

    public void StartServer() {
        if (!Network.isClient && !Network.isServer) {
            hostServerName = hostServerNameInput.text;
            hostServerPort = int.Parse(hostServerPortInput.text);
            hostServerPlayerAmount = 4;

            Debug.Log("Server Name: " + hostServerName + " Port: " + hostServerPort + " Max Players: " + hostServerPlayerAmount);
            Network.InitializeServer(hostServerPlayerAmount, hostServerPort, !Network.HavePublicAddress());
            MasterServer.RegisterHost(serverTypeName, hostServerName);
        }
    }

    private void OnServerInitialized() {
        Debug.Log("Server " + hostServerName + " Initialized on port " + hostServerPort);
        Application.LoadLevel(1);
    }

    public void RefreshHostList() {
        MasterServer.RequestHostList(serverTypeName);
    }

    void OnMasterServerEvent(MasterServerEvent e) {
        if (e == MasterServerEvent.HostListReceived) {
            hostList = MasterServer.PollHostList();
            for(int i = 0; i < hostList.Length; i++) {
                Debug.Log("Server Detected, Creating Button");
                CreateServerButton(hostList[i].gameName, hostList[i].connectedPlayers, hostList[i].playerLimit, i, hostList[i]);
            }
        }
    }

    private void CreateServerButton(string gamename, int players, int maxplayers, int id, HostData hostData) {
        GameObject temp = Instantiate(serverListButtonPrefab) as GameObject;
        temp.GetComponentInChildren<Text>().text = gamename + "         " + players + "/" + maxplayers;
        temp.transform.SetParent(serverListPanel.transform);
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        temp.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -18.11f * id, 0);
        temp.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        Debug.Log(id);
        temp.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        temp.GetComponent<Button>().onClick.AddListener(() => JoinServer(hostData));
    }

    private void JoinServer(HostData hostData) {
        Network.Connect(hostData);
    }

    void OnConnectedToServer() {
        Debug.Log("Joined Server");
        Network.RemoveRPCsInGroup(0);
        Network.RemoveRPCsInGroup(1);
        Application.LoadLevel(1);
    }

    void OnLevelWasLoaded(int level) {
        if (level == 1) {
            print("Level is loaded.");
            NetworkViewID viewID = Network.AllocateViewID();
            GetComponent<NetworkView>().RPC("SpawnPlayer", RPCMode.AllBuffered, viewID);
            //SpawnPlayer(viewID);
        }
    }

    [RPC]
    void SpawnPlayer(NetworkViewID id) {
        GameObject p = Network.Instantiate(player, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), 0) as GameObject;
        p.GetComponent<NetworkView>().viewID = id;
        print("Spawning player.");
    }
}
