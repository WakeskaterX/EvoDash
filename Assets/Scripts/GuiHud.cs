using UnityEngine;
using System.Collections;

public class GuiHud : MonoBehaviour {

	public float end_ratio					= 0f;
	public float jump_ratio					= 0f;
	public float speed_ratio				= 0f;
	public float dash_ratio					= 0f;
	public int p_lives	= 0;
	public bool can_dash = false;
	public playerControl pCont;
	public GameObject player;
	public int num_ghosts;
	public float curr_time					= 0f;
	public float start_time					= 0f;

	public Texture end_border_tex;
	public Texture end_bar_tex;
	public Texture end_back_tex;
	public Texture life_tex;
	public Texture empty_tex;
	public Texture dash_tex;
	public Texture nodash_tex;
	public Texture speed_bar_tex;
	public Texture jump_bar_tex;
	public Texture dash_bar_tex;
	public Texture bar_back_tex;
	
	public Texture speed_bar;
	public Texture dash_bar;
	public Texture jump_bar;

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
		jump_ratio = (pCont.jump_force - pCont.jump_force_min)/(pCont.jump_force_max - pCont.jump_force_min);
		speed_ratio = (pCont.speed_top - pCont.speed_min)/(pCont.speed_max - pCont.speed_min);
		dash_ratio = (pCont.dash_cool_max - pCont.dash_cool)/(pCont.dash_cool_max - pCont.dash_cool_min);
		
		can_dash = pCont.can_dash;
		GUI.color = Color.white;
		GUI.DrawTexture (new Rect(10f,0f,128f,64f),end_back_tex);
		GUI.DrawTexture (new Rect(38f,0f, 100f * end_ratio,64f),end_bar_tex);
		GUI.DrawTexture (new Rect(10f,0f,128f,64f),end_border_tex);
		//Draw Bar Backgrounds
		GUI.DrawTexture (new Rect(10f,75f,128f,64f),bar_back_tex);
		GUI.DrawTexture (new Rect(10f,105f,128f,64f),bar_back_tex);
		GUI.DrawTexture (new Rect(10f,135f,128f,64f),bar_back_tex);
		//Draw Bars
		GUI.DrawTexture (new Rect(34f,75f,104f*jump_ratio,64f),jump_bar);
		GUI.DrawTexture (new Rect(34f,105f,104f*dash_ratio,64f),dash_bar);
		GUI.DrawTexture (new Rect(34f,135f,104f*speed_ratio,64f),speed_bar);
		
		
		//Draw Bar Foregrounds
		GUI.DrawTexture (new Rect(10f,75f,128f,64f),jump_bar_tex);
		GUI.DrawTexture (new Rect(10f,105f,128f,64f),dash_bar_tex);
		GUI.DrawTexture (new Rect(10f,135f,128f,64f),speed_bar_tex);

		

		for (int i = 0; i < 3; i++){
			if(i + 1 > p_lives){
				GUI.DrawTexture(new Rect(15f + 25f*i,50f,20f,20f),empty_tex);
			}
			else{
				GUI.DrawTexture(new Rect(15f + 25f*i,50f,20f,20f),life_tex);
			}
		}

		if (can_dash){
			GUI.DrawTexture(new Rect(100f,50f,30f,30f),dash_tex);
		} else GUI.DrawTexture(new Rect(100f,50f,30f,30f),nodash_tex);
		



		curr_time = Mathf.Floor (Time.time - start_time);
		player.GetComponent<playerControl>().gametimer = (int)curr_time;
		GUI.color = Color.black;
		GUI.Label (new Rect(Screen.width-50f,20f,40f,20f),curr_time.ToString());

		//GUI.Box (new Rect(5f,5f,10f,12f),"BOX");
	}
}
