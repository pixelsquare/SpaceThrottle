using UnityEngine;
using System.Collections;

public class ParallaxBackground : ParallaxProperties {

	public float scrollSpeed = 5.0f;
	public float positionOffset = 0.2f;
	public Camera camera;
	public Vector3 movementDirection = new Vector3();

	public Transform parallaxPartner;

	private void Awake() {
		propScrollSpeed = scrollSpeed;
		propPositionOffset = positionOffset;
		propCamera = camera;
		propMovementDirection = movementDirection;
	}


	protected override void Update() {
		Vector3 partnerSize = new Vector3();
		Vector3 newPosition = new Vector3();
		newPosition = transform.position;

		MeshFilter partnerMeshFilter = parallaxPartner.GetComponent<MeshFilter>();

		if (partnerMeshFilter != null) {
			partnerSize = partnerMeshFilter.sharedMesh.bounds.size;
			partnerSize.Scale(parallaxPartner.localScale);
		}
		else
			partnerSize = parallaxPartner.localScale;

		if (propPosition == ObjectPosition.Left) {
			newPosition.x = parallaxPartner.position.x + (((partnerSize.x - propPositionOffset) + objectSize.x) / 2.0f);
			transform.position = newPosition;
		}

		base.Update();
	}
}
