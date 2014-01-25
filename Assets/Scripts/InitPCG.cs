using UnityEngine;
using System.Collections;

public class InitPCG : MonoBehaviour {

	public GameObject platform;
	public GameObject nextTrigger;

	// Use this for initialization
	void Start () {
		
		Vector3 curLocation = Vector3.zero;
		Vector3 triggerLoc = Vector3.zero;
		Instantiate (platform, new Vector3(curLocation.x, curLocation.y, 0f), Quaternion.identity);
		for (int i = 0; i < 10; i++)
		{
			curLocation.x += 1.5f;
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
