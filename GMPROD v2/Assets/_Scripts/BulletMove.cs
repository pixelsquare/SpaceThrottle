using UnityEngine;
using System.Collections;

public class BulletMove : ParallaxProperties {

	public float bulletSpeed = 15.0f;

	private void Start() {
		propMovementDirection = Vector3.right;
	}

	protected override void Update() {
		if(propIsMoving)
			transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);

		if (propPosition == ObjectPosition.Right) {
			Destroy(this.gameObject);
		}

		base.Update();
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Asteroid") {
			col.SendMessageUpwards("DestroySound");
			Destroy(this.gameObject);
		}
	}
}
