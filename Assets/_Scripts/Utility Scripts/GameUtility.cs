using UnityEngine;
using System.Collections;

public class GameUtility : MonoBehaviour {

	// This global function is used to change the layer of the root including its children

	public static void ChangeLayerRecursively(Transform root, Layers layer) {
		if (root.gameObject.layer != (int)layer)
			root.gameObject.layer = (int)layer;

		foreach (Transform obj in root) {
			if (root.gameObject.layer != (int) layer)
				root.gameObject.layer = (int) layer;

			ChangeLayerRecursively(obj, layer);
		}
	}

	// This global function is used to set properties to ParallaxNPC script

	public static void SetNPCValuesRecursively(Transform root, float scrollSpeed, float posOffset,
		Transform player, Camera camera, Layers layer, Vector3 moveDirection) {
		
		foreach(Transform obj in root) {
			ParallaxNPC objScript = obj.GetComponent<ParallaxNPC>();
				if(objScript != null) {
					objScript.SetScrollSpeed(scrollSpeed);
					objScript.SetPositionOffset(posOffset);
					objScript.SetPlayer(player);
					objScript.SetCamera(camera);
					objScript.SetLayer(layer);
					objScript.SetMovementDirection(moveDirection);
				}

			SetNPCValuesRecursively(obj, scrollSpeed, posOffset,
				player, camera, layer, moveDirection);
		}
	}

	// This global function is used to add / create a sound to the gameobject

	public static AudioSource AddSound(GameObject root, AudioClip clip) {
		AudioSource rootAudioSource = root.GetComponent<AudioSource>();

		if (rootAudioSource == null) {
			rootAudioSource = root.AddComponent<AudioSource>();
		}
		rootAudioSource.clip = clip;
		rootAudioSource.playOnAwake = false;

		return rootAudioSource;
	}

	public static void SearchGemRecursively(Transform root, ref int num) {
		foreach (Transform obj in root) {
			if (obj.tag == "Gem") {
				num++;
			}

			SearchGemRecursively(obj, ref num);
		}
	}
}
