using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTransitions : MonoBehaviour {

	public void LoadEndingCutscene_2() {
		GameManager.instance.changeScene("GameEnd2",null);
	}

	public void LoadEndingCutscene_3() {
		GameManager.instance.changeScene("GameEnd3",null);
	}

	public void LoadEndingCutscene_4() {
		GameManager.instance.changeScene("GameEnd4",null);
	}
	
	// true end of the game
	public void LoadCredits() {
		GameManager.instance.changeScene("GameEndCredits",null);
	}
}
