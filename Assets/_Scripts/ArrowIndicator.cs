using UnityEngine;
using System.Collections;

public class ArrowIndicator : MonoBehaviour {

	public float speed = 5.0f;
	public float amplitude = 0.5f;

	void Update () {
		transform.position += transform.forward * Mathf.Sin(Time.time * this.speed) * this.amplitude;
	}
}
