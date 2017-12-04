using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Steering, Actor {
	// Ability enum
	private static int ATTACK = 1;

	private ActionMap actionMap;

	private static float DIST_PER_WAYPOINT = 2f;
	private Vector3 wayPoint;
	private bool hasWayPoint = false;
	public GameObject target;
	private int health = 1;

	public void Start () {
		MAXV = 4f;
		ACCEL = 6f;
		base.Start();

		actionMap = new ActionMap(this);
		actionMap.add(ATTACK, new Ability(1.6f));
	}

	public void FixedUpdate () {
		//var rb = GetComponent<Rigidbody2D>();
		//rb.velocity = transform.up * MAXV;
		if (target != null) {
			var delta = target.transform.position - transform.position;
			var dist = delta.magnitude;
			if(!hasWayPoint && dist > (2 + DIST_PER_WAYPOINT)) {
				var randomRotation = Quaternion.AngleAxis(Random.Range(-35f,35f), Vector3.forward);
				wayPoint = transform.position + DIST_PER_WAYPOINT * (randomRotation * delta.normalized);
				hasWayPoint = true;
			}
			if (hasWayPoint) {
				seek(wayPoint);
				// Check if the alien is as close to the destination as the waypoint is.
				if ((target.transform.position - wayPoint).magnitude + 0.1f >= dist) {
					hasWayPoint = false;
				}
			}
			if (!hasWayPoint && dist < 2) {
				if (actionMap.ready(ATTACK)) {
					actionMap.use(ATTACK, null);
					target.GetComponent<Planet>().damage(1);
				}
				flee(target);
			} else {
				seek(target);
			}
		}
		actionMap.update(Time.fixedDeltaTime);
		base.FixedUpdate();
	}

	public void damage(int amount) {
		health -= amount;
		if (health <= amount) {
			AudioPlayer.PlaySound(AudioClip.ENEMY_KILL);
			Destroy(gameObject);
		}
	}
}
