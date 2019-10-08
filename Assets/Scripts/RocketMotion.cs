using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RocketMotion : MonoBehaviour {
	public Rigidbody body;
	public ParticleEmitter fireEmitter;
	public GameObject   landingPosition;
	public GameObject	velocityText;
	public GameObject	heightText;
	public GameObject 	explosion;
	public Transform 	enginePosition;

	private const int LANDING_MODE = 1;
	private const int HOVER_MODE = 2;

	private int mode = 1;

	private float thrust;
	private float angle;
	private float landingHeight;
	private bool landed = false;
	private bool explode = false;
	private float maxThrust = 0;

	void Start ()
	{
		thrust = 0.0f;
		angle = 0;
		landingHeight = 0;
	}

	void FixedUpdate () 
	{
		if (Input.GetKey(KeyCode.LeftShift)) {
			thrust += 0.1f;
		}
		if (Input.GetKey(KeyCode.LeftControl)) {
			thrust -= 0.1f;
		}
		
		if (Input.GetKey (KeyCode.LeftArrow)) {
			angle += 0.01f;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			angle -= 0.01f;
		}
		
		fireEmitter.maxEnergy = thrust * 0.03f;
	}

	void Update() 
	{
		if (explode) {
			explosion.transform.position = transform.position;
			Instantiate(explosion);
			Destroy(gameObject);
			return;
		}

		float height = body.position.y;
		float heightDiff = abs(landingPosition.transform.position.y - height);
		Vector3 balanceVector = transform.up - Vector3.up;
		Vector3 airResistance = (transform.up - Vector3.up) * body.velocity.y;
		Vector3 landingPosDiff = landingPosition.transform.position - body.position;
		float steerFactor = 2f / clamp(20, heightDiff, heightDiff*0.5f);
		Text vel = velocityText.GetComponent<Text> ();
		vel.text = "Velocity: " + body.velocity.magnitude;
		
		Text h = heightText.GetComponent<Text>();
		h.text = "Height: " + heightDiff;

		if (heightDiff < 10)
			steerFactor /= 10.0f / heightDiff;
		balanceVector.x += body.velocity.x * 0.4f - landingPosDiff.x * steerFactor;
		balanceVector.z += body.velocity.z * 0.4f - landingPosDiff.z * steerFactor;
		if (mode == LANDING_MODE) {
			thrust = 0;
			if (body.position.y < -body.velocity.y * 10f) //15
				if (maxThrust < 12)
					maxThrust += Time.deltaTime * 10.0f;

			thrust = clamp (0, maxThrust, -body.velocity.y);
			if (body.velocity.y > -0.1f)
				landed = true;

			if (heightDiff < 10)
				thrust += 3f;

			if (heightDiff < 3)
				thrust += 3f;
			
			if (heightDiff < 1f)
				thrust += 1f;

			if (landed) {
				thrust = 0;
				return;
			}
		} else if (mode == HOVER_MODE) {
			thrust = -body.velocity.y * 10 + 5;
		}

		if (thrust > 0) { 
			body.transform.Rotate (balanceVector.x, 0, balanceVector.z);
			body.AddForceAtPosition (body.transform.up * thrust, enginePosition.position);
		}
	}

	void OnCollisionEnter (Collision col)
	{
		Debug.Log ("COLLISION ! " + body.velocity.magnitude);
		if (body.velocity.magnitude > 3)
			explode = true;
	}

	void onGui()
	{
		GUI.Label (new Rect (5, 5, 100, 50), "TEST");
	}

	float clamp(float a, float b, float v)
	{
		if (v < a)
			v = a;
		if (v > b)
			v = b;
		return v;
	}

	float abs(float v)
	{
		if (v < 0)
			v = -v;
		return v;
	}
}