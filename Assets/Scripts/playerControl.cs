using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {
	
	public float speed_min			= 6f;
	public float speed_max			= 15f;
	public float speed_curr			= 1f;
	public float speed_top			= 15f;								//current top speed available
	public float sprint_mult		= 1f;								//multiplies the speed_top
	public float sprint_max			= 1.5f;
	
	public float jump_force_min		= 2f;
	public float jump_force_max		= 4f;
	public float jump_force			= 2f;
	
	public float accel_force		= 100f;
	public float decel_force		= 2f;
	
	public float dash_len_min		= 1f;
	public float dash_len_max		= 2f;
	public float dash_len			= 1f;
	public float dash_speed			= 10f;
	public float dash_cool_min		= 3f;
	public float dash_cool_max		= 6f;
	public float dash_cool			= 6f;
	
	public float endure_max			= 100f;
	public float endure				= 100f;
	public float endure_regen		= 1f;
	public float endure_regen_min	= 1f;
	public float endure_regen_max	= 3f;
	public float endure_cool		= 1f;								//cooldown when you hit 0 endurance
	
	public float sprint_cost		= .1f;
	public float dash_cost			= 10f;
	public float jump_cost			= 2f;
	public float dub_jump_cost		= 5f;
	public float wall_grab_cost		= .2f;
	
	public int lives				= 3;
	
	public bool can_jump			= true;
	public bool can_dub_jump		= true;
	public bool can_dash			= true;
	public bool can_airdash			= true;
	public bool can_sprint			= true;
	public bool can_wall_grab		= true;
	
	public bool grounded			= true;
	public bool sprinting			= false;
	public bool dashing				= false;
	
	public Transform ground_check;
	
	public LayerMask is_ground;
	
	public bool tap_right			= false;
	public bool tap_left			= false;
	public float tap_time 			= 0f;
	public float tap_cool			= .1f;								//time to double tap in seconds
	public float last_move			= 0f;

	public PlayerData data = new PlayerData();

	void Start () {
		
	}
	
	void Update(){
		bool jump = Input.GetButtonDown("Jump");
		
		grounded = Physics2D.OverlapArea (new Vector2(transform.position.x-.1f,transform.position.y), new Vector2(ground_check.position.x+.1f,ground_check.position.y), 1 << LayerMask.NameToLayer ("Platform"));

		if (!grounded)
			data.AddAirTime (Time.deltaTime);

		if (grounded && jump && can_jump){
			data.PlayerJumped();
			rigidbody2D.AddForce(Vector2.up * jump_force * 100);
			grounded = false;
		}

		DashCheck();
		
	}
	
	void FixedUpdate () {
		
		float h = Input.GetAxis ("Horizontal");
		float velX = Mathf.Sign (rigidbody2D.velocity.x) * (Mathf.Max (Mathf.Abs (rigidbody2D.velocity.x) - decel_force, 0));
		
		if (Input.GetButton ("Sprint") && can_sprint){
			data.AddRunningTime(Time.deltaTime);
			sprint_mult = sprint_max;
			Debug.Log ("Sprinting");
		} else sprint_mult = 1f;
		
		rigidbody2D.velocity = new Vector2 (velX, rigidbody2D.velocity.y);
		
		rigidbody2D.AddForce (Vector2.right * h * accel_force * sprint_mult);
		
		if (Mathf.Abs(rigidbody2D.velocity.x) > (speed_top * sprint_mult)){ 
			rigidbody2D.velocity = new Vector2(Mathf.Sign (rigidbody2D.velocity.x) * (speed_top * sprint_mult), rigidbody2D.velocity.y);
			Debug.Log ("Top Speed reached!");
		}

	}

	void DashCheck(){
		Debug.Log ("Dash Check");
		if (!tap_right && Input.GetAxis("Horizontal") > 0 && last_move == 0){
			tap_right = true;
			tap_time = Time.time + tap_cool;
			Debug.Log ("Tap Right");
		}
		else if (tap_right && Input.GetAxis ("Horizontal") > 0 && last_move == 0){
			dashing = true;
			data.PlayerDashed ();
			Debug.Log ("Dash Right");
		}
		
		if (!tap_left && Input.GetAxis("Horizontal") < 0 && last_move == 0){
			tap_left = true;
			tap_time = Time.time + tap_cool;
			Debug.Log ("Tap Left");
		}
		else if (tap_left && Input.GetAxis ("Horizontal") < 0 && last_move == 0){
			dashing = true;
			data.PlayerDashed ();
			Debug.Log ("Dash Left");
		}

		if (Time.time >= tap_time){
			tap_right = false;
			tap_left = false;
		}
		last_move = Input.GetAxis ("Horizontal");
	}

	void OnApplicationQuit() {
		print (data.ToString());
	}

}

