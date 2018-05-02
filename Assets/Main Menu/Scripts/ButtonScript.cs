using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	public Texture2D normalTexture;
	public Texture2D hoverTexture;

	public int loadLevelIndx;
	public float scaleOffset = 0.01f;
	public bool isExit;
	public Color hoverColor = Color.gray;

	private Vector3 originalScale;
	private Color originalColor;
	private AudioSource audio;

	private void Start() {
		originalScale = transform.localScale;
		originalColor = GetComponent<GUITexture>().color;

		audio = this.gameObject.GetComponent<AudioSource>();
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
		if (isExit)
			Application.Quit();
		else
			Application.LoadLevel(loadLevelIndx);
	}
}
