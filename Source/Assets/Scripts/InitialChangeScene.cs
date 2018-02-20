using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialChangeScene : MonoBehaviour {

    public float timeToWait;
    public string sceneToLoad;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(ChangeScene());
	}

	IEnumerator ChangeScene()
	{
		yield return new WaitForSeconds(timeToWait);
		SceneManager.LoadScene(sceneToLoad);
	}
}
