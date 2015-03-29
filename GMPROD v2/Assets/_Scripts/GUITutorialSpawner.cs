using UnityEngine;
using System.Collections;

public class GUITutorialSpawner : ParallaxProperties {
	public Camera camera;
	public TextureUI[] texturesUI;

	private bool spawnNext = true;
	private int textureIndx = 0;

	private float spawnTime = 0.0f;

	private void Start() {
		propCamera = camera;
	}

	protected override void  Update() {
		if (propIsMoving) {
			if (textureIndx < texturesUI.Length) {
				if (spawnNext) {
					spawnTime += Time.deltaTime;

					if (spawnTime > texturesUI[textureIndx].delay) {
						StartCoroutine("SpawnNextUI", texturesUI[textureIndx].duration + 4);
					}
				}
			}
		}

		base.Update();
	}

	private IEnumerator SpawnNextUI(float time) {
		spawnNext = false;
		GameObject tmpTexture = (GameObject)Instantiate(texturesUI[textureIndx].texture);
		TutorialGUI textureScript = tmpTexture.GetComponent<TutorialGUI>();
		textureScript.SetDuration(texturesUI[textureIndx].duration);
		textureScript.SetCamera(camera);

		yield return new WaitForSeconds(time);
		spawnTime = 0.0f;
		textureIndx++;
		spawnNext = true;
	}
}

[System.Serializable]
public class TextureUI {
	public GameObject texture;
	public float duration;
	public float delay;
}