using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Scene : MonoBehaviour {
	// Prefabs
	public GameObject playerObject;
	public GameObject enemyObject;
	public GameObject indicatorObject;
	public GameObject planetExplosionObject;
	// Actual object instances
	public GameObject camera;
	public GameObject uiCanvas;
	public RectTransform xpRectangle;
	public RectTransform hpRectangle;
	public RectTransform recentDamageRectangle;
	public RectTransform levelUpText; 

	public List<GameObject> planets = new List<GameObject>();

	private List<GameObject> enemies = new List<GameObject>();
	private List<GameObject> indicators = new List<GameObject>();
	private int planetHealth;
	private float timeSinceGameEnded = 0f;
	private float recentDamage = 0f;
	private int PLANET_MAX_HEALTH = 10;
	private Vector2 levelUpPosition = new Vector2();
	private float levelUpAlpha = 0f;
	private int waveNumber = 0;
	private int enemiesRemaining = 0;
	private List<DelayedSpawn> delayedSpawns = new List<DelayedSpawn>();

	private GameObject player;
	void Start () {
		planetHealth = PLANET_MAX_HEALTH;
		player = Instantiate(playerObject);
		camera.GetComponent<CameraLogic>().player = player;
	}

	void Update () {
		updateEnemies();
		updateUI();
		if (planetHealth <= 0) {
			timeSinceGameEnded += Time.deltaTime;
			if (timeSinceGameEnded > 3.5f) {
				SceneManager.LoadScene("splashScene", LoadSceneMode.Single);
			}
		}
		if (Input.GetKey("escape")) {
			Application.Quit();
		}
	}

	private void updateUI() {
		xpRectangle.localScale = new Vector3(player.GetComponent<Player>().getExperience(), 1, 1);
		hpRectangle.localScale = new Vector3(planetHealth / (float) PLANET_MAX_HEALTH, 1, 1);

		recentDamage = Mathf.Max(0f, recentDamage - 4f * Time.deltaTime);
		recentDamageRectangle.localScale = new Vector3(recentDamage / (float) PLANET_MAX_HEALTH, 1, 1);
		var recentDamagePos = recentDamageRectangle.localPosition;
		recentDamagePos.x = hpRectangle.localPosition.x + hpRectangle.rect.width * hpRectangle.localScale.x;
		recentDamageRectangle.localPosition = recentDamagePos;

		levelUpAlpha = Mathf.Max(0f, levelUpAlpha - Time.deltaTime);
		if (player.GetComponent<Player>().hasLeveledUp()) {
			levelUpPosition = player.transform.position;
			levelUpPosition.y += 2;
			levelUpAlpha = 1f;
		}
		levelUpText.transform.position = Camera.main.WorldToScreenPoint(levelUpPosition);
		Color c = levelUpText.GetComponent<Text>().color;
		c.a = levelUpAlpha;
		levelUpText.GetComponent<Text>().color = c;
	}

	private void updateEnemies() {
		// Check for dead enemies
		for (int i = enemies.Count - 1; i >= 0; --i) {
			if (enemies[i]== null) {
				enemies.RemoveAt(i);
				player.GetComponent<Player>().receiveExperience(1);
				enemiesRemaining--;
			}
		}
		// Spawn new enemies
		if (enemiesRemaining == 0 && planetHealth > 0) {
			waveNumber++;
			EnemySpawn[] enemySpawns = EnemySpawn.getWave(waveNumber);
			enemiesRemaining = enemySpawns.Length;
			var randomAngle = Random.Range(0,360);
			foreach (EnemySpawn enemySpawn in enemySpawns) {
				if (enemySpawn.delay == 0f) {
					spawnEnemy(enemySpawn.distance, randomAngle + enemySpawn.angleOffset);
				} else {
					delayedSpawns.Add(new DelayedSpawn(enemySpawn.distance, randomAngle + enemySpawn.angleOffset, enemySpawn.delay));
				}
			}
		}
		// Spawn delayed enemies
		for (int i = delayedSpawns.Count - 1; i>= 0; i--) {
			var delayedSpawn = delayedSpawns[i];
			delayedSpawn.delay -= Time.deltaTime;
			if (delayedSpawn.delay <= 0f) {
				spawnEnemy(delayedSpawn.distance, delayedSpawn.angle);
				delayedSpawns.RemoveAt(i);
			}
		}
		// Update off-screen enemy indicators
		Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
		for (int i=0; i< indicators.Count; i++) {
			var indicator = indicators[i];
			if (i >= enemies.Count) {
				indicator.SetActive(false);
				continue;
			}
			var enemy = enemies[i];
			var borderOffset = 30;
			Vector2 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
			if (screenPos.x < 0 || screenPos.x > Screen.width 
				|| screenPos.y < 0 || screenPos.y > Screen.height) {
				indicator.SetActive(true);
				screenPos -= screenCenter;
				var absScreenPos = new Vector2(Mathf.Abs(screenPos.x), Mathf.Abs(screenPos.y));
				// compare y:x ratio of the screenPos vector to the y:x ratio of the screen
				// If it is larger, top of screen. otherwise, side of screen.
				if ((absScreenPos.y / absScreenPos.x) > (screenCenter.y / screenCenter.x)) {
					screenPos = ((screenCenter.y - borderOffset ) / absScreenPos.y) * screenPos;
				} else {
					screenPos = ((screenCenter.x - borderOffset ) / absScreenPos.x) * screenPos;
				}
				// The indicators are children of the canvas, so they use coordinates relative to the screen center
				indicator.transform.localPosition = screenPos;
				float angle = Mathf.Rad2Deg * Mathf.Atan2(screenPos.y, screenPos.x);
				indicator.transform.localEulerAngles = new Vector3(0, 0, angle);
			} else {
				indicator.SetActive(false);
			}
		}
	}

	public void damagePlanet(int amount) {
		if (planetHealth <= 0) {
			return;
		}
		recentDamage += Mathf.Min(amount, planetHealth);
		planetHealth -= Mathf.Min(amount, planetHealth);
		if (planetHealth <= 0) {
			AudioPlayer.PlaySound(AudioClip.PLANET_KILL);
			foreach (var planet in planets) {
				// TODO: show game-over text, reload the scene, or quit or something
				var explosion = Instantiate(planetExplosionObject);
				explosion.transform.position = planet.transform.position;
				Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
				Destroy(planet, 1);
			}
			camera.GetComponent<CameraLogic>().gameOver();
			delayedSpawns.Clear();
		} else { 
			AudioPlayer.PlaySound(AudioClip.PLANET_DAMAGE);
		}
	}

	private void spawnEnemy(float distance, float angle) {
		angle *= Mathf.Deg2Rad;
		var enemy = Instantiate(enemyObject);
		enemy.transform.position = distance * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		enemies.Add(enemy);
		if (enemies.Count > indicators.Count) {
			GameObject indicator = Instantiate(indicatorObject);
			indicator.transform.SetParent(uiCanvas.transform);
			indicators.Add(indicator);
		}
		var nearestPlanet = planets[0];
		if ((planets[1].transform.position - enemy.transform.position).sqrMagnitude
			< (nearestPlanet.transform.position - enemy.transform.position).sqrMagnitude) {
			nearestPlanet = planets[1];
		}
		enemy.GetComponent<Enemy>().target = nearestPlanet;
	}

	private class DelayedSpawn {
		public float distance;
		public float angle;
		public float delay;

		public DelayedSpawn(float distance, float angle, float delay) {
			this.distance = distance;
			this.angle = angle;
			this.delay = delay;
		}
	}
}
