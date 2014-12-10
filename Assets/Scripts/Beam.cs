using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour {
	public float speed = 100f;
	
	// Use this for initialization
	void Start () {
		rigidbody.velocity = transform.forward * speed;
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag.Equals("Boundary")) {
			// add a delay here so that it doesn't get destroyed in the same
			// frame as it does its thing
			Destroy(gameObject, 1f);	
		}
	}
}
