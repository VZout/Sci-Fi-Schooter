using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour {

    public Text p1t;
    public Text p2t;
    public Text p3t;
    public Text p4t;

    void Update() {
        p1t.text = "Host: " + GameObject.Find("PlayerManager").GetComponent<GameManager>().scorep0 + " Kills";
        p2t.text = "Player 1: " + GameObject.Find("PlayerManager").GetComponent<GameManager>().scorep1 + " Kills";
        p3t.text = "Player 2: " + GameObject.Find("PlayerManager").GetComponent<GameManager>().scorep2 + " Kills";
        p4t.text = "Player 3: " + GameObject.Find("PlayerManager").GetComponent<GameManager>().scorep3 + " Kills";
    }
}
