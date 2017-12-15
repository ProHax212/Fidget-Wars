using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;

public class FidgetNetworkManager : NetworkManager {

	public List<SpinnerNamePrefab> spinnerOptions;

	private Dictionary<string, GameObject> spinnerOptionsDict;

	// Used for grouping spinner names to their respective prefabs
	[System.Serializable]
	public struct SpinnerNamePrefab{
		public string SpinnerName;
		public GameObject SpinnerPrefab;

		public SpinnerNamePrefab(string name, GameObject prefab){
			SpinnerName = name;
			SpinnerPrefab = prefab;
		}
	}

	public static FidgetNetworkManager Instance{
		get;
		set;
	}

	void Awake(){
		
		DontDestroyOnLoad (gameObject);
		Instance = this;
	}

	void Start(){
		Debug.Assert (spinnerOptions != null);
		Instance.StartMatchMaker ();

		// Set up the spinnerOptionsDict
		spinnerOptionsDict = new Dictionary<string, GameObject>();
		foreach (SpinnerNamePrefab item in spinnerOptions) {
			spinnerOptionsDict [item.SpinnerName] = item.SpinnerPrefab;
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessage){
		Debug.Assert (extraMessage != null);

		var playerChoice = extraMessage.ReadMessage<StringMessage> ();
		GameObject player = (GameObject)Instantiate (spinnerOptionsDict[playerChoice.value], Vector3.zero, Quaternion.identity);
		NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
	}

	// Override so a new player is not created
	public override void OnClientSceneChanged (NetworkConnection conn)
	{
		//base.OnClientSceneChanged (conn);
	}

	public override void OnClientConnect (NetworkConnection conn)
	{
		//base.OnClientConnect (conn);
	}

}
