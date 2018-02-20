using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {

	// Called by animator
	public void NextScene () {
		GameManager.instance.IntroSceneFinished();
		
	}
}
