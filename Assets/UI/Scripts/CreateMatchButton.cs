using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class CreateMatchButton : MonoBehaviour {
	
	public InputField inputName;
	public string multiplayerSceneName = "multiplayer";

	private MatchInfo matchToJoin;

	// Create a new internet match for others to join
	public void CreateInternetMatch(){
		string matchName = inputName.text;

		// No name given
		if (matchName == "") {
			return;
		}

		var networkManager = FidgetNetworkManager.Instance;
		networkManager.matchMaker.CreateMatch (matchName, 4, true, "", "", "", 0, 0, OnMatchCreate);
	}

	// Callback when the match has been created
	public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo){
		if (!success) {
			Debug.Log ("Failed to create match: " + extendedInfo);
			return;
		}

		matchToJoin = matchInfo;

		// Host the match
		var networkManager = FidgetNetworkManager.Instance;
		NetworkServer.Listen (matchInfo, 9000);
		networkManager.StartHost (matchInfo);

		// Change the scene
		networkManager.ServerChangeScene("multiplayer");
	}

}
