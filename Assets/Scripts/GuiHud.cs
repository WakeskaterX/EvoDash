using UnityEngine;
using System.Collections;

public class GuiHud : MonoBehaviour {

	public float end_ratio					= 0f;
	public int p_lives	= 0;
	public bool can_dash = false;
	public playerControl pCont;
	public GameObject player;
	public int num_ghosts;
	public int counter						= 0;
	public float count_time					= 0f;

	public Texture border_tex;
	public Texture bar_tex;
	public Texture life_tex;
	public Texture empty_tex;
	public Texture dash_tex;
	public Texture nodash_tex;

	void Start () {
		pCont = player.GetComponent<playerControl>();
	}
	void Update () { 
		count_time += 1f;

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

		counter = Mathf.FloorToInt(count_time * Time.deltaTime);
		GUI.color = Color.black;
		GUI.Label (new Rect(Screen.width-50f,20f,40f,20f),counter.ToString());

		//GUI.Box (new Rect(5f,5f,10f,12f),"BOX");
	}
}
