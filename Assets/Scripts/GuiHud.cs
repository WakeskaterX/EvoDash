using UnityEngine;
using System.Collections;

public class GuiHud : MonoBehaviour {

	public float end_ratio					= 0f;
	public int p_lives	= 0;
	public playerControl pCont;
	public GameObject player;

	public Texture border_tex;
	public Texture bar_tex;
	public Texture life_tex;

	void Start () {
		pCont = player.GetComponent<playerControl>();
	}
	void Update () {
	}

	void OnGUI() {
		p_lives = pCont.lives;
		end_ratio = pCont.endure/pCont.endure_max;
		GUI.DrawTexture (new Rect(10f,10f,150f,18f),border_tex);
		GUI.DrawTexture (new Rect(12f,12f, 146f * end_ratio,14f),bar_tex);

		//for (


		//GUI.Box (new Rect(5f,5f,10f,12f),"BOX");
	}
}
