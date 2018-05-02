using UnityEngine;
using System.Collections;

public class CharacterAnim : MonoBehaviour {

	public float moveduration;
	public Vector3 startPos;
	public Vector3 endPos;

	public float rotduration;
	public Vector3 startRot;
	public Vector3 endRot;

	private void Start() {
		if (moveduration == 0.0f) {
			Debug.LogError("Move duration is not set to " + gameObject.name);
			Debug.Break();
		}

		if (rotduration == 0.0f) {
			Debug.LogError("Rot duration is not set to " + gameObject.name);
			Debug.Break();
		}
	}

	private void Update() {
		float moveLerp = Mathf.PingPong(Time.time, moveduration) / moveduration;
		transform.position = Vector3.Lerp(startPos, endPos, moveLerp);

		float rotLerp = Mathf.PingPong(Time.time, rotduration) / rotduration;
		transform.eulerAngles = Vector3.Lerp(startRot, endRot, rotLerp);
	}
}
