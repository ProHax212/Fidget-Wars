using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerCamera : MonoBehaviour {

	public Transform spinnerTarget;
	public Vector3 cameraOffset = new Vector3(0, 5.0f, -5.0f);

	public float cameraMoveSpeed = 5.0f;

	private float lastX;
	private float lastY;

	private float yRotationOffset = 0f;	// Used to maintain y offset of camera

	// Use this for initialization
	void Start () {
		// Check for spinner target
		if (spinnerTarget == null) {
			Debug.Log ("No spinner target given to SpinnerCamear script");
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Check if spinner target has been instantiated yet
		if (spinnerTarget == null) {
			return;
		}

		getInput ();
		updateFollowTarget ();
		updateCamera ();
	}

	// Get the movement of the mouse to move the camera
	private void getInput(){
		lastX = Input.GetAxis ("Mouse X");
		lastY = Input.GetAxis ("Mouse Y");
	}

	// Rotate the target if the X value changed
	private void updateFollowTarget(){
		spinnerTarget.Rotate (spinnerTarget.up, lastX * cameraMoveSpeed * Time.deltaTime);
	}

	// Update the camera based on the mouse x/y
	private void updateCamera(){
		yRotationOffset += lastY * cameraMoveSpeed * Time.deltaTime;

		// Update camera
		transform.position = spinnerTarget.TransformPoint(cameraOffset);
		transform.LookAt (spinnerTarget.transform);
		transform.RotateAround (spinnerTarget.position, spinnerTarget.right, yRotationOffset);
	}
}
