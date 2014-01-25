using UnityEngine;
using System.Collections;

public class InitWorld : MonoBehaviour {

	public GameObject platform;

	// Use this for initialization
	void Start () {

	}

	void Awake() {

	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Random.seed = (int)Time.fixedTime;
		GameObject newObject = (GameObject) Instantiate (platform, new Vector3(Random.Range(-10, 10), 0, 0), Quaternion.identity);
		newObject.transform.localScale = new Vector3 (2, 1, 1);
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 255);
	}
}
