using UnityEngine;
using System.Collections;

public class MakeNextChunk : MonoBehaviour {

	public GameObject platform;
	public GameObject nextTrigger;
	public GameObject player;
	public bool madeNextChunk = false;
	public Vector3 startLocation;

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
		PlayerDataCapsule data = player.GetComponent<playerControl> ().data.giveData ();
		/*
		if (madeNextChunk)
						return;
		Vector3 curLocation = startLocation;
		Vector3 triggerLoc = Vector3.zero;
		Instantiate (platform, new Vector3(curLocation.x, curLocation.y, 0f), Quaternion.identity);
		for (int i = 0; i < 10; i++)
		{
			curLocation.x +=1.5f;
			curLocation.y +=data.numberJumps/10f;
			print(data.numberJumps);
			Instantiate (platform, new Vector3(curLocation.x, curLocation.y, 0f), Quaternion.identity);
			if (i == 5) 
			{
				triggerLoc = curLocation;
				triggerLoc.y += 2f;
			}
		}
		GameObject nTrigger = (GameObject) Instantiate (nextTrigger, triggerLoc, gameObject.transform.rotation);
		nTrigger.GetComponent<MakeNextChunk> ().platform = platform;
		nTrigger.GetComponent<MakeNextChunk> ().nextTrigger = nextTrigger;
		nTrigger.GetComponent<MakeNextChunk> ().startLocation = curLocation;
		nTrigger.GetComponent<MakeNextChunk> ().player = player;
		*/
		Generator.MakeChunk (data, platform, nextTrigger);
		madeNextChunk = true;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 255);
	}
}
