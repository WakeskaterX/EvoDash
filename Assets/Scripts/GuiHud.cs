﻿using UnityEngine;
using System.Collections;

public class GuiHud : MonoBehaviour {

	public float end_ratio					= 0f;
	public int p_lives	= 0;
	public bool can_dash = false;
	public playerControl pCont;
	public GameObject player;
	public int num_ghosts;
	public float curr_time					= 0f;
	public float start_time					= 0f;

	public Texture border_tex;
	public Texture bar_tex;
	public Texture life_tex;
	public Texture empty_tex;
	public Texture dash_tex;
	public Texture nodash_tex;

	void Start () {
		pCont = player.GetComponent<playerControl>();
		start_time = Time.time;
	}
	void Update () { 

		if (Input.GetButton ("Exit")){Application.Quit();}
	}

	void OnGUI() {
		p_lives = pCont.lives;
		end_ratio = pCont.endure/pCont.endure_max;
		can_dash = pCont.can_dash;
		GUI.color = Color.white;
		GUI.DrawTexture (new Rect(10f,10f,150f,18f),border_tex);
		GUI.DrawTexture (new Rect(12f,12f, 146f * end_ratio,14f),bar_tex);

	

		for (int i = 0; i < 3; i++){
			if(i + 1 > p_lives){
				GUI.DrawTexture(new Rect(15f + 25f*i,30f,20f,20f),empty_tex);
			}
			else{
				GUI.DrawTexture(new Rect(15f + 25f*i,30f,20f,20f),life_tex);
			}
		}

		if (can_dash){
			GUI.DrawTexture(new Rect(100f,30f,30f,30f),dash_tex);
		} else GUI.DrawTexture(new Rect(100f,30f,30f,30f),nodash_tex);

		curr_time = Mathf.Floor (Time.time - start_time);
		player.GetComponent<playerControl>().gametimer = (int)curr_time;
		GUI.color = Color.black;
		GUI.Label (new Rect(Screen.width-50f,20f,40f,20f),curr_time.ToString());

		//GUI.Box (new Rect(5f,5f,10f,12f),"BOX");
	}
}
