using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainMenuScript : MonoBehaviour {

    public GameObject controlsListPanel;
    public GameObject controlsButtonPrefab;

    private GameObject t;
    private float moveTargetX;
    private float moveTargetY;
    private Vector3 velocity = Vector3.zero;

    void Start() {
        moveTargetX = transform.position.x;
        moveTargetY = transform.position.y;
        t = new GameObject();
        CreateKeybindButton("Up", "W", 0);
        CreateKeybindButton("Down", "S", 1);
        CreateKeybindButton("Right", "D", 2);
        CreateKeybindButton("Left", "A", 3);
        CreateKeybindButton("Fire", "LMB", 4);
        CreateKeybindButton("Aim", "RMB", 5);
        CreateKeybindButton("Weapon slot 1", "1", 6);
        CreateKeybindButton("Weapon slot 2", "2", 7);
        CreateKeybindButton("Special", "F", 8);
        CreateKeybindButton("Grenade", "G", 9);
        CreateKeybindButton("Couch", "CTRL", 10);
        CreateKeybindButton("Class Selection", "N", 11);
        CreateKeybindButton("Team Selection", "M", 12);
    }

    void Update() {

        /* Camera Position and Rotation */
        float mx = Input.mousePosition.x;
        float my = Input.mousePosition.y;

        mx -= Screen.width / 2;
        my -= Screen.height / 2;

        //mx = Mathf.Clamp(mx, -1, 1);
        //my = Mathf.Clamp(my, -1, 1);
        mx = mx / 400;
        my = my / 400;

        mx += transform.position.x;
        my += transform.position.y;

        t.transform.position = new Vector3(mx, my, 0);
        var targetRotation = Quaternion.LookRotation(t.transform.position - transform.position);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.5f * Time.deltaTime);

        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(moveTargetX, moveTargetY, transform.position.z), ref velocity, 0.3f, 20f);
    }

    private void CreateKeybindButton(string name, string key, int id) {
        //Set Content Box The Right Size For Slider
        //controlsListPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(controlsListPanel.GetComponent<RectTransform>().anchoredPosition.x, controlsListPanel.GetComponent<RectTransform>().anchoredPosition.y + (4 * id));

        GameObject temp = Instantiate(controlsButtonPrefab) as GameObject;
        temp.transform.Find("Key Name").GetComponentInChildren<Text>().text = name;
        temp.transform.Find("Key").GetComponentInChildren<Text>().text = key;
        temp.transform.SetParent(controlsListPanel.transform);
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        temp.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -25.15f * id, 0);
        temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        temp.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        //temp.GetComponent<Button>().onClick.AddListener(() => JoinServer(hostData));
    }

    public void MoveToX(float targetX) {
        moveTargetX = targetX;
    }

    public void MoveToY(float targetY) {
        moveTargetY = targetY;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SwitchScene(int id) {
    	Application.LoadLevel(0);
    }
}
