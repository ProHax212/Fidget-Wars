using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpinnerController))]
[RequireComponent(typeof(Rigidbody))]
public class SpinnerCombat : MonoBehaviour {

	public float hittingForceMultipier = 1.0f;
	public float differenceInSpeedMultiplier = 1.0f;

	private SpinnerController spinnerController;
	private Rigidbody rigidBody;

	// Directions for collisions
	private Vector3 myVelocity;
	private Vector3 theirVelocity;

	// Use this for initialization
	void Start () {
		// Get the spinner controller object
		spinnerController = GetComponent<SpinnerController>();

		// Get the rigidbody
		rigidBody = GetComponent<Rigidbody>();
	}

	// Called when spinner hits something
	void OnCollisionEnter(Collision collision){
		// Check if it hit another spinner
		if (collision.gameObject.layer == LayerMask.NameToLayer("Spinner")) {
			damageCalculation (collision);
			//applyPhysics (collision);
		}
	}

	// Do damage calculations for this spinner object
	private void damageCalculation(Collision collision){
		// Get initial data
		myVelocity = rigidBody.velocity;
		theirVelocity = collision.gameObject.GetComponent<Rigidbody> ().velocity;

		// Calculate damage

		// Apply damage
	}

	// Apply the physics of spinner collision
	private void applyPhysics(Collision collision){
		// Calculate force
		Vector3 newDirection = theirVelocity - myVelocity;

		// Apply force
		rigidBody.AddForce(newDirection * hittingForceMultipier);
	}

}
