﻿using UnityEngine;
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
		Vector3 curLocation = gameObject.transform.position;
		curLocation.x += 1.28f * 5f;
		curLocation.y -= 1f * 5f;
		for (int i = 0; i < 10; i++)
		{
			curLocation.x += 1.28f;
			curLocation.y -=1;
			Instantiate (platform, new Vector3(curLocation.x, curLocation.y, 0f), Quaternion.identity);
		}
		GameObject nTrigger = (GameObject) Instantiate (nextTrigger, new Vector3(curLocation.x-1.28f*5f, gameObject.transform.position.y-10f, 1), gameObject.transform.rotation);
		nTrigger.GetComponent<MakeNextChunk> ().platform = platform;
		nTrigger.GetComponent<MakeNextChunk> ().nextTrigger = nextTrigger;
		madeNextChunk = true;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 255);
	}
}
