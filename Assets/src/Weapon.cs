using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {GUN=1,MISSILE=2};

public class Weapon
{
	public WeaponType weaponType;
	public Vector2 offset;
	public float relativeAngle;

	public int numBullets;
	public float delayPerAttack;
	public float angleVariance;

	public Weapon (
			WeaponType weaponType,
			Vector2 offset,
			float relativeAngle,
			int numBullets = 1,
			float delayPerAttack = 0f,
			float angleVariance = 0f) {
		this.weaponType = weaponType;
		// I invert y and angle so that -y is on the left and positive rotation is clockwise
		this.offset = new Vector2(offset.x, -offset.y);
		this.relativeAngle = -relativeAngle;
		this.numBullets = numBullets;
		this.delayPerAttack = delayPerAttack;
		this.angleVariance = angleVariance;
	}

	public static Weapon[] getWeapons(int level) {

		switch (level) {
		case 1:
			// 2 bullets
			return new [] {
				new Weapon(WeaponType.GUN, new Vector2(0.2f, -0.5f), 0),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, 0.5f), 0),
			};
		case 2:
			// 2 very rapid guns forward
			return new [] {
				new Weapon(WeaponType.GUN, new Vector2(0.2f, -0.5f), 0, 6, 0.05f,10f),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, 0.5f), 0, 6, 0.05f,10f),
			};
		case 3:
			// 2 missiles
			return new [] {
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, -0.6f), 0),
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, 0.6f), 0),
			};
		case 4:
			// 4 bullet spread, semi-rapid
			return new [] {
				new Weapon(WeaponType.GUN, new Vector2(0f, -0.8f), -20, 2, 0.1f),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, -0.4f), -5, 2, 0.1f),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, 0.4f), 5, 2, 0.1f),
				new Weapon(WeaponType.GUN, new Vector2(0f, 0.8f), 20, 2, 0.1f),
			};
		case 5:
			// forward + back, semi-rapid
			return new [] {
				new Weapon(WeaponType.GUN, new Vector2(0.2f, -0.5f), 0, 2, 0.1f),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, 0.5f), 0, 2, 0.1f),
				new Weapon(WeaponType.GUN, new Vector2(-0.3f, -0.6f), 180, 2, 0.1f),
				new Weapon(WeaponType.GUN, new Vector2(-0.3f, 0.6f), 180, 2, 0.1f),
			};
		case 6:
			// missiles + rapid fire
			return new [] {
				new Weapon(WeaponType.MISSILE, new Vector2(0f, -0.8f), -20),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, -0.4f), 0, 2, 0.1f),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, 0.4f), 0, 2, 0.1f),
				new Weapon(WeaponType.MISSILE, new Vector2(0f, 0.8f), 20),
			};
		case 7:
			// rapid fire + missiles
			return new [] {
				new Weapon(WeaponType.GUN, new Vector2(0.1f, -0.8f), -30, 2, 0.1f),
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, -0.4f), -5),
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, 0.4f), 5),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, 0.8f), 30, 2, 0.1f),
			};
		case 8:
			// 6 guns, spread
			return new [] {
				new Weapon(WeaponType.GUN, new Vector2(0.1f, -0.9f), -30),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, -0.6f), -20),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, -0.3f), -10),
				new Weapon(WeaponType.GUN, new Vector2(0.3f, 0.0f), 0),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, 0.3f), 10),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, 0.6f), 20),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, 0.9f), 30),
			};
		case 9:
			// wider spread, some missiles.
			return new [] {
				new Weapon(WeaponType.MISSILE, new Vector2(0.1f, -0.8f), -50),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, -0.4f), -30),
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, -0.2f), -10),
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, 0.2f), 10),
				new Weapon(WeaponType.GUN, new Vector2(0.2f, 0.4f), 30),
				new Weapon(WeaponType.MISSILE, new Vector2(0.1f, 0.8f), 50),
			};
		case 10:
			// 4 orthogonal directions, but 2 missiles straight ahead
			return new [] {
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, -0.5f), 0),
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, 0.5f), 0),
				new Weapon(WeaponType.GUN, new Vector2(-0.2f, -1f), -90),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, 0.0f), 180),
				new Weapon(WeaponType.GUN, new Vector2(-0.2f, +1f), 90),
			};
		case 11:
			// same, but semi-rapid guns
			return new [] {
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, -0.5f), 0),
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, 0.5f), 0),
				new Weapon(WeaponType.GUN, new Vector2(-0.2f, -1f), -90, 2, 0.07f),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, 0.0f), 180, 3, 0.05f,8f),
				new Weapon(WeaponType.GUN, new Vector2(-0.2f, +1f), 90, 2, 0.07f),
			};
		case 12:
			// same but side missiles  NOTE: this is pretty tough
			return new [] {
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, -0.5f), 0),
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, 0.5f), 0),
				new Weapon(WeaponType.MISSILE, new Vector2(-0.2f, -0.9f), -90),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, 0.0f), 180, 3, 0.05f,8f),
				new Weapon(WeaponType.MISSILE, new Vector2(-0.2f, +0.9f), 90),
			};
		case 13:
			// same but also 2 semi-rapid angled guns
			return new [] {
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, -0.5f), 0),
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, 0.5f), 0),
				new Weapon(WeaponType.MISSILE, new Vector2(-0.2f, -0.9f), -90),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, 0.0f), 180, 3, 0.05f,8f),
				new Weapon(WeaponType.MISSILE, new Vector2(-0.2f, +0.9f), 90),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, -0.9f), -30, 2, 0.07f),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, 0.9f), 30, 2, 0.07f),
			};
		case 14:
		default:
			// same but 2 reversed gunswith a spread
			return new [] {
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, -0.5f), 0),
				new Weapon(WeaponType.MISSILE, new Vector2(0.2f, 0.5f), 0),
				new Weapon(WeaponType.MISSILE, new Vector2(-0.2f, -0.9f), -90),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, -0.1f), -170, 2, 0.08f,8f),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, 0.1f), 170, 2, 0.08f,8f),
				new Weapon(WeaponType.MISSILE, new Vector2(-0.2f, +0.9f), 90),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, -0.9f), -30, 2, 0.07f),
				new Weapon(WeaponType.GUN, new Vector2(0.1f, 0.9f), 30, 2, 0.07f),
			};
		/*
		default:
			var numshots = level;
			var weapons = new Weapon[numshots];
			for (var i = 0; i < numshots; i++) {
				float relativeAngle = 10 * (i - (numshots-1)/2f);
				Vector2 offset = new Vector2(0.3f, 0f);
				if (relativeAngle < 0f) {
					offset = new Vector2(0.2f, -0.6f);
				} else if (relativeAngle > 0f) {
					offset = new Vector2(0.2f, 0.6f);
				}
				weapons[i] = new Weapon(WeaponType.MISSILE, offset, relativeAngle);
			}
			return weapons;
		*/
		}
	}
}

