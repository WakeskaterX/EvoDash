using UnityEngine;
using System.Collections;

public class EndCredits : MonoBehaviour {

	public Texture death;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButton("Enter"))
		{
			Application.LoadLevel("Title");
		}
	}

	void OnGUI(){
		GUI.DrawTexture(new Rect(Screen.width/3,Screen.height/4,Screen.width/4,Screen.height/12),death,ScaleMode.StretchToFill);

		GUI.Label (new Rect(Screen.width/2-100,Screen.height/2-50,200,50),"Total Time: " + ScoreHold.life_time);
		GUI.Label (new Rect(Screen.width/2-100,Screen.height/2+50,200,50),"Maximum Distance: " + Mathf.FloorToInt (ScoreHold.max_dist));
		GUI.Label (new Rect(Screen.width/2-100,Screen.height/2+200,200,50),"Press Enter to Restart");
	}
}
