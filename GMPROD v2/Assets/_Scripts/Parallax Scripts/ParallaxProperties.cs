using UnityEngine;
using System.Collections;

public enum ObjectPosition { None, Top, Left, MiddleCenter, Center, Bottom, Right, Null };

public class ParallaxProperties : MonoBehaviour {
	protected float propScrollSpeed = 5.0f;
	protected float propPositionOffset = 0.2f;
	protected Transform propPlayer;
	protected Camera propCamera;
	protected Layers propLayer;
	protected Vector3 propMovementDirection = new Vector3();

	protected bool propIsMoving = true;
	protected ObjectPosition propPosition = ObjectPosition.Null;

	protected Vector3 objectSize = new Vector3();
	protected Vector3 viewportCoord = new Vector3();

	protected ObjectPosition propStopAt;
	protected float propStopduration;

	protected int gemCount = 0;

	public static bool pause = false;

	protected virtual void Update() {
		UpdateObjectPosition();

		propIsMoving = !pause;
		if (!propIsMoving) return;
			
		transform.position += propMovementDirection * propScrollSpeed * Time.deltaTime;
	}

	// Used to update the current position of the object on the screen / camera

	protected void UpdateObjectPosition() {
		Vector3 objectCorner = gameObject.transform.position;
		Vector2 multiplier = new Vector2();

		MeshFilter objMeshFilter = gameObject.GetComponent<MeshFilter>();
		if (objMeshFilter != null) {
			objectSize = objMeshFilter.sharedMesh.bounds.size;
			objectSize.Scale(gameObject.transform.localScale);
		}
		else
			objectSize = gameObject.transform.localScale;

		if (propMovementDirection.x != 0.0f) {
			multiplier.x = propMovementDirection.x / Mathf.Abs(propMovementDirection.x);
			objectCorner.x += multiplier.x * objectSize.x / -2.0f;
		}

		if (propMovementDirection.z != 0.0f) {
			multiplier.y = propMovementDirection.z / Mathf.Abs(propMovementDirection.z);
			objectCorner.z += multiplier.y * objectSize.z / -2.0f;
		}

		viewportCoord = propCamera.WorldToViewportPoint(objectCorner);

		Vector2 defaultSize = new Vector2(27.0f, 7.0f);
		Vector2 cameraOffset = new Vector2();
		cameraOffset.x = objectSize.x / defaultSize.x;
		cameraOffset.y = objectSize.y / defaultSize.y;

		cameraOffset.x = Mathf.Clamp(cameraOffset.x, 1.0f, 2.0f);
		cameraOffset.y = Mathf.Clamp(cameraOffset.y, 1.0f, 2.0f);

		Vector2 middleCenterMin = new Vector2();
		middleCenterMin.x = (cameraOffset.x * 0.5f) - 0.01f;
		middleCenterMin.y = (cameraOffset.y * 0.5f) - 0.01f;

		Vector2 middleCenterMax = new Vector2();
		middleCenterMax.x = (cameraOffset.x * 0.5f) + 0.01f;
		middleCenterMax.y = (cameraOffset.y * 0.5f) + 0.01f;

		if (multiplier.x != 0.0f) {
			if (viewportCoord.x > cameraOffset.x)
				propPosition = ObjectPosition.Right;
			else if (viewportCoord.x > middleCenterMin.x && viewportCoord.x < middleCenterMax.x)
				propPosition = ObjectPosition.MiddleCenter;
			else if (viewportCoord.x < 0.0f)
				propPosition = ObjectPosition.Left;
			else if (viewportCoord.x < middleCenterMin.x || viewportCoord.x > middleCenterMax.x)
				propPosition = ObjectPosition.Center;
			else
				propPosition = ObjectPosition.None;
		}

		if (multiplier.y != 0.0f) {
			if (viewportCoord.y > cameraOffset.y)
				propPosition = ObjectPosition.Top;
			else if (viewportCoord.y > middleCenterMin.y && viewportCoord.y < middleCenterMax.y)
				propPosition = ObjectPosition.MiddleCenter;
			else if (viewportCoord.y < 0.0f)
				propPosition = ObjectPosition.Bottom;
			else if (viewportCoord.y < middleCenterMin.y || viewportCoord.y > middleCenterMax.y)
				propPosition = ObjectPosition.Center;
			else
				propPosition = ObjectPosition.None;
		}
	}

	// Accessible functions outside this general parallax properties class

	public void SetPlayer(Transform player) {
		propPlayer = player;
	}
	public Transform GetPlayer() { return propPlayer; }

	public void SetScrollSpeed(float scrollpropScrollSpeed) {
		propScrollSpeed = scrollpropScrollSpeed;
	}
	public float GetScrollSpeed() { return propScrollSpeed; }

	public void SetPositionOffset(float offset) {
		propPositionOffset = offset;
	}
	public float GetOffset() { return propPositionOffset; }

	public void SetCamera(Camera camera) {
		propCamera = camera;
	}
	public Camera GetCamera() { return propCamera; }

	public void SetLayer(Layers layer) {
		propLayer = layer;
	}
	public Layers GetLayer() { return propLayer; }

	public void SetMovementDirection(Vector3 dir) {
		propMovementDirection = dir;
	}
	public Vector3 GetMovementDirection() { return propMovementDirection; }

	public void SetIsMoving(bool flag) {
		propIsMoving = flag;
	}
	public bool GetIsMoving() { return propIsMoving; }

	public void SetObjectPosition(ObjectPosition pos) {
		propPosition = pos;
	}
	public ObjectPosition GetObjectPosition() { return propPosition; }

	public void SetStopAt(ObjectPosition stop) {
		propStopAt = stop;
	}
	public ObjectPosition GetStopAt() { return propStopAt; }

	public void SetStopduration(float duration) {
		propStopduration = duration;
	}
	public float GetStopduration() { return propStopduration; }

	public void AddGem(int num) {
		gemCount += num;
	}
	public void SetGemCount(int count) {
		gemCount = count;
	}
	public int GetGemCount() { return gemCount; }
}
