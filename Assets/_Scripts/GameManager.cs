using UnityEngine;
using System.Collections;

public enum SceneManager {
	Tutorial,
	Level1,
	Level2,
	Level3,
};

public class GameManager : MonoBehaviour {
	public SceneManager scene;
	public PauseWindowProperties pauseWindowProperties;
	public GameOverWindowProperties gameOverProperties;
	public EndGameWindowProperties endGameProperties;

	public WindowProperties topWindow;
	public WindowProperties bottomWindow;

	private int curGemCount;
	private int maxGemCount;

	private float tmpGemCount = 0.0f;
	private float gemPercentage = 0.0f;

	private int starCurIndx = 0;
	private int starMaxIndx = 0;
	private bool starIndxIncrement = false;

	public static bool pauseWindowInit = false;

	private bool gameOver = false;
	private bool gameEnd = false;

	private void Awake() {
		pauseWindowProperties.Initialize();
		gameOverProperties.Initialize();
		endGameProperties.Initialize();

		if (scene == SceneManager.Tutorial) {
			endGameProperties.comments.text = endGameProperties.phrases[0];
		}
		else {
			endGameProperties.phraseIndx = Random.Range(1, endGameProperties.phrases.Length);
			endGameProperties.comments.text = endGameProperties.phrases[endGameProperties.phraseIndx];
		}

		topWindow.InitializeProperties();
		bottomWindow.InitializeProperties();

		pauseWindowInit = false;
		gameOver = false;
		gameEnd = false;
	}

	private void Update() {
		topWindow.UpdateWindow();
		bottomWindow.UpdateWindow();

		// Game Over

		if (!gameOver) {
			if (!topWindow.PlayerIsAlive() || !bottomWindow.PlayerIsAlive()) {
				ParallaxProperties.pause = true;
				gameOverProperties.ResetButton();
				gameOverProperties.gameOverWindow.InitPauseWindow();

				gameOver = true;
			}
		}

		if (gameOver) {
			gameOverProperties.comments.text = gameOverProperties.phrases[gameOverProperties.phraseIndx];

			gameOverProperties.gameOverBG.SetActive(gameOver);
			gameOverProperties.gameOverWindow.gameObject.SetActive(gameOver);

			gameOverProperties.comments.text = "" + gameOverProperties.phrases[gameOverProperties.phraseIndx];
		}

		curGemCount = topWindow.GetCurGemCount() + bottomWindow.GetCurGemCount();
		maxGemCount = topWindow.GetGemCount() + bottomWindow.GetGemCount();

		bool switchPlayer = Input.GetButtonDown("Switch Player");

		if (topWindow.player != null && bottomWindow.player != null) {
			if (switchPlayer) {
				topWindow.control = !topWindow.control;
				bottomWindow.control = !bottomWindow.control;
			}
		}
		else {
			if (topWindow.player)
				topWindow.control = true;
			if (bottomWindow.player)
				topWindow.control = true;
		}

		// Game Over
		if (!gameEnd) {
			if (topWindow.HasTouchedPortal() && bottomWindow.HasTouchedPortal()) {
				ParallaxProperties.pause = true;
				endGameProperties.ResetButton();
				endGameProperties.endGameWindow.InitPauseWindow();

				gameEnd = true;
			}
		}

		if (gameEnd) {
			endGameProperties.endGameBG.SetActive(gameEnd);
			endGameProperties.endGameWindow.gameObject.SetActive(gameEnd);

			if (scene == SceneManager.Tutorial) {
				endGameProperties.comments.gameObject.SetActive(true);
				endGameProperties.comments.gameObject.transform.localPosition = new Vector3(0.5f, 0.55f, 2.0f);

				endGameProperties.slash.gameObject.SetActive(false);
				endGameProperties.curGem.gameObject.SetActive(false);
				endGameProperties.maxGem.gameObject.SetActive(false);

				starMaxIndx = 3;
				if (!starIndxIncrement) {
					StartCoroutine("ShowStar", 0.5f);
				}
			}
			else {
				int tmpIndx = Random.Range(1, endGameProperties.phrases.Length);
				endGameProperties.comments.text = endGameProperties.phrases[tmpIndx];

				gemPercentage = ((float)curGemCount / (float)maxGemCount) * 100.0f;

				if (gemPercentage >= 0 && gemPercentage < 33.3f)
					starMaxIndx = 1;
				if (gemPercentage >= 33.3f && gemPercentage < 66.6f)
					starMaxIndx = 2;
				if (gemPercentage >= 66.6f && gemPercentage < 100.0f)
					starMaxIndx = 3;

				if (gameEnd) {
					Invoke("CountGem", 0.5f);
				}

				endGameProperties.curGem.text = "" + (int)tmpGemCount;
				endGameProperties.maxGem.text = "" + maxGemCount;
			}
		}

		
		// Pause

		if (!pauseWindowInit && !gameEnd) {
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
				ParallaxProperties.pause = true;
				pauseWindowProperties.pauseWindow.InitPauseWindow();
				pauseWindowProperties.ResetButton();
				pauseWindowInit = true;
			}
		}

		pauseWindowProperties.pauseBG.SetActive(pauseWindowInit);
		pauseWindowProperties.pauseWindow.gameObject.SetActive(pauseWindowInit);

