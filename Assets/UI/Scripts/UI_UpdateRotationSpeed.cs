using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;

public class UI_UpdateRotationSpeed : MonoBehaviour {

	public SpinnerController spinnerController;

	private Text text;
	private Slider slider;

	// Use this for initialization
	void Start () {
		// Look for slider and text in children - these should be there
		slider = GetComponentInChildren<Slider>();
		text = GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Check if player has been initialized yet
		if (spinnerController == null) {
			return;
		}

		updateUI ();
	}

	// Update the UI based on Rotation Speed of the player
	private void updateUI(){
		text.text = "Speed: " + Mathf.Round(spinnerController.currentRotationSpeed).ToString();
		slider.value = spinnerController.currentRotationSpeed / spinnerController.initialRotationSpeed;
	}
		
}
