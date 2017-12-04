using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
	private bool hasReleasedMouseButton = false;

	public void Update() {
		// If the mouse button is still being held (because of a gameover that just happened), wait for them to release it.
		if (!Input.GetMouseButton(0)) {
			hasReleasedMouseButton = true;
		}
		if (hasReleasedMouseButton && Input.GetMouseButton(0)) {
			SceneManager.LoadScene("mainScene", LoadSceneMode.Single);
		}
		if (Input.GetKey("escape")) {
			Application.Quit();
		}
	}
}

