using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpinnerController))]
[RequireComponent(typeof(Rigidbody))]
public class SpinnerCombat : MonoBehaviour {
	
	public float impactAngleMultiplier = 1.0f;
	public float differenceInMomentumMultiplier = 1.0f;

	private SpinnerController spinnerController;
	private Rigidbody rigidBody;

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
		Vector3 theirVelocity = collision.gameObject.GetComponent<SpinnerController>().velocityBeforeFixedUpdate;
		Vector3 myVelocity = spinnerController.velocityBeforeFixedUpdate;
		Vector3 collisionPoint = collision.contacts [0].point;
		Vector3 collisionDirection = collisionPoint - transform.position;
		// Calculate damage
		// Handle impact angle
		float impactAngle = Vector3.Angle(myVelocity, collisionDirection);
		impactAngle /= 180.0f;	// Normalize
		impactAngle *= impactAngleMultiplier;	// Apply modifier
		Debug.Log("Impact Angle: " + impactAngle);
		// Handle momentum
		Vector3 theirVelocityInCollisionDirection = Vector3.zero;
		if (Vector3.Angle (theirVelocity, -1.0f * collisionDirection) < 90.0f) {
			theirVelocityInCollisionDirection = Vector3.Project (theirVelocity, -1.0f * collisionDirection);
		}
		Vector3 myVelocityInCollisionDirection = Vector3.zero;
		if (Vector3.Angle (myVelocity, collisionDirection) < 90.0f) {
			myVelocityInCollisionDirection = Vector3.Project (myVelocity, collisionDirection);
		}
		Vector3 theirMomentumInCollisionDirection = theirVelocityInCollisionDirection * collision.rigidbody.mass;
		Vector3 myMomentumInCollisionDirection = myVelocityInCollisionDirection * rigidBody.mass;
		float momentumDifference = theirMomentumInCollisionDirection.magnitude - myMomentumInCollisionDirection.magnitude;
		momentumDifference *= differenceInMomentumMultiplier;
		momentumDifference = Mathf.Max (momentumDifference, 0f);
		Debug.Log ("Momentum difference: " + momentumDifference);

		// Apply damage
		// Weighted sum
		float rotationToLose = impactAngle*0.70f + momentumDifference*0.30f;
		spinnerController.currentRotationSpeed -= rotationToLose;
		Debug.Log ("Rotation to lose: " + rotationToLose);
	}

	// Apply the physics of spinner collision
	private void applyPhysics(Collision collision){
		Vector3 collisionPoint = collision.contacts [0].point;
		Vector3 collisionDirection = collisionPoint - transform.position;
		float oldMagnitude = rigidBody.velocity.magnitude;
		rigidBody.velocity = collisionDirection.normalized * oldMagnitude * -1.0f;
	}

}
