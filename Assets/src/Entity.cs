using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
	private static float WORLD_W = 15f;
	private static float WORLD_H = 10f;
	// If lifetime is initially 0, it will do nothing. Otherwise it will decrease over time
	// and the object will be destroyed once it reaches 0.
	public float lifetime = 0f;

	public void Update () {
		//worldwrap();
		if (lifetime > 0f) {
			lifetime -= Time.deltaTime;
			if (lifetime <= 0f) {
				Destroy(gameObject);
			}
		}
	}

	private void worldwrap() {
		var pos = transform.position;
		if (pos.x > WORLD_W) { pos.x -= 2 * WORLD_W; }
		if (pos.x < -WORLD_W) { pos.x += 2 * WORLD_W; }
		if (pos.y > WORLD_H) { pos.y -= 2 * WORLD_H; }
		if (pos.y < -WORLD_H) { pos.y += 2 * WORLD_H; }
		transform.position = pos;
	}
}
