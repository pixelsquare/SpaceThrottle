using UnityEngine;
using System.Collections;

public class GemSpawner : MonoBehaviour {
	public string layer;
	public GameObject[] gems;

	private int randomNum;

	private void Start() {
		GameObject gemInstance = (GameObject)Instantiate(GetRandomGem(), transform.position, Quaternion.identity);
		gemInstance.layer = gameObject.layer;
		gemInstance.name = gems[randomNum].name;
		gemInstance.transform.parent = transform.parent;

		Destroy(this.gameObject);
	}

	private GameObject GetRandomGem() {
		randomNum = Random.Range(0, gems.Length);
		return gems[randomNum];
	}
}
