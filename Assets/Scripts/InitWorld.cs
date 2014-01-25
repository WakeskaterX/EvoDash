using UnityEngine;
using System.Collections;

public class InitWorld : MonoBehaviour {

	public GameObject platform;

	// Use this for initialization
	void Start () {
	
	}

	void Awake() {
		GameObject newObject = (GameObject) Instantiate (platform, Vector3.zero, Quaternion.identity);
		newObject.transform.localScale = new Vector3 (2, 1, 1);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
