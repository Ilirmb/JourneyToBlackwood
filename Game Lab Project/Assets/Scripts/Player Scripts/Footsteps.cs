using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {

    public AudioClip SFX_Footsteps_Rocks_Snekers;
    public AudioClip SFX_Footsteps_Rocks_Sneaker_Jump_Landing;

    public AudioSource audioS;

	// Use this for initialization
	void Walking()
    {
        audioS.PlayOneShot(SFX_Footsteps_Rocks_Snekers);
	}

    void Jumping()
    {
        audioS.PlayOneShot(SFX_Footsteps_Rocks_Sneaker_Jump_Landing);
    }
}
