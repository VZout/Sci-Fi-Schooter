using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

	public Vector3 rotateAmount;
	
	void Update () {
		transform.Rotate (rotateAmount * Time.deltaTime);
	}
}
