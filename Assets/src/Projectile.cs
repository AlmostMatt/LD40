using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Steering {

	public GameObject target;
	public GameObject explosionParticle;
	public bool isMissile = false;

	private bool collided = false;

	public void Start () {
		MAXV = 20f;
		ACCEL = 100f;
		base.Start();
		var rb = GetComponent<Rigidbody2D>();
		rb.velocity = transform.right * MAXV;

		lifetime = 3f;
	}

	public void Update () {
		//var rb = GetComponent<Rigidbody2D>();
		//rb.velocity = transform.up * MAXV;
		if (collided) {
			GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
		} else if (target != null) {
			pursue(target);
		}
		base.Update();
	}

	public void OnCollisionEnter2D(Collision2D collision) {
		// Create an explosion at the point of collision
		GameObject explosion = Instantiate(explosionParticle);
		explosion.transform.position = collision.contacts[0].point;
		Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
		// Damage the other object
		//AudioPlayer.PlaySound(isMissile ? AudioClip.MISSILE_HIT : AudioClip.LASER_HIT);
		int dmgAmount = 1;
		if (collision.gameObject.GetComponent<Enemy>() != null) {
			collision.gameObject.GetComponent<Enemy>().damage(dmgAmount);
		}
		if (collision.gameObject.GetComponent<Planet>() != null) {
			collision.gameObject.GetComponent<Planet>().damage(dmgAmount);
		}
		// Disable most properties of this object and destroy it after a short delay (to let the trail fade out)
		collided = true;
		GetComponent<BoxCollider2D>().enabled = false;
		if (GetComponent<PolygonCollider2D>() != null) {
			GetComponent<PolygonCollider2D>().enabled = false;
		}
		transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
		Destroy(gameObject,1);
	}

	public void OnTriggerEnter2D(Collider2D other) {
		if (target == null) {
			target = other.gameObject;
		}
	}
}
