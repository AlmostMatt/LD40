using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Steering {

	public MonoBehaviour target;
	public GameObject explosionParticle;

	void Start () {
		MAXV = 20f;
		ACCEL = 10f;
		base.Start();
		var rb = GetComponent<Rigidbody2D>();
		rb.velocity = transform.right * MAXV;
	}

	void Update () {
		//var rb = GetComponent<Rigidbody2D>();
		//rb.velocity = transform.up * MAXV;
		if (target != null) {
			pursue(target);
		}
		base.Update();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		GameObject explosion = Instantiate(explosionParticle);
		explosion.transform.position = transform.position;
		Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
		Destroy(gameObject);
	}
}
