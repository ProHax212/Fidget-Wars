using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class SpinnerController : NetworkBehaviour {

	public bool controlsActive = true;

	[Range(0, 45)]
	public float rotationAngleMax = 15.0f;
	public float initialRotationSpeed = 100.0f;
	public float maxSpeed = 1f;
	public float acceleration = 5.0f;
	public float naturalRotationDecceleration = 1.0f;
	public float handling = 5.0f;
	public float spinnerMass = 1.0f;
	public GameObject spinner;
	public Transform tiltAxis;
	public AnimationCurve rotationSpeedChangeCurve;

	[HideInInspector]
	[SyncVar]
	public float currentRotationSpeed;
	[HideInInspector]
	public float currentSpeed = 0f;
	[HideInInspector]
	public Vector3 currentDirection;
	[HideInInspector]
	public Vector3 velocityBeforeFixedUpdate;

	private Vector3 newDirection;
	private Vector3 currentRotationOffset;
	private Animator spinnerAnimator;
	private Rigidbody rigidBody;

	// Things to set when player is instantiated
	UI_UpdateRotationSpeed uiRotationSpeed;

	// Use this for initialization
	void Start () {
		currentRotationOffset = Vector3.zero;
		currentRotationSpeed = initialRotationSpeed;

		// Check for spinner
		if (spinner == null) {
			Debug.Log ("Must choose a spinner object for the player");
			return;
		}

		// Check for animator
		spinnerAnimator = spinner.GetComponent<Animator>();
		if (spinnerAnimator == null) {
			Debug.Log ("Spinner object needs an Animator");
		}

		// Check for tilt axis
		if (tiltAxis == null) {
			Debug.Log ("No tilt axis found");
		}
			
		rigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Happens regardless of local player
		updateRotation ();
		updateAnimation ();
		updateTilt ();

		// Check if local player
		if (!isLocalPlayer) {
			return;
		}

		// Check if controls are active
		if (controlsActive) {
			checkInput ();
		}

		move ();
	}
		
	private void checkInput(){
		movement ();
	}

	// Get user input for movement and move the spinner accordingly
	private void movement(){
		// Start at (0, 0, 0)
		newDirection = Vector3.zero;

		// Forward
		if (Input.GetKey (KeyCode.W)) {
			newDirection += transform.forward;
		} 
		// Left
		if (Input.GetKey (KeyCode.A)) {
			newDirection += -transform.right;
		} 
		// Back
		if (Input.GetKey (KeyCode.S)) {
			newDirection += -transform.forward;
		} 
		// Right
		if (Input.GetKey (KeyCode.D)) {
			newDirection += transform.right;
		}

		newDirection.Normalize ();
	}

	// Move the player based on the movement vector
	private void move(){
		Vector3 force = newDirection * acceleration;

		// Check max speed in x/z space
		Vector3 currentVelocity = rigidBody.velocity;
		currentVelocity.y = 0f;
		if (currentVelocity.magnitude < maxSpeed) {
			rigidBody.AddForce(force);
		}

		currentSpeed = currentVelocity.magnitude;
	}

	// Update the tilt
	private void updateTilt(){
		// Get tilt information
		Vector3 currentDirection = rigidBody.velocity;
		currentDirection.y = 0;
		// Don't apply tilt if no movement
		if (currentDirection == Vector3.zero) {
			return;
		}
		Quaternion newRotation = Quaternion.LookRotation (currentDirection, transform.up);
		currentRotationOffset = new Vector3(rotationAngleMax * (currentSpeed / maxSpeed), 0f, 0f);

		// Do the tilt
		tiltAxis.rotation = newRotation;
		tiltAxis.Rotate (currentRotationOffset);
	}

	// Update the rotation of the spinner
	public void updateRotation(){
		currentRotationSpeed -= naturalRotationDecceleration * Time.deltaTime;
	}

	// Update the speed of the animation based on rotation speed
	private void updateAnimation(){
		float ratio = currentRotationSpeed / initialRotationSpeed;
		float newAnimationSpeed = rotationSpeedChangeCurve.Evaluate (ratio);
		spinnerAnimator.speed = newAnimationSpeed;
	}

	// Player was instantiated
	public override void OnStartLocalPlayer(){
		if (!isLocalPlayer) {
			return;
		}

		// Make the camera follow the player
		SpinnerCamera spinnercam = Camera.main.GetComponent<SpinnerCamera>();
		spinnercam.spinnerTarget = transform;

		// Create player hook for UI updates
		var rotationSpeedObj = GameObject.Find("UI_RotationSpeed");
		rotationSpeedObj.GetComponent<UI_UpdateRotationSpeed> ().spinnerController = this;
	}

	// Used to update the velocity before physics takes place
	void FixedUpdate(){
		velocityBeforeFixedUpdate = rigidBody.velocity;
	}

}
