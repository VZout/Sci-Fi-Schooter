using UnityEngine;
using System.Collections;

public class GunSway : MonoBehaviour {

 public float MoveAmount = 1;
 public float MoveSpeed = 2;
 public float AimedMoveAmount = 1;
 public float AimedMoveSpeed = 2;
 public GameObject GUN;
 public float MoveOnX;
 public float MoveOnY;
 public Vector3 DefaultPos;
 public Vector3 NewGunPos;

    void Start() {
        DefaultPos = transform.localPosition;
    }

    void FixedUpdate() {
		if (transform.parent.parent.parent.GetComponent<PlayerScript> ().aiming) {
			MoveOnX = Input.GetAxis ("Mouse X") * Time.deltaTime * AimedMoveAmount;
			
			MoveOnY = Input.GetAxis ("Mouse Y") * Time.deltaTime * AimedMoveAmount;
			
			NewGunPos = new Vector3 (DefaultPos.x + MoveOnX, DefaultPos.y + MoveOnY, DefaultPos.z);
			
			GUN.transform.localPosition = Vector3.Lerp (GUN.transform.localPosition, NewGunPos, MoveSpeed * Time.deltaTime);
		} else {

			MoveOnX = Input.GetAxis ("Mouse X") * Time.deltaTime * MoveAmount;

			MoveOnY = Input.GetAxis ("Mouse Y") * Time.deltaTime * MoveAmount;

			NewGunPos = new Vector3 (DefaultPos.x + MoveOnX, DefaultPos.y + MoveOnY, DefaultPos.z);

			GUN.transform.localPosition = Vector3.Lerp (GUN.transform.localPosition, NewGunPos, MoveSpeed * Time.deltaTime);
		}
    }
}
