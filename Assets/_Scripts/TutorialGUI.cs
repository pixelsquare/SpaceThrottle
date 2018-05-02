using UnityEngine;
using System.Collections;

public class TutorialGUI : ParallaxProperties {
	public Color startColor;
	public Color endColor;

	private float duration;

	private bool freeze = false;
	private float fadeDuration = 2.0f;
	private float time = 0.0f;

	private bool destroyTexture = false;
	private float destroyTimer = 0.0f;

	private void Start() {
		GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0.0f);
		//StartCoroutine("DestroyTime", ((fadeDuration * 2) + duration));
	}

	protected override void Update() {
		if (propIsMoving) {
			if (!freeze) {
				time += Time.deltaTime;
			}

			if (destroyTexture) {
				destroyTimer += Time.deltaTime;
				if (destroyTimer > 2.0f)
					Destroy(gameObject);
			}
		}

		float lerp = Mathf.PingPong(time, fadeDuration) / fadeDuration;
		GetComponent<GUITexture>().color = Color.Lerp(startColor, endColor, lerp);

		if (lerp >= 0.9f) {
			StartCoroutine("FreezeTexture", duration);
		}

		base.Update();
	}

	private IEnumerator FreezeTexture(float time) {
		freeze = true;
		yield return new WaitForSeconds(time);
		freeze = false;
		destroyTexture = true;
	}

	public void SetDuration(float time) {
		duration = time;
	}

}