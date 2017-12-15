using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SparkOnContact : MonoBehaviour {

	public ParticleSystem sparkParticle;

	// Use this for initialization
	void Start () {
		// Check for the spark
		if (sparkParticle == null) {
			Debug.Log ("Particle system not found for spark");
			return;
		}
	}

	// Collision detected
	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.layer == LayerMask.NameToLayer ("Spinner")) {
			Rigidbody rigidBody = GetComponent<Rigidbody> ();

			Vector3 contactPoint = collision.contacts [0].point;

			// Create the spark
			var particle = Instantiate(sparkParticle, contactPoint, Quaternion.identity);
			particle.Play ();
			Destroy (particle.gameObject, particle.main.duration);
		}
	}

}
