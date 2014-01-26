using UnityEngine;
using System.Collections;

public class PlatformDestruction : MonoBehaviour {

	public GameObject player;
	public float dist_to_destroy		= 25f;
	// Use this for initialization
	void Start () {
		if (player == null) player = GameObject.FindWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if ((player.transform.position.x - transform.position.x) > dist_to_destroy)
		{
			Debug.Log ("Destroyed a Platform");
			Destroy(this.gameObject);
		}
	}
}
