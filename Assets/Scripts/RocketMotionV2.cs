using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RocketMotionV2 : MonoBehaviour {
	public Rigidbody body;
	public ParticleEmitter fireEmitter;
	public GameObject   landingPosition;
	public GameObject	velocityText;
	public GameObject	heightText;
	public GameObject	thrustText;
	public GameObject 	explosion;
	public Transform 	enginePosition;

	private float thrust;
	private bool landed = false;
	private bool explode = false;
	private bool igniteEngine = false;

	void Start ()
	{
		thrust = 0.0f;
		fireEmitter.maxEnergy = 0;
	}
		
	void FixedUpdate ()
	{


		Vector3 positionCorrection = (landingPosition.transform.position - body.position);
		Vector3 gimbal = transform.up - Vector3.up;

		//gimbal.x += -clamp(-1, 1, positionCorrection.x * 0.1f) * 0.001f;
		//gimbal.z += -clamp(-1, 1, positionCorrection.z * 0.1f) * 0.001f;


		float anticipation = Mathf.Pow(body.velocity.y, 2.0f);
		float thrustRatio = ((landingPosition.transform.position.y - body.transform.position.y) + anticipation);
		Debug.Log (thrustRatio);

		//float thrust = clamp (0.0f, 2.0f, thrustRatio);

		float thrust = -Physics.gravity.y * body.mass * 1;

		enginePosition.Rotate (gimbal.x, 0, gimbal.z);
		body.AddForceAtPosition (-enginePosition.forward * thrust, enginePosition.position);
		//body.mass -= thrust / (-Physics.gravity.y * body.mass) * 0.001f;
		fireEmitter.maxEnergy = thrust * 0.00001f;
		//fireEmitter.transform.LookAt (-thrust * gimbal);

		velocityText.GetComponent<Text> ().text = "Velocity: " + body.velocity.y;
		heightText.GetComponent<Text> ().text = "Height: " + body.position.y;
		thrustText.GetComponent<Text> ().text = "Thrust: " + -enginePosition.forward * thrust;
	}

	float	getFireHeight()
	{
		float result = 0;

		result = -body.velocity.y * 8.3f;
		return result;
	}

	void Update() {
		if (body.mass < 100)
			return;
		if (landed)
			return;
		/*if (explode) {
			explosion.transform.position = transform.position;
			Instantiate(explosion);
			Destroy(gameObject);
			return;
		}*/

		/*thrust = 0;
		if (body.transform.position.y <= getFireHeight () + landingPosition.transform.position.y)
			igniteEngine = true;
		if (igniteEngine)
			thrust = -Physics.gravity.y * body.mass * 1.0f;
		if (body.velocity.y > 0)
			landed = true;
		*/
		/*float thrustRatio = (200 - transform.position.y) - body.velocity.y * -Physics.gravity.y;
			
		Debug.Log (thrustRatio);
		float thrustFactor = clamp(0, 2f, (thrustRatio * 0.1f));
		thrust = -Physics.gravity.y * body.mass * abs(thrustFactor);*/


	


		/*fireEmitter.transform.LookAt(fireEmitter.transform.position + -inverse);
		*/

		Debug.DrawLine (enginePosition.position, enginePosition.position + enginePosition.forward);
	//	Debug.DrawLine (transform.position, body.position + velocityCorrection * 4);
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

	float zeroZone(float a, float min, float max)
	{
		if (a <= max && a >= min)
			a = 0;
		else if (a > max)
			a = a - max;
		else if (a < min)
			a = a + min;
		return a;
	}

	Vector3 abs(Vector3 v)
	{
		if (v.x >= 0)
			v.x = 1;
		if (v.x < 0)
			v.x = -1;

		if (v.y >= 0)
			v.y = 1;
		if (v.y < 0)
			v.y = -1;

		if (v.z >= 0)
			v.z = 1;
		if (v.z < 0)
			v.z = -1;

		return (v);
	}

	float abs(float v)
	{
		if (v < 0)
			v = -v;
		return v;
	}
}