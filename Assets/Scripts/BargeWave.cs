using UnityEngine;
using System.Collections;

public class BargeWave : MonoBehaviour {

	float xx, yy, zz;
	Vector3 startPos;
	float time;
	public float startTime;
	public float amnt;

	void Start () {
		xx = 0;
		yy = 0;
		zz = 0;
		startPos = transform.position;
		time = startTime;
	}

	void Update () {
		time += Time.deltaTime;
		xx = Mathf.Sin (time * 2f) * 0.001f;
		yy = Mathf.Sin (time * 2f) * 0.01f;
		zz = Mathf.Sin (time * 2f) * 0.001f;

		Debug.Log (xx + " " + yy + " " + zz);

		transform.Translate(xx, yy, zz);
	}
}