		if (Input.GetKeyDown(KeyCode.Alpha0)) {
			Application.LoadLevel(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Application.LoadLevel(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Application.LoadLevel(2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			Application.LoadLevel(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			Application.LoadLevel(4);
		}
	}

	private void CountGem() {
		if (tmpGemCount == curGemCount) {
			endGameProperties.comments.gameObject.SetActive(true);
			if (!starIndxIncrement) {
				StartCoroutine("ShowStar", 1.0f);
			}
		}

		if (tmpGemCount < curGemCount)
			tmpGemCount += 0.5f;

	}

	private IEnumerator ShowStar(float time) {
		starIndxIncrement = true;
		yield return new WaitForSeconds(time);
		if(starCurIndx < starMaxIndx) {
			endGameProperties.stars[starCurIndx].SetActive(true);
			starCurIndx++;
			starIndxIncrement = false;
		}
	}
}

[System.Serializable]
public class PauseWindowProperties {
	public GameObject pauseBG;
	public WindowAnim pauseWindow;

	public void Initialize() {
		pauseBG.SetActive(false);
		pauseWindow.gameObject.SetActive(false);
	}

	public void ResetButton() {
		ResetButtonRecursively(pauseWindow.transform);
	}

	private void ResetButtonRecursively(Transform root) {
		foreach (Transform obj in root) {
			WindowButtons pauseButtonScript = obj.GetComponent<WindowButtons>();
			if (pauseButtonScript)
				pauseButtonScript.ResetButton();

			ResetButtonRecursively(obj);
		}
	}
}

[System.Serializable]
public class GameOverWindowProperties {
	public GameObject gameOverBG;
	public WindowAnim gameOverWindow;

	public GUIText comments;

	[HideInInspector]
	public string[] phrases;

	[HideInInspector]
	public int phraseIndx = 0;

	public void Initialize() {
		gameOverBG.SetActive(false);
		gameOverWindow.gameObject.SetActive(false);

		phrases = new string[4];

		phrases[0] = "You just died a horrible death";
		phrases[1] = "You snooze, you lose!";
		phrases[2] = "You got killed!";
		phrases[3] = "You have been terminated";

		phraseIndx = Random.Range(0, phrases.Length);
	}

	public void ResetButton() {
		ResetButtonRecursively(gameOverWindow.transform);
	}

	private void ResetButtonRecursively(Transform root) {
		foreach (Transform obj in root) {
			WindowButtons pauseButtonScript = obj.GetComponent<WindowButtons>();
			if (pauseButtonScript)
				pauseButtonScript.ResetButton();

			ResetButtonRecursively(obj);
		}
	}
}

[System.Serializable]
public class EndGameWindowProperties {
	public GameObject endGameBG;
	public WindowAnim endGameWindow;

	public GUIText slash;
	public GUIText curGem;
	public GUIText maxGem;
	public GUIText comments;

	[HideInInspector]
	public string[] phrases;

	public GameObject[] stars;
	[HideInInspector]
	public int phraseIndx = 0;

	public void Initialize() {
		endGameBG.SetActive(false);
		endGameWindow.gameObject.SetActive(false);
		comments.gameObject.SetActive(false);

		phrases = new string[2];

		phrases[0] = "Congratulations! \n You just completed the Tutorial Level";
		phrases[1] = "Congratulations!";
	}

	public void ResetButton() {
		ResetButtonRecursively(endGameWindow.transform);
	}

	private void ResetButtonRecursively(Transform root) {
		foreach (Transform obj in root) {
			WindowButtons pauseButtonScript = obj.GetComponent<WindowButtons>();
			if (pauseButtonScript)
				pauseButtonScript.ResetButton();

			ResetButtonRecursively(obj);
		}
	}
}

[System.Serializable]
public class WindowProperties {
	public Transform player;
	public Camera camera;
	public Layers layer;
	public GUIText levelNumberGUI;
	public bool control;

	public ParallaxSpawner spawner;
	public SpawnerProperties spawnerProperties;

	public void InitializeProperties() {
		if(player != null)	InitializePlayer();
		if(spawner != null)	InitializeSpawner();
	}

	private void InitializePlayer() {
		PlayerController playerScript = player.GetComponent<PlayerController>();
		playerScript.SetPlayer(player);
		playerScript.SetCamera(camera);
		playerScript.SetLayer(layer);
		//playerScript.SetIsMoving(false);
		playerScript.SetScrollSpeed(0.0f);
		playerScript.SetPositionOffset(0.0f);
		playerScript.SetStopduration(0.0f);
	}

	private void InitializeSpawner() {		
		if(player != null)		spawner.SetPlayer(player);
		if(camera != null)		spawner.SetCamera(camera);
		spawner.SetLayer(layer);

		spawner.SetScrollSpeed(spawnerProperties.scrollSpeed);
		spawner.SetPositionOffset(spawnerProperties.positionOffset);
		spawner.SetMovementDirection(spawnerProperties.movementDirection);
		spawner.SetPlatformProperties(spawnerProperties.platforms);
	}

	public void UpdateWindow() {
		UpdatePlatformGUI();
		UpdatePlayerControl();
	}

	private void UpdatePlatformGUI() {
		levelNumberGUI.text = "" + (spawner.GetPlatformIndx() - 1);
	}

	private void UpdatePlayerControl() { 
		if(player == null) return;
		player.SendMessage("SetControl", control);
	}

	public bool HasTouchedPortal() {
		if(player != null)
			return player.GetComponent<PlayerController>().GetHasTouchedPortal();
		return false;
	}

	public int GetGemCount() {
		return spawner.GetGemCount();
	}

	public int GetCurGemCount() {
		if(player != null)
			return player.GetComponent<PlayerController>().GetGemCount();
		return 0;
	}

	public bool PlayerIsAlive() {
		if(player != null)
			return true;
		return false;
	}
}

[System.Serializable]
public class SpawnerProperties {
	public float scrollSpeed;
	public float positionOffset;
	public Vector3 movementDirection;
	public PlatformProperties[] platforms;
}

[System.Serializable]
public class PlatformProperties {
	public Transform platform;
	public ObjectPosition stopAt;
	public float stopduration;
}