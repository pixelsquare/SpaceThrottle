using UnityEngine;
using System.Collections;

public class WarningScript : MonoBehaviour {

	public Color startColor;
	public Color endColor;

	private float duration = 0.5f;
	private float destroyTime = 2.0f;
	private float time = 0.0f;

	public void Init() {
		StartCoroutine("DestroyTime", destroyTime);
	}

	private void Update() {
		time += Time.deltaTime;
		float lerp = Mathf.PingPong(time, duration) / duration;
		guiTexture.color = Color.Lerp(startColor, endColor, lerp);

	}

	private IEnumerator DestroyTime(float time) {
		yield return new WaitForSeconds(time);
		Destroy(gameObject);
	}

	public void Setduration(float dur) {
		duration = dur;
	}

	public void SetDestroyTime(float time) {
		destroyTime = time;
	}
}
