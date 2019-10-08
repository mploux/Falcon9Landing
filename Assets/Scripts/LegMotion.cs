using UnityEngine;
using System.Collections;

public class LegMotion : MonoBehaviour {

	public Rigidbody	body;
	public GameObject   landingPosition;
	public GameObject[] legs;

	private int angle;
	private int startAngle = -90;
	private int endAngle = 25;

	private bool extend = false;

	void Start () {
		angle = startAngle;
	}

	void Update() {

	}
 
	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.G)) {
			extend = !extend;
		}

		if (body.position.y - landingPosition.transform.position.y < -body.velocity.y + 10)
			extend = true;

		if (extend) {
			if (angle < endAngle) {
				angle++;
			}else {
				angle = endAngle;
			}
		} else {
			if (angle > startAngle) {
				angle--;
			}else {
				angle = startAngle;
			}
		}
		
		for (int i = 0; i < legs.Length; i++) {
			GameObject o = legs[i];
			Vector3 rot = o.transform.localRotation.eulerAngles;
			o.transform.localRotation = Quaternion.Euler(angle, rot.y, rot.z);
		}
	}
}