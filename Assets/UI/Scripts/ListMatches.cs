using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class ListMatches : MonoBehaviour {

	public RectTransform match_p;

	private List<RectTransform> currentMatches = new List<RectTransform> ();

	private void clearMatches(){
		foreach (var match in currentMatches) {
			Destroy (match.gameObject);
		}

		currentMatches.Clear ();
	}

	// Redraw the available matches to join
	private void updateMatchList(List<MatchInfoSnapshot> matches){
		// Loop through available matches
		foreach (var match in matches) {
			RectTransform matchObj = Instantiate (match_p, this.transform, false);
			matchObj.GetComponent<MatchOption> ().SetMatchName (match.name);
			matchObj.GetComponent<MatchOption> ().matchInfoSnapshot = match;

			currentMatches.Add (matchObj);
		}
	}

	// Refresh the match list for multiplayer
	public void RefreshMatches(){
		clearMatches ();

		var networkManager = FidgetNetworkManager.Instance;
		networkManager.matchMaker.ListMatches (0, 10, "", true, 0, 0, OnMatchList);
	}

	// Called when matches are retrieved
	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches){
		if (!success) {
			Debug.Log ("Failed to find matches");
			return;
		}

		if (matches == null) {
			return;
		}

		updateMatchList (matches);
	}

}
