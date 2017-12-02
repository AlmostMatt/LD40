using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Steering {

	public MonoBehaviour target;

	void Start () {
		MAXV = 5f;
		ACCEL = 5f;
		base.Start();
		var rb = GetComponent<Rigidbody2D>();
		rb.velocity = transform.right * MAXV;
	}

	void Update () {
		//var rb = GetComponent<Rigidbody2D>();
		//rb.velocity = transform.up * MAXV;
		if (target != null) {
			var dist = (target.transform.position - transform.position).magnitude;
			if (dist < 10) {
				evade(target);
			} else {
				pursue(target);
			}
		}
		base.Update();
	}
}
