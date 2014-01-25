using UnityEngine;
using System.Collections;

public class MakeNextChunk : MonoBehaviour {

	public GameObject platform;
	public GameObject nextTrigger;
	public bool madeNextChunk = false;

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
		if (madeNextChunk)
						return;
		Instantiate (platform, new Vector3(gameObject.transform.position.x +1.28f, 0, 0), Quaternion.identity);
		GameObject nTrigger = (GameObject) Instantiate (nextTrigger, new Vector3(gameObject.transform.position.x +1.28f, gameObject.transform.position.y, 0), gameObject.transform.rotation);
		nTrigger.GetComponent<MakeNextChunk> ().platform = platform;
		nTrigger.GetComponent<MakeNextChunk> ().nextTrigger = nextTrigger;
		madeNextChunk = true;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 255);
	}
}
