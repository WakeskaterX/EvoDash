using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public Vector3 lastLoc = Vector3.zero; 
	public Vector3 initLocal = Vector3.zero;
	public GameObject player;
	private Vector3 movingFrom;
	private int lerpCompletion;

	// Use this for initialization
	void Start () {
		lastLoc = gameObject.transform.position;
		initLocal = gameObject.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 camLoc = Camera.main.transform.position;
		if (lerpCompletion > 0 && lerpCompletion < 100)
		{
			lerpCompletion++;
			gameObject.transform.localPosition = Vector3.Lerp (movingFrom, initLocal, lerpCompletion/100);
		}
		else if (player.GetComponent<playerControl> ().grounded)
		{
			movingFrom = gameObject.transform.localPosition;
			lerpCompletion = 1;
			gameObject.transform.localPosition = Vector3.Lerp (movingFrom, initLocal, lerpCompletion/100);
			lastLoc = gameObject.transform.position;
		}
		else
			Camera.main.transform.position = new Vector3 (camLoc.x, lastLoc.y, camLoc.z);
	}
}
