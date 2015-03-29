using UnityEngine;
using System.Collections;

public class ParallaxPlatform : ParallaxProperties {

	private ParallaxSpawner spawner;
	private bool stopMovingAt = true;

	private void Start() {
		StartCoroutine("GemSearch");
	}

	protected override void Update() {
		switch (propPosition) {
			case ObjectPosition.Left:
				Destroy(this.gameObject);
				spawner.SendMessage("SpawnNext");
				break;

			case ObjectPosition.MiddleCenter:
				spawner.SetCurPlatform(this.transform);
				break;
		}

		if (stopMovingAt) {
			if (propPosition == propStopAt) {
				StartCoroutine("StopMoving", propStopduration);
				stopMovingAt = false;
			}
		}

		base.Update();
	}

	public void SetSpawner(ParallaxSpawner spawn) {
		spawner = spawn;
	}

	private IEnumerator GemSearch() {
		yield return new WaitForSeconds(0.2f);
		GameUtility.SearchGemRecursively(transform, ref gemCount);

		if (spawner != null)
			spawner.AddGem(gemCount);
	}

	private IEnumerator StopMoving(float time) {
		propIsMoving = false;
		yield return new WaitForSeconds(time);
		propIsMoving = true;
	}
}
