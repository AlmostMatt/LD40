using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : Steering {
	public GameObject player;
	private bool gameEnded = false;

	void Start () {
		turnAutomatically = false;
		MAXV = 80f;
		ACCEL = 150f;
		base.Start();
	}

	void Update () {
		var Damping = 5.0f;
		if (gameEnded) {
			// return to the origin
			transform.position = Vector3.Lerp(transform.position, new Vector3(0,0,-10), Time.deltaTime * Damping);
		} else  if (player != null) {
			var cameraOffset = getWorldMousePos() - (Vector2) player.transform.position;
			if (cameraOffset.magnitude > 5) {
				cameraOffset = 5 * cameraOffset.normalized;
			}
			//MAXV = Mathf.Max(30f, 20f * cameraOffset.magnitude);
			//arrival(player.transform.position + (Vector3) cameraOffset);
			//transform.position = player.transform.position + new Vector3(cameraOffset.x, cameraOffset.y, -10);
			var desiredPosition = player.transform.position + new Vector3(cameraOffset.x, cameraOffset.y, -10);
			transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * Damping);
		} 
	}

	public void gameOver() {
		gameEnded = true;
	}

	private Vector2 getWorldMousePos() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		return ray.origin;
	}
}
