using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarolineAnimationEvents : MonoBehaviour {

	public AudioClip step1;
	public AudioClip step2;
	public float pitchRange;

	public bool isInCutscene;

	private AudioSource source;
	private PlayerMovementsAnima movementController;

	private void Awake() {
		source = gameObject.GetComponent<AudioSource>();
		movementController = gameObject.GetComponent<PlayerMovementsAnima>();
	}

	public void PlayHornSound() {
		SoundManager.instance.UseHorn();
	}

	public void PlayStepOne() {
		if(!isInCutscene && movementController.grounded) {
			source.pitch = 1 + Random.Range(-pitchRange, pitchRange);
			source.PlayOneShot(step1);
		}
	}

	public void PlayStepTwo() {
		if(!isInCutscene && movementController.grounded) {
			source.pitch = 1 + Random.Range(-pitchRange, pitchRange);
			source.PlayOneShot(step2);
		}
	}
}
