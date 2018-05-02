using UnityEngine;
using System.Collections;

public class ParallaxNPC : ParallaxProperties {

	public float scrollSpeed;
	public float rotationSpeed = -300.0f;

	public float moveDelay;
	public bool moveToPlayerDirection;
	public Vector3 moveDirection;

	public Indicator indicator;
	private bool showIndicator;

	private void Start() {
		propScrollSpeed = scrollSpeed;
		propMovementDirection = moveDirection;

		if (moveDelay != 0.0f) {
			propIsMoving = false;
			StartCoroutine("StartMoving");
		}

		if (propPlayer != null && moveToPlayerDirection) {
			Vector3 tmpPlayerPos = propPlayer.position;
			tmpPlayerPos.x = transform.position.x;
			transform.position = tmpPlayerPos;
		}

		showIndicator = true;
	}

	protected override void Update() {
		if (propPosition == ObjectPosition.Left || propPosition == ObjectPosition.Bottom) {
			Destroy(this.gameObject);
		}

		if (rotationSpeed != 0.0f) {
			transform.localEulerAngles += Vector3.up * rotationSpeed * Time.deltaTime;
		}

		if (showIndicator) {
			if (propMovementDirection.x != 0.0f) {
				if (viewportCoord.x > (indicator.showPositionOffset + 1.0f) - 0.09f && viewportCoord.x < (indicator.showPositionOffset + 1.0f) + 0.01f) {
					SendMessage("ShowIndicator");
					SendMessage("PlayMainSound");
					showIndicator = false;
				}
			}

			if (propMovementDirection.z != 0.0f) {
				if (viewportCoord.y > (indicator.showPositionOffset + 1.0f) - 0.08f && viewportCoord.y < (indicator.showPositionOffset + 1.0f) + 0.02f) {
					SendMessage("ShowIndicator");
					SendMessage("PlayMainSound");
					showIndicator = false;
				}
			}
		}

		base.Update();
	}

	private IEnumerator StartMoving() {
		yield return new WaitForSeconds(moveDelay);
		propIsMoving = true;
	}

	private void ShowIndicator() {
		if (indicator.indicatorPrefab != null) {
			Vector2 screenPos = new Vector2();
			if (propMovementDirection.x != 0.0f) {
				screenPos.x = 0.95f;
				screenPos.y = viewportCoord.y;
			}

			if (propMovementDirection.z != 0.0f) {
				screenPos.x = viewportCoord.x - (indicator.showPositionOffset * 0.3f);
				screenPos.y = 0.88f;
			}

			GameObject indicatorSign = (GameObject)Instantiate(indicator.indicatorPrefab.gameObject, new Vector3(screenPos.x, screenPos.y, 0.0f), Quaternion.identity);
			indicatorSign.layer = gameObject.layer;
			WarningScript indicatorScript = indicatorSign.GetComponent<WarningScript>();
			indicatorScript.Setduration(indicator.duration);
			indicatorScript.SetDestroyTime(indicator.destroyTime);
			indicatorScript.SendMessage("Init");
		}
	}
}

[System.Serializable]
public class Indicator {
	public Transform indicatorPrefab;
	public float showPositionOffset = 0.5f;
	public float duration = 1.0f;
	public float destroyTime = 2.0f;
}
