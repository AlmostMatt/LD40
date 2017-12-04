using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClip {LEVELUP=0, FIRE=1, ENEMY_KILL=2, PLANET_KILL=3, LASER_HIT=4, MISSILE_HIT=5, PLANET_DAMAGE=6};

public class AudioPlayer : MonoBehaviour {
	private static AudioPlayer instance;

	void Start () {
		instance = this;
	}

	public static void PlaySound(AudioClip index) {
		instance.GetComponents<AudioSource>()[(int) index].Play();
	}
}
