using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerController : MonoBehaviour {

	public List<SpinnerNamePrefab> spinnerOptions;

	private class MyMessage : MessageBase{

		public string message;

		public MyMessage(string message){
			this.message = message;
		}

		public override void Deserialize (NetworkReader reader)
		{
			message = reader.ReadString ();
		}

		public override void Serialize (NetworkWriter writer)
		{
			writer.Write (message);
		}
	}

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
	
	void Start(){
		Debug.Assert (spinnerOptions != null);
	}

	// Called when the player choosed a spinner for the match
	public void ChoosePlayer(string playerChoice){
		bool result = ClientScene.AddPlayer (FidgetNetworkManager.Instance.client.connection, 0, new MyMessage(playerChoice));
		if (!result) {
			Debug.LogError ("Error calling AddPlayer for client");
			return;
		}
	}

}
