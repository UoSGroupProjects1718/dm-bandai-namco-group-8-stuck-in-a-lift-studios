using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public AudioClip weaponHitShield;
	public AudioClip weaponHit;
	public AudioClip weaponFire;
	public AudioClip[] pickupSoundEffects;
	public AudioSource audioSourceShield;
	public AudioSource audioSourceHit;
	public Color playerColour;
	public float movementSpeed;
	public float projectileSpeed;
	public float weaponCooldown;
	public float scaleAmount;
	public float shieldRate;
	public float size;
	public float respawnTime;
	public float damageTime;
	public float scoreFlashTime;
	public int id;
	public ParticleSystem lightning;
	public ParticleSystem damage;
	public GameObject pickupSpawner;
	public GameObject singlePickupSpawner;
	public GameObject shield;
	public GameObject shieldCollider;
	public GameObject projectile;
	public GameObject lightGameObject;
	public Sprite playerDefaultSprite;
	public Sprite playerDamagedSprite;
	public VirtualJoystick joystick;

	private GameObject otherPlayer;

	private bool fireWeaponAtEnemy;
	private bool isOriginalSpriteFlipped;
	private bool isScoreFlashingCoroutineRunning;
	private bool pickingUp;
	private bool scoreFlashing;
	private bool shieldActive;
	private bool takingDamage;
	private bool turnShieldOn;
	private bool weaponActive;

	private float currentMovementSpeed;
	private float direction;
	private float lightIntensity;
	private float lightStartRange;
	private float projectileInitHeight;
	private float projectilePower;
	private float shieldPower;

	private GameManager gameManager;
	private GameObject shieldUI;
	private GameObject projectileUI;
	private GameObject currentTarget;
	private int scoreDifference;
	private int pickupCount;
	private Light light;
	private ParticleSystem.MinMaxCurve rate;
	private ParticleSystem.VelocityOverLifetimeModule lightningTrailVelocity;
	private PlayerMode playerMode;
	private Rigidbody2D rigidbody;
	private SpriteRenderer sprite;
	private TextMesh scoreTextMesh;

	private Vector2 startScaleShip;
	private Vector2 startScaleShield;
	private Vector2 lightPosition;
	private Vector2 lightPositionStart;

	private Vector3 damageRot;
	private Vector3 damageRotStart;
	private Vector3 originalStartPosition;

	void Awake() {
		damageRotStart = damage.transform.localEulerAngles;
		sprite = GetComponent<SpriteRenderer>();
		isOriginalSpriteFlipped = sprite.flipX;
		light = lightGameObject.GetComponent<Light>();
		lightIntensity = light.intensity;
		lightPositionStart = lightGameObject.transform.localPosition;
		lightStartRange = light.range;
		lightningTrailVelocity = lightning.velocityOverLifetime;
		originalStartPosition = transform.position;
		rate = new ParticleSystem.MinMaxCurve();
		rigidbody = GetComponent<Rigidbody2D>();
		scoreTextMesh = GetComponentInChildren<TextMesh>();scoreTextMesh = GetComponentInChildren<TextMesh>();
		startScaleShield  = shield.transform.localScale;
		startScaleShip  = transform.localScale;
	}

	public void Start () {
		gameManager = GameManager.instance;
		switch (id){
			case 1:
				shieldUI = GameObject.FindGameObjectWithTag("shieldP1");
				projectileUI = GameObject.FindGameObjectWithTag("projectileP1");
				joystick = GameObject.FindGameObjectWithTag("joystickP1").GetComponent<VirtualJoystick>();
				otherPlayer = GameObject.FindGameObjectWithTag("player2");
				break;
			case 2:
				if (playerMode == PlayerMode.HUMAN){
					shieldUI = GameObject.FindGameObjectWithTag("shieldP2");
					projectileUI = GameObject.FindGameObjectWithTag("projectileP2");
					joystick = GameObject.FindGameObjectWithTag("joystickP2").GetComponent<VirtualJoystick>();
				}
				otherPlayer = GameObject.FindGameObjectWithTag("player1");
				break;
		}
		if (otherPlayer == null){
			Debug.Log("ERROR OTHER PLAYER IS NULL");
		} else {
			Debug.Log("This is player "+ id + " OTHER PLAYER IS "+ otherPlayer.name);
		}
		Restart();
	}

	public void Restart(){
		shieldActive = false;
		weaponActive = true;
		takingDamage = false;
		scoreFlashing = false;
		isScoreFlashingCoroutineRunning = false;
		pickingUp = false;
		
		currentMovementSpeed = movementSpeed;
		if (isOriginalSpriteFlipped){
			direction = 1f;
		} else {
			direction = -1f;
		}
		sprite.flipX = isOriginalSpriteFlipped;
		projectilePower = 1;
		shieldPower = 1;
		pickupCount = 0;
		sprite.sprite = playerDefaultSprite;
		damageRot = damageRotStart;
		lightPosition = lightPositionStart;
		transform.position = originalStartPosition;
	}

	public void SetPlayerMode(PlayerMode playerMode){
		this.playerMode = playerMode;
	}

	public PlayerMode GetPlayerMode(){
		return this.playerMode;
		}

	public void Update () {
		damage.gameObject.SetActive(takingDamage);
		shield.SetActive(shieldActive);
		shieldCollider.SetActive(shieldActive);
		gameManager.setPlayerScore(id, pickupCount);
		scoreDifference = pickupCount - gameManager.getOtherPlayerScore(id);
		GetComponentInChildren<TextMesh>().text = pickupCount.ToString("00");

		UpdateAvatarSpritesParticles();

		if (!gameManager.hasGameStarted()){
			switch (playerMode){
				case PlayerMode.HUMAN:
					gameManager.setPlayerReady(id, true);
					return;
				case PlayerMode.COMPUTER:
					gameManager.setPlayerReady(id, true);
					return;
			}
		}

		if (takingDamage){
			light.intensity = 0f;
			return;
		}

		switch (playerMode){
			case PlayerMode.HUMAN:
/*
				float moveHorizontal = Input.GetAxis("Horizontal_joy_"+id);
				float moveVertical = Input.GetAxis("Vertical_joy_"+id);

				Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);
				transform.Translate (movement * currentMovementSpeed * Time.deltaTime, Space.World);

				if (Input.GetAxis("Horizontal_joy_"+id) != 0f){
					sprite.flipX = Input.GetAxis("Horizontal_joy_"+id) < 0f;
				}
*/
				transform.Translate(joystick.InputDirection * currentMovementSpeed * Time.deltaTime, Space.World);
				if (joystick.InputDirection.x != 0f){
					sprite.flipX = joystick.InputDirection.x < 0f;
				}

				break;
			case PlayerMode.COMPUTER:
				if (shieldPower < 0.1f){
					turnShieldOn = false;
				}
				if (scoreDifference <= 0){
					if (Random.Range(0f, 150f) < 1f){ // Chance that attack won't happen
//						Debug.Log("we're losing! we need to attack!");
						turnShieldOn = false;
						fireWeaponAtEnemy = true;
					}
				} else if (scoreDifference > 0){
//					Debug.Log("we're winning! we need to defend");
					if (shieldPower > 0.4f){
						turnShieldOn = true;
					}
				}
				if (currentTarget == null){
					currentTarget = findTarget();
					if (currentTarget == null){
						fireWeaponAtEnemy = true;
					}
				} else {
					transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, currentMovementSpeed * Time.deltaTime);
					if (!currentTarget.activeSelf){
						currentTarget = null;
					}
				}
				if (currentTarget != null){
					if (currentTarget.transform.position.x > transform.position.x){
						sprite.flipX = false;
					} else if (currentTarget.transform.position.x <= transform.position.x){
						sprite.flipX = true;
					}
				}
				break;
		}
		if (turnShieldOn && shieldPower > 0f){
			if(!audioSourceShield.isPlaying){
				audioSourceShield.Play();
			}
			shieldActive = true;
			shieldPower -= shieldRate+(pickupCount*0.0001f);
			currentMovementSpeed = movementSpeed * 0.5f;
		} else if (shieldPower < 1){
			shieldActive = false;
			if (!turnShieldOn){
				shieldPower += shieldRate;
			}
			if (shieldPower < 0.01){
				turnShieldOn = false;
			}
			currentMovementSpeed = movementSpeed;
		}

		if (fireWeaponAtEnemy){
			if (weaponActive){
				projectilePower = 0;
				weaponActive = false;
				StartCoroutine(PlayHitAudio(weaponFire));
				StartCoroutine(WeaponCooldown());
				Vector2 dir = otherPlayer.transform.position - transform.position;
				dir.Normalize();
				Rigidbody2D bulletInstance = Instantiate(projectile.GetComponent<Rigidbody2D>(), transform.position, Quaternion.Euler(new Vector3(0, 0, 1))) as Rigidbody2D;
				bulletInstance.AddForce(dir * projectileSpeed, ForceMode2D.Impulse);
				Vector2 lookAtDir = bulletInstance.velocity;
				float angle = Mathf.Atan2(lookAtDir.y, lookAtDir.x) * Mathf.Rad2Deg;
				bulletInstance.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			}
			fireWeaponAtEnemy = false;

		}
	}

	public void TurnShieldOn(){
		if (playerMode == PlayerMode.HUMAN){
			turnShieldOn = !turnShieldOn;
		}
	}

	public void FireWeapon(){
		if (playerMode == PlayerMode.HUMAN){
			fireWeaponAtEnemy = true;
		}
	}

	public bool IsTakingDamage(){
		return takingDamage;
	}

	private void UpdateAvatarSpritesParticles(){
		if (playerMode == PlayerMode.HUMAN){
			projectileUI.GetComponent<Image>().fillAmount = projectilePower;
			shieldUI.GetComponent<Image>().fillAmount = shieldPower;
		}
		float newScale = pickupCount * scaleAmount;
		Vector2 endScaleShip = new Vector2(startScaleShip.x+newScale, startScaleShip.y+newScale);
		Vector2 endScaleShield = new Vector2(startScaleShield.x+(newScale*7f), startScaleShield.y+(newScale*7f));
		transform.localScale = endScaleShip;
		shield.transform.localScale = endScaleShield;

		if (sprite.flipX){
			direction = 1f;
		} else {
			direction = -1f;
		}
		rate.constantMax = 30f * direction;
		lightningTrailVelocity.x = rate;

		if (id == 1){ //HORRIBLE HORRIBLE HACK BUT IT WORKS SO I DON'T CARE
			damageRot = new Vector3(damageRotStart.x, damageRotStart.y * -direction, damageRotStart.z);
		} else {
			damageRot = new Vector3(damageRotStart.x, damageRotStart.y * direction, damageRotStart.z);
		}
		damage.transform.localEulerAngles = damageRot;

		lightPosition = new Vector2(lightPositionStart.x * direction * Mathf.Sign(lightPositionStart.x), lightPositionStart.y);
		lightGameObject.transform.localPosition = lightPosition;
		if (isScoreFlashingCoroutineRunning){
			light.intensity += Time.deltaTime;
			light.range = lightStartRange + (newScale * (6f+Time.deltaTime));
		} else {
			light.range = lightStartRange + (newScale * 6f);
			light.intensity = lightIntensity*0.3f;;
		}
	}

	public void OnCollisionEnter2D(Collision2D c){
		if (!gameManager.hasGameStarted() || takingDamage || pickingUp){
			return;
		}
		if (c.gameObject.tag == "pickup"){
			if (shield.activeSelf) {
				return;
			}
			pickingUp = true;
			c.gameObject.SetActive(false);
			StartCoroutine(PlayRandomAudio());
			pickupCount++;
			if (!isScoreFlashingCoroutineRunning) {
				StartCoroutine(FlashScore());
			}
			pickingUp = false;
		}

		if(c.gameObject.tag == gameManager.getOtherPlayerID(id).ToString()){
			rigidbody.velocity = Vector3.zero;
			Destroy(c.gameObject);
			if (shieldActive) {
				StartCoroutine(PlayHitAudio(weaponHitShield));
				return;
			} else {
				takingDamage = true;
				switch(gameManager.getGameMode()){
					case GameMode.DEFAULT:
						if (pickupCount == 1){
							pickupCount = 0;
							pickupSpawner.GetComponent<BoidController>().SetFlockSize(1);
							Instantiate (pickupSpawner, transform.position, transform.rotation);
						} else if (pickupCount > 1){
							int newPickupCount = Mathf.FloorToInt(pickupCount/2);
							pickupSpawner.GetComponent<BoidController>().SetFlockSize(Mathf.CeilToInt((newPickupCount)));
							Instantiate (pickupSpawner, transform.position, transform.rotation);
							pickupCount = newPickupCount;
						}
						break;
					case GameMode.SINGLE_PICKUP:
						if (pickupCount > 0){
							pickupCount = 0;
							Instantiate (singlePickupSpawner, Vector3.zero, transform.rotation);
						}
						break;
					case GameMode.BATTLE:
						break;
				}
				WaveExploPostProcessing.Get().StartIt(transform.position);
				StartCoroutine(PlayHitAudio(weaponHit));
				StartCoroutine(TakeDamage());
			}
		}
	}

	private GameObject findTarget(){
		GameObject[] pickupsInScene;
		pickupsInScene = GameObject.FindGameObjectsWithTag("pickup");

		List<GameObject> activePickupsInScene = new List<GameObject>();
		for (int i=0; i<pickupsInScene.Length; i++){
			if (pickupsInScene[i].activeSelf){
				activePickupsInScene.Add(pickupsInScene[i]);
			}
		}
		if (activePickupsInScene.Count <= scoreDifference*10){
			if (scoreDifference <= 0){
//				Debug.Log("we're losing! we need to attack!");
				turnShieldOn = false;
				fireWeaponAtEnemy = true;
			} else if (scoreDifference > 0){
//				Debug.Log("we're winning! we need to defend");
				if (shieldPower > 0.4f){
					turnShieldOn = true;
				}
			}
		} else {
			turnShieldOn = false;
		}
		if (activePickupsInScene.Count > 0){
			return activePickupsInScene[Random.Range(0, activePickupsInScene.Count)];
		} else {
			return null;
		}
	}

	private IEnumerator PlayHitAudio(AudioClip audioClip){
			audioSourceHit.PlayOneShot(audioClip);
			yield return new WaitForSeconds(audioClip.length);
	}

	private IEnumerator PlayRandomAudio(){
			AudioClip audioClip = pickupSoundEffects[Random.Range(0, pickupSoundEffects.Length)];
			audioSourceHit.PlayOneShot(audioClip);
			yield return new WaitForSeconds(audioClip.length);
	}

	private IEnumerator FlashScore(){
		isScoreFlashingCoroutineRunning = true;
		float elapsed = 0.0f;
		while (elapsed < scoreFlashTime){
			elapsed += Time.deltaTime;
			if (scoreFlashing){
				scoreTextMesh.color = Color.white;
			} else {
				scoreTextMesh.color = playerColour;
			}
			scoreFlashing = !scoreFlashing;
			yield return new WaitForSeconds(0.01f);
		}
		scoreTextMesh.color = playerColour;
		isScoreFlashingCoroutineRunning = false;
	}

	private IEnumerator TakeDamage(){
		float elapsed = 0.0f;
		bool damaged = false;
		sprite.sprite = playerDefaultSprite;
		AndroidManager.HapticFeedback();
		while (elapsed < damageTime){
			elapsed += Time.deltaTime;
			if (!damaged){
				sprite.sprite = playerDamagedSprite;
			} else {
				sprite.sprite = playerDefaultSprite;
			}
			damaged = !damaged;
			yield return new WaitForSeconds(0.05f);
		}
		takingDamage = false;
		sprite.sprite = playerDefaultSprite;
	}

	private IEnumerator WeaponCooldown(){
		float elapsed = 0.0f;
		float percentage;
		while (elapsed < weaponCooldown){
			elapsed += Time.deltaTime;
			percentage = elapsed / weaponCooldown;
//			projectilePower = Mathf.Lerp(projectilePower, 1f, Time.deltaTime);
			projectilePower = Mathf.Lerp(0f, 1f, percentage);
			yield return null;
		}
		weaponActive = true;
	}
}
