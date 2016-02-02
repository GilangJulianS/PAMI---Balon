using UnityEngine;
using System.Collections;

public class Float : MonoBehaviour {

	private Rigidbody rb;
	public float forceConstant;
	public float maxSpeed;
	public float velocity;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		float multiplier = transform.localScale.x;
		float force = 0;
		if (transform.localScale.x > 0.25) {
			force = multiplier * forceConstant;
		}
		if (rb.velocity.y < maxSpeed) {
			rb.AddForce (new Vector3 (0, force, 0));
		}
		velocity = rb.velocity.y;
	}
}
