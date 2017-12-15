using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;

public class MatchOption : MonoBehaviour {

	public Text matchName;
	public MatchInfoSnapshot matchInfoSnapshot;

	public void SetMatchName(string name){
		if (name == "") {
			return;
		}

		matchName.text = name;
	}

	// Join this match
	public void JoinMatch(){
		if (matchInfoSnapshot == null) {
			Debug.LogError ("No matchInfoSnapshot found, please initialize the variable first");
			return;
		}
		Debug.Log ("Joining: " + matchName.text);

		var networkManager = FidgetNetworkManager.Instance;
		networkManager.matchMaker.JoinMatch (matchInfoSnapshot.networkId, "", "", "", 0, 0, OnJoinMatch);
	}

	// Callback for when the match is joined
	public void OnJoinMatch(bool success, string extendedInfo, MatchInfo matchInfo){
		if (!success) {
			Debug.LogError ("Error joining match");
			return;
		}

		// Start the client
		var networkManager = FidgetNetworkManager.Instance;
		networkManager.StartClient (matchInfo);
	}

}
