using UnityEngine;
using System.Collections;

public class ParallaxSpawner : ParallaxProperties {
	private Transform initialPlatform;
	private Transform[] otherPlatforms;
	private PlatformProperties[] platformProperties;

	private Transform curPlatform;
	private Transform nextPlatform;
	private int platformIndx;

	private void Start() {
		platformIndx = 0;

		initialPlatform = PlatformInit(otherPlatforms[platformIndx]);
		curPlatform = initialPlatform;
		SendMessage("SpawnNext");
	}

	protected override void Update() {
		// Update the current platform's moving flag to the next platform's moving flag

		if (curPlatform != null && nextPlatform != null) {
			ParallaxPlatform curPlatformScript = curPlatform.GetComponent<ParallaxPlatform>();
			ParallaxPlatform nextPlatformScript = nextPlatform.GetComponent<ParallaxPlatform>();

			nextPlatformScript.SetIsMoving(curPlatformScript.GetIsMoving());
		}
		base.Update();
	}

	public void SetPlatformProperties(PlatformProperties[] platformProp) {
		platformProperties = platformProp;

		otherPlatforms = new Transform[platformProp.Length];
		int indx = 0;

		foreach (PlatformProperties obj in platformProp) {
			otherPlatforms[indx] = obj.platform;
			indx++;
		}
	}

	public void SetCurPlatform(Transform cur) {
		curPlatform = cur;
	}

	public int GetPlatformIndx() { return platformIndx; }

	private void SpawnNext() {
		if (platformIndx < otherPlatforms.Length) {
			Transform tmpPlatform = (Transform)Instantiate(otherPlatforms[platformIndx]);
			tmpPlatform.name = platformIndx + " " + otherPlatforms[platformIndx].name;
			nextPlatform = PlatformInit(tmpPlatform);

			ComputePosition(nextPlatform);
			GameUtility.SetNPCValuesRecursively(nextPlatform, propScrollSpeed, propPositionOffset,
				propPlayer, propCamera, propLayer, propMovementDirection);
			GameUtility.ChangeLayerRecursively(nextPlatform, propLayer);
		}
	}

	private Transform PlatformInit(Transform platform) {
		// Checking if there is a Parallax Platform script attached to the platform
		// When there is none, add the script

		ParallaxPlatform platformScript = platform.GetComponent<ParallaxPlatform>();
		if (platformScript == null) {
			platformScript = platform.gameObject.AddComponent<ParallaxPlatform>();
		}

		// Set private members of the script

		platformScript.SetScrollSpeed(propScrollSpeed);
		platformScript.SetPositionOffset(propPositionOffset);
		platformScript.SetPlayer(propPlayer);
		platformScript.SetCamera(propCamera);
		platformScript.SetLayer(propLayer);
		platformScript.SetMovementDirection(propMovementDirection);
		platformScript.SetSpawner(this);

		platformScript.SetStopAt(platformProperties[platformIndx].stopAt);
		platformScript.SetStopduration(platformProperties[platformIndx].stopduration);

		platformIndx++;
		return platform;
	}

	private void ComputePosition(Transform platform) {
		// Computing the position of the next platform beside the current platform

		Vector3 curPlatformSize = new Vector3();
		MeshFilter curPlatformMesh = curPlatform.GetComponent<MeshFilter>();
		if (curPlatformMesh != null) {
			curPlatformSize = curPlatformMesh.mesh.bounds.size;
			curPlatformSize.Scale(curPlatform.transform.localScale);
		}
		else
			curPlatformSize = curPlatform.localScale;

		Vector3 platformSize = new Vector3();
		MeshFilter platformMesh = platform.GetComponent<MeshFilter>();
		if (platformMesh != null) {
			platformSize = platformMesh.mesh.bounds.size;
			platformSize.Scale(platform.transform.localScale);
		}
		else
			platformSize = platform.transform.localScale;

		// Computing for the new position of the spawner
		// Getting the sum of the average of the sizes of the current and next
		// platform to get the new position of the spawner

		Vector3 newPosition = transform.position;
		newPosition.x = ((curPlatformSize.x + platformSize.x) * 0.5f) - propPositionOffset;
		newPosition.y = 0.0f;

		nextPlatform.position = newPosition;
	}
}
