using UnityEngine;
using System.Collections;

public class ClassSelect : MonoBehaviour {

    public GameObject player;

    public void selectShotgun() {
        player.GetComponent<PlayerScript>().respawnWeaponID = 1;
    }

    public void selectAssault() {
        player.GetComponent<PlayerScript>().respawnWeaponID = 0;
    }
}
