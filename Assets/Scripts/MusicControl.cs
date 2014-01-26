using UnityEngine;
using System.Collections;

public class MusicControl : MonoBehaviour {

	public int counter 					= 0; //current song
	public AudioClip[] myThemes;				//array of songs
	// Use this for initialization
	void Start () {
		audio.clip = myThemes[0];
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if(!audio.isPlaying) {
			counter++;
			if (counter > 3) {
				counter = 3;
			}
			audio.clip = myThemes[counter];
			audio.Play();
		}
	}
}

