using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

	public GameObject scene;

	public void damage(int amount) {
		scene.GetComponent<Scene>().damagePlanet(amount);
	}

	public void Update() {
		transform.localEulerAngles = new Vector3(0,0,15*Time.deltaTime + transform.localEulerAngles.z);
	}
}
