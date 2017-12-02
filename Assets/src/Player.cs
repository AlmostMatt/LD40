using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, Actor {
	// Ability enum
	private static int ATTACK = 1;
	// Constants
	private static float ROT_SPEED = 360f;
	private static float MAX_SPEED = 15f;
	private static float ACCEL = 40f;

	private ActionMap actionMap;
	private int level = 1;
	private float xp = 0f;

	public GameObject bulletObj;
	public RectTransform xpRectangle;

	void Start () {
		actionMap = new ActionMap(this);
		actionMap.add(ATTACK, new Ability(0.2f));
	}
	
	// Update is called once per frame
	void Update () {
		var rb = GetComponent<Rigidbody2D>();
		// Accelerate with up and down. (on local y axis)
		Vector2 desiredV = transform.right * Input.GetAxis("Vertical") * MAX_SPEED;
		rb.AddForce(ACCEL * (desiredV - rb.velocity));
		// Turn at a constant rate (on local z axis)
		transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * ROT_SPEED * Time.deltaTime);
		// Fire
		if (Input.GetButton ("Fire1")) {
			attack();
		}
		// Misc updates
		actionMap.update(Time.deltaTime);
		base.Update();
		var xpRectScale = xpRectangle.localScale;
		xpRectScale.x = xp;
		xpRectangle.localScale = xpRectScale;
		xp += 0.2f * Time.deltaTime;
		if (xp >= 1f) {
			xp -= 1f;
			level += 1;
		}
	}

	private void attack() {
		if (actionMap.ready(ATTACK)) {
			// gunAudio.PlayOneShot(shootSound, 0.5f);
			actionMap.use(ATTACK, null);
			var numshots = 1 + level;
			for (var i = 0; i < numshots; i++) {
				GameObject shot = Instantiate(bulletObj);
				shot.transform.position = transform.position;
				var rot = transform.localEulerAngles;
				rot.z = rot.z + 10 * (i - numshots/2f);
				shot.transform.localEulerAngles = rot;
				// shot.GetComponent<Entity>().lifetime = 0.8f;
				// shot.GetComponent<Projectile>().target = this;
			}
		}
	}

}
