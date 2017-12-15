using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public static SceneLoader Instance{
		get;
		set;
	}

	void Awake(){
		DontDestroyOnLoad (gameObject);
		Instance = this;
	}

	public void LoadScene(string sceneName){
		SceneManager.LoadScene (sceneName);
	}

	// Exit out of the game
	public void ExitGame(){
		Application.Quit();
	}

}
