using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

	public Texture main_tex;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton ("Enter")) {
			ScoreHold.max_dist = 0;
			ScoreHold.life_time = 0;
			Application.LoadLevel ("Level_One");
		}
	}

	void OnGUI(){
		GUI.DrawTexture (new Rect(-20f,-5f,Screen.width + 35f ,Screen.height + 15f),main_tex);
		//GUI.Label(new Rect(Screen.width/2-200,Screen.height/2+100,400,200),"Evo Dash");

		//GUI.Label (new Rect(Screen.width/2-200,Screen.height/2-100,400,200),"Press Enter");
	}
}
