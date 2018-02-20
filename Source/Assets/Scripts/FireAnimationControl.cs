using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAnimationControl : MonoBehaviour {

	public ParticleSystem mouthFire;
	public ParticleSystem noseFire1;
	public ParticleSystem noseFire2;

	public void EmitMouthFire() {
		mouthFire.Play();
	}

	public void StopMouthFire() {
		mouthFire.Stop();
	}

	public void EmitNoseFire() {
		noseFire1.Play();
		noseFire2.Play();
	}

	public void StopNoseFire() {
		noseFire1.Stop();
		noseFire2.Stop();
	}
}
