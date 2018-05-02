using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Playsound : MonoBehaviour {
	public Sound mainSound;
	public Sound endSound;

	private AudioSource audioSource;

	private void Awake() {
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	private void PlayMainSound() {
		StartCoroutine("Play", mainSound);
	}

	private void DestroySound() {
		Destroy(gameObject.GetComponent<Collider>());
		gameObject.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

		StartCoroutine("Play", endSound);
		StartCoroutine("DestroyObject", endSound.clip.length);
	}

	private IEnumerator DestroyObject(float time) {
		yield return new WaitForSeconds(time);
		Destroy(this.gameObject);
	}

	private IEnumerator Play(Sound sound) {
		yield return new WaitForSeconds(sound.playDelay);
		if (sound.clip != null) {
			audioSource.clip = sound.clip;
			audioSource.loop = sound.isLooping;
			audioSource.Play();
		}

	}
}

[System.Serializable]
public class Sound {
	public AudioClip clip;
	public float playDelay;
	public bool isLooping;
}
