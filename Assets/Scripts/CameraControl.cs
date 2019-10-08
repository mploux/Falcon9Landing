using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject follow;
	Vector3 posToFollow;
	void Start () {
	}

	void Update () {
		Vector3 vec = follow.transform.position;
		posToFollow = new Vector3 (vec.x, vec.y + 5, vec.z - 30);

		this.transform.position = posToFollow;			
	}
}