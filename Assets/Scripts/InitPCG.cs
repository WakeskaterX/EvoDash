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

		//Create Starting Area for the Player
		GameObject play = Instantiate (player, new Vector3(0,1,0),Quaternion.identity) as GameObject;

		GameObject plat = Instantiate (platform,new Vector3(0 , 0 , 0), Quaternion.identity) as GameObject;
		plat.GetComponent<PlatformDestruction>().player = play;
		Vector3 tempScale = plat.transform.localScale;
		tempScale = new Vector3(tempScale.x * 32, tempScale.y,tempScale.z);
		plat.transform.localScale = tempScale;



		for (int i = -4; i < 4; i ++){
			GameObject spk = Instantiate(spikes, new Vector3(i*2,-2,0), Quaternion.identity) as GameObject;
			spk.GetComponent<PlatformDestruction>().player = player;
		}

		GameObject wall = Instantiate(platform,new Vector3(-8f,7f,0f),Quaternion.identity) as GameObject;
		wall.GetComponent<PlatformDestruction>().player = play;
		tempScale = wall.transform.localScale;
		tempScale = new Vector3(tempScale.x, tempScale.y*36,tempScale.z);
		wall.transform.localScale = tempScale;

		plat = Instantiate (platform,new Vector3(0f, 15.5f, 0f), Quaternion.identity) as GameObject;
		plat.GetComponent<PlatformDestruction>().player = play;
		tempScale = plat.transform.localScale;
		tempScale = new Vector3(tempScale.x * 32, tempScale.y,tempScale.z);
		plat.transform.localScale = tempScale;

		plat = Instantiate (platform,new Vector3(7.5f, 15f, 0f), Quaternion.identity) as GameObject;
		plat.GetComponent<PlatformDestruction>().player = play;

		plat = Instantiate (platform,new Vector3(0f, 8f, 0f), Quaternion.identity) as GameObject;
		plat.GetComponent<PlatformDestruction>().player = play;
		tempScale = plat.transform.localScale;
		tempScale = new Vector3(tempScale.x * 8, tempScale.y,tempScale.z);
		plat.transform.localScale = tempScale;


		//Instantiate (platform, new Vector3(curLocation.x, curLocation.y, 0f), Quaternion.identity);
		player = play;

		Generator.numberGenerated = 0;
		Generator.player = player;
		Generator.spikes = spikes;
		Generator.ground = ground;
		Generator.air = air;
		Generator.ghost = ghost;

		Generator.roomLocation = new Vector3(8f,0f,0f);

		GameObject trig = GameObject.Instantiate(nextTrigger,new Vector3(2f,0f,0f),Quaternion.Euler (0,0,90)) as GameObject;
		trig.GetComponent<MakeNextChunk>().player = player;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
