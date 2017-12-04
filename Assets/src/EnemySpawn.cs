using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn
{
	public float distance;
	public float angleOffset;
	public float delay;

	public EnemySpawn (
		float distance = 25f,
		float angleOffset = 0f,
		float delay = 0f) {
		this.distance = distance;
		this.angleOffset = angleOffset;
		this.delay = delay;
	}

	public static EnemySpawn[] getWave(int waveNumber) {
		switch (waveNumber) {
		case 1:
			// 3 groups of 5
			int nextIndex = 0;
			var spawns = new EnemySpawn[15];
			for (int i=0; i<3; i++) {
				var groupAngle = Random.Range(0f,360f);
				var groupDist = Random.Range(25f, 30f);
				for (int j=0;j<5;j++) {
					spawns[nextIndex++] = new EnemySpawn(groupDist, groupAngle + 6*j, 2f + (5f) * i);
				}
			}
			return spawns;
		case 2:
		default:
			// 6 groups. Alternating size 2 and 3. Total 15.
			nextIndex = 0;
			spawns = new EnemySpawn[15];
			for (int i=0; i<6; i++) {
				var groupAngle = Random.Range(0f,360f);
				var groupDist = Random.Range(25f, 30f);
				for (int j=0; j < 2 + (i%2); j++) {
					spawns[nextIndex++] = new EnemySpawn(groupDist+Random.Range(0f,2f), groupAngle + 6*j, (2f)*i);
				}
			}
			return spawns;
		}
	}

	/*
	 * 
			// 2 groups, 3s delay
	return new [] {
		new EnemySpawn(25f, 0f, 0f),
		new EnemySpawn(27f, 4f, 0f),
		new EnemySpawn(29f, 8f, 0f),
		new EnemySpawn(28f, 120f, 3f),
		new EnemySpawn(28f, 126f, 3f),
		new EnemySpawn(28f, 132f, 3f),
	};
	*/
}

