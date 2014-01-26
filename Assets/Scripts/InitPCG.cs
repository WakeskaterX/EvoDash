using UnityEngine;
using System.Collections;

public class InitPCG : MonoBehaviour {

	public GameObject platform;
	public GameObject nextTrigger;
	public GameObject player;
	public GameObject spikes;
	public GameObject ground;
	public GameObject air;
	public GameObject ghost;


	// Use this for initialization
	void Start () {
		
		Vector3 curLocation =  new Vector3(5,0,0);
		Vector3 triggerLoc = Vector3.zero;
		Instantiate (platform, new Vector3(curLocation.x, curLocation.y, 0f), Quaternion.identity);
		/*for (int i = 0; i < 10; i++)
		{
			curLocation.x += 1.5f;
			Instantiate (platform, new Vector3(curLocation.x, curLocation.y, 0f), Quaternion.identity);
			if (i == 5) 
			{
				triggerLoc = curLocation;
				triggerLoc.y += 2f;
			}
			Instantiate (spikes,new Vector3( curLocation.x, curLocation.y-2, 0f), Quaternion.identity);
		}*/
		Generator.numberGenerated = 0;
		Generator.player = player;
		Generator.spikes = spikes;
		Generator.ground = ground;
		Generator.air = air;
		Generator.ghost = ghost;
		/*
		GameObject nTrigger = (GameObject) Instantiate (nextTrigger, triggerLoc, gameObject.transform.rotation);
		nTrigger.GetComponent<MakeNextChunk> ().platform = platform;
		nTrigger.GetComponent<MakeNextChunk> ().nextTrigger = nextTrigger;
		nTrigger.GetComponent<MakeNextChunk> ().startLocation = curLocation;
		nTrigger.GetComponent<MakeNextChunk> ().player = player;
		*/
		Generator.ClearPaths();
		Generator.AddPath (curLocation);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
