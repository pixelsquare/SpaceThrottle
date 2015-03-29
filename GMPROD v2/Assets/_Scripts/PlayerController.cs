using UnityEngine;
using System.Collections;

public class PlayerController : ParallaxProperties {
	public float playerSpeed = 5.0f;
	public Transform bullet;
	public GameObject indicator;

	public Vector2 moveLimitMin = new Vector2(-0.02f, -0.08f);
	public Vector2 moveLimitMax = new Vector2(0.8f, 0.5f);

	private bool controlIsMine;
	private bool hasTouchedPortal = false;

	private Vector3 indicatorOriginalPos;

	private void Start() {
		indicator.SetActive(false);
		propMovementDirection = new Vector3(1.0f, 0.0f, 1.0f);

		indicatorOriginalPos = indicator.transform.localPosition;
	}

	protected override void Update() {
		float horizontal = 0;
		float vertical = 0;

		AnimatedTextureUV animationScript = gameObject.GetComponent<AnimatedTextureUV>();
		animationScript.enabled = propIsMoving;

		indicator.SetActive(controlIsMine);

		if (controlIsMine) {
			indicator.transform.localPosition = indicatorOriginalPos;
			indicator.transform.localEulerAngles = Vector3.zero;
		}

		if (viewportCoord.y > 0.4f) {
			indicator.transform.localPosition = new Vector3(0.0f, 0.0f, -0.45f);
			indicator.transform.localEulerAngles = new Vector3(0.0f, -180.0f, 0.0f);
		}

		if (viewportCoord.y < 0.0f) {
			indicator.transform.localPosition = indicatorOriginalPos;
			indicator.transform.localEulerAngles = Vector3.zero;
		}

		if (propIsMoving && controlIsMine) {
			horizontal = this.playerSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
			if (((viewportCoord.x > moveLimitMin.x) || (horizontal > 0.0f)) && ((viewportCoord.x < moveLimitMax.x) || (horizontal < 0.0f))) {
				transform.Translate(Vector3.right * horizontal);
			}

			vertical = this.playerSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
			if (((viewportCoord.y > moveLimitMin.y) || (vertical > 0.0f)) && ((viewportCoord.y < moveLimitMax.y) || (vertical < 0.0f))) {
				transform.Translate(Vector3.forward * vertical);
			}

			if (rigidbody.IsSleeping())
				rigidbody.velocity = Vector3.zero;

			if (Input.GetButtonDown("Fire1")) {
				Transform tmpBullet = (Transform)Instantiate(bullet, transform.position, Quaternion.identity);
				BulletMove bulletScript = tmpBullet.GetComponent<BulletMove>();
				if (bulletScript == null) {
					bulletScript = tmpBullet.gameObject.AddComponent<BulletMove>();
				}
				bulletScript.SetCamera(propCamera);
			}
		}

		base.Update();
	}

	public bool GetHasTouchedPortal() {
		return hasTouchedPortal;
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.layer == gameObject.layer) {
			if (col.tag == "Gem") {
				col.SendMessageUpwards("DestroySound");
				gemCount++;
			}

			if (col.tag == "Asteroid" || col.tag == "Wood" || col.tag == "Broken Glass" || col.tag == "Stalagmite") {
				col.SendMessageUpwards("DestroySound", SendMessageOptions.RequireReceiver);
				Destroy(this.gameObject);
			}

			if (col.tag == "Finish") {
				hasTouchedPortal = true;
			}
		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.contacts[0].normal.x != 0.0f && col.contacts[0].normal.z > 0.0f) {
			rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
									RigidbodyConstraints.FreezePositionZ |
									RigidbodyConstraints.FreezeRotation;
		}
	}

	void OnCollisionExit(Collision col) {
		rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
								RigidbodyConstraints.FreezeRotation;

		rigidbody.velocity = Vector3.zero;
	}

	public void SetControl(bool control) {
		controlIsMine = control;
	}
}
