using UnityEngine;
using System.Collections;

public class DizzyScript : MonoBehaviour {

	public GameObject player;
	public playerControl pC;
	public float rotSpd						= .2f;
	// Use this for initialization
	void Start () {
		pC = player.GetComponent<playerControl>();
	}
	
	// Update is called once per frame
	void Update () {
		if (pC.stunned){
			renderer.enabled = true;
		} else renderer.enabled = false;
	
		transform.Rotate (new Vector3(0,0,rotSpd));
	}
}
