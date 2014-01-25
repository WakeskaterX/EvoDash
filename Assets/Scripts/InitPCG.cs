using UnityEngine;
using System.Collections;

public class InitPCG : MonoBehaviour {

	public GameObject platform;
	public GameObject nextTrigger;

	// Use this for initialization
	void Start () {
		
		Vector3 curLocation = Vector3.zero;
		Instantiate (platform, new Vector3(curLocation.x, curLocation.y, 0f), Quaternion.identity);
		for (int i = 0; i < 10; i++)
		{
			curLocation.x += 1.28f;
			curLocation.y -=1;
			Instantiate (platform, new Vector3(curLocation.x, curLocation.y, 0f), Quaternion.identity);
		}
		GameObject nTrigger = (GameObject) Instantiate (nextTrigger, new Vector3(curLocation.x-1.28f*5f, gameObject.transform.position.y-5f, 1), gameObject.transform.rotation);
		nTrigger.GetComponent<MakeNextChunk> ().platform = platform;
		nTrigger.GetComponent<MakeNextChunk> ().nextTrigger = nextTrigger;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
