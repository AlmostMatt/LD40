using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Steering, Actor {
	// Ability enum
	private static int ATTACK = 1;
	public static int XP_PER_LEVEL = 15;

	private ActionMap actionMap;
	private Weapon[] weapons;
	private int level = 1;
	private int xp = 0;
	private bool justLeveledUp = false;
	private List<DelayedFire> delayedAttacks = new List<DelayedFire>();
	private List<GameObject> gunObjects = new List<GameObject>();

	public GameObject bulletObj;
	public GameObject missileObj;
	public GameObject bulletGunObj;
	public GameObject missileGunObj;
	public GameObject levelUpObj;

	void Start () {

		// Constants that are relevant to Steering
		TURN_RATE = 360f;
		MAXV = 15f;
		ACCEL = 40f;
		turnAutomatically = false;

		actionMap = new ActionMap(this);
		actionMap.add(ATTACK, new Ability(0.3f));
		base.Start();
		updateWeapons(level);
	}
	
	// Update is called once per frame
	void Update () {
		// X is forward, and Y is left.
		moveInDirection(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		// 
		bool playedAttackSound = false;
		for (int i = delayedAttacks.Count - 1; i>= 0; i--) {
			var delayedAttack = delayedAttacks[i];
			delayedAttack.delay -= Time.deltaTime;
			var playerAngle = transform.localEulerAngles.z;
			if (delayedAttack.delay <= 0f) {
				if (!playedAttackSound) {
					AudioPlayer.PlaySound(AudioClip.FIRE);
				}
				playedAttackSound = true;
				spawnProjectile(delayedAttack.weaponType, delayedAttack.offset, playerAngle, delayedAttack.relativeAngle);
				delayedAttacks.RemoveAt(i);
			}
		}
		// Fire
		Vector2 mouse = getWorldMousePos();
		turnToward(mouse);
		if (Input.GetMouseButton(0)) {
			attack(mouse);
		}
		// Misc updates
		actionMap.update(Time.deltaTime);
		base.Update();
		//receiveExperience(0.2f * Time.deltaTime);

	}

	private void attack(Vector2 target) {
		if (actionMap.ready(ATTACK)) {
			AudioPlayer.PlaySound(AudioClip.FIRE);
			// gunAudio.PlayOneShot(shootSound, 0.5f);
			actionMap.use(ATTACK, null);
			foreach (var weapon in weapons) {
				var playerAngle = transform.localEulerAngles.z; // angleToward(target);
				var relativeAngle = weapon.relativeAngle + weapon.angleVariance * Random.Range(-0.5f, 0.5f);
				spawnProjectile(weapon.weaponType, weapon.offset, playerAngle, relativeAngle);
				for (int i=1; i < weapon.numBullets; i++){
					relativeAngle = weapon.relativeAngle + weapon.angleVariance * Random.Range(-0.5f, 0.5f);
					delayedAttacks.Add(new DelayedFire(weapon.weaponType, weapon.offset, relativeAngle, weapon.delayPerAttack * i));
				}
			}
		}
	}

	private void spawnProjectile(WeaponType weaponType, Vector2 offset, float offsetAngle, float relativeAngle) {
		GameObject objectToSpawn = weaponType == WeaponType.MISSILE ? missileObj : bulletObj;
		GameObject shot = Instantiate(objectToSpawn);
		shot.GetComponent<Projectile>().isMissile = weaponType == WeaponType.MISSILE;
		Vector3 rotatedOffset = Quaternion.AngleAxis(offsetAngle, Vector3.forward) * (Vector3) offset;
		shot.transform.position = transform.position + rotatedOffset ;
		shot.transform.localEulerAngles = new Vector3(0, 0, offsetAngle + relativeAngle);
	}

	public void receiveExperience(int amount) {
		xp += amount;
		if (xp >= XP_PER_LEVEL) {
			AudioPlayer.PlaySound(AudioClip.LEVELUP);
			xp -= XP_PER_LEVEL;
			level += 1;
			justLeveledUp = true;
			var levelUp = Instantiate(levelUpObj);
			var particleSystem = levelUp.GetComponent<ParticleSystem>();
			//levelUp.transform.parent = transform;
			//levelUp.transform.localPosition = new Vector3(0,0,2);
			particleSystem.transform.position = transform.position;
			Destroy(levelUp, particleSystem.main.duration);
			updateWeapons(level);
		}
	}

	public bool hasLeveledUp() {
		bool result = justLeveledUp;
		justLeveledUp = false;
		return result;
	}

	public float getExperience() {
		return xp / (float) XP_PER_LEVEL;
	}

	private Vector2 getWorldMousePos() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		return ray.origin;
	}

	private void updateWeapons(int level) {
		weapons = Weapon.getWeapons(level);
		foreach (var weaponObj in gunObjects) {
			Destroy(weaponObj);
		}
		gunObjects.Clear();
		foreach (var weapon in weapons) {
			GameObject weaponObj = Instantiate(weapon.weaponType == WeaponType.MISSILE ? missileGunObj : bulletGunObj);
			weaponObj.transform.parent = transform;
			weaponObj.transform.localPosition = weapon.offset;
			weaponObj.transform.localEulerAngles = new Vector3(0, 0, weapon.relativeAngle);
			gunObjects.Add(weaponObj);
		}
		// Put 'attack' on cooldown to prevent accidental fire as you level up
		actionMap.setCurrentCooldown(ATTACK, 0.3f);
		delayedAttacks.Clear();
	}

	private class DelayedFire {
		public WeaponType weaponType;
		public Vector2 offset;
		public float relativeAngle;
		public float delay;

		public DelayedFire(WeaponType weaponType, Vector2 offset, float relativeAngle, float delay) {
			this.weaponType = weaponType;
			this.offset = offset;
			this.relativeAngle = relativeAngle;
			this.delay = delay;
		}
	}
}
