using UnityEngine;
using System.Collections;

public class WindowAnim : MonoBehaviour {

	private bool fall = false;

	private void Update() {
		if (!fall) {
			Vector3 tmpFallPos = transform.position;
			tmpFallPos.y -= Time.deltaTime;
			transform.position = tmpFallPos;

			if (transform.position.y < 0.01f)
				fall = true;
		}
	}

	public void InitPauseWindow() {
		Vector3 tmpPos = transform.position;
		tmpPos.y = 0.5f;
		transform.position = tmpPos;

		fall = false;
	}
}
