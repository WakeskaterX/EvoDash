using UnityEngine;
using System.Collections;

public class GhostSpawner : MonoBehaviour {

	public GameObject player;
	public GameObject ghost;
	public float 	spawn_timer				= 0f;
	public float 	spawn_interval			= 10f;

	void Start () {
		spawn_timer = Time.time + spawn_interval;
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time > spawn_timer)
		{
			spawn_timer = Time.time + spawn_interval;
			Vector3 spawn_loc = Camera.main.ScreenToWorldPoint(new Vector3(.5f,0f+Random.Range (0f,100f),20f));
			GameObject enGhost = Object.Instantiate (ghost,spawn_loc,Quaternion.identity) as GameObject;
			enGhost.GetComponent<EnemyGhost>().player = player;
		}
	
	}
}
