using UnityEngine;
using System.Collections;

public enum PauseWindowButtons {
	MainMenu,
	Restart,
	Resume,
	NextLevel
};

public class WindowButtons : MonoBehaviour {
	public Texture2D normalTexture;
	public Texture2D hoverTexture;

	public int currentLevelIndx;
	public PauseWindowButtons buttonType;

	public float scaleOffset = 0.01f;
	public Color hoverColor = Color.gray;

	private Vector3 originalScale;
	private Color originalColor;
	private AudioSource audio;

	private void Start() {
		originalScale = transform.localScale;
		originalColor = GetComponent<GUITexture>().color;

		audio = this.gameObject.GetComponent<AudioSource>();
	}

	public void ResetButton() {
		transform.localScale = originalScale;
		GetComponent<GUITexture>().texture = normalTexture;
		GetComponent<GUITexture>().color = Color.grey;
	}

	private void OnMouseEnter() {
		transform.localScale += Vector3.one * scaleOffset;
		GetComponent<GUITexture>().texture = hoverTexture;
		GetComponent<GUITexture>().color = hoverColor;

		if (audio && !audio.isPlaying) {
			audio.Play();
		}
	}

	private void OnMouseExit() {
		transform.localScale = originalScale;
		GetComponent<GUITexture>().texture = normalTexture;
		GetComponent<GUITexture>().color = originalColor;

		if (audio) {
			audio.Stop();
		}
	}

	private void OnMouseDown() {
		if (buttonType == PauseWindowButtons.MainMenu) {
			ParallaxProperties.pause = false;
			Application.LoadLevel(0);
		}

		if (buttonType == PauseWindowButtons.Restart) {
			ParallaxProperties.pause = false;
			Application.LoadLevel(currentLevelIndx);
		}

		if (buttonType == PauseWindowButtons.Resume) {
			ParallaxProperties.pause = false;
		}

		if (buttonType == PauseWindowButtons.NextLevel) {
			ParallaxProperties.pause = false;
			Application.LoadLevel(currentLevelIndx);
		}

		GameManager.pauseWindowInit = false;
	}
}
