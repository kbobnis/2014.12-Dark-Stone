using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour{

	public static AudioClip Ambient;

	private static bool SoundsEnabled;

	static SoundManager(){

		Ambient = Resources.Load<AudioClip> ("Sounds/muzyczka");
		Game.Me.GetComponent<AudioSource>().enabled = PlayerPrefs.GetInt("MusicOn", 1)==1;
		SoundsEnabled = PlayerPrefs.GetInt("SoundsOn", 1) == 1;
	}

	public static void EnableMusic(bool on){
		Game.Me.GetComponent<AudioSource>().enabled = on;
		PlayerPrefs.SetInt("MusicOn", on ? 1 : 0);
	}

	public static bool IsMusicEnabled() {
		return Game.Me.GetComponent<AudioSource>().enabled;
	}

	public static void EnableSounds(bool on){
		SoundsEnabled = on;
		PlayerPrefs.SetInt("SoundsOn", on ? 1 : 0);
	}

	public static bool AreSoundsEnabled() {
		return SoundsEnabled;
	}
}
