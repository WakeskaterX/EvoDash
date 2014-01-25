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
	public float dash_time			= 0f;
	public float dash_cool_min		= 3f;
	public float dash_cool_max		= 6f;
	public float dash_cool			= 6f;
	public float dash_cool_time		= 0f;

	public float dash_speed			= 10f;
	public float dash_dampen		= .2f;

	public float wall_slide_speed	= 1f;

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
	public bool wallgrabbing		= false;

	public Transform ground_check;
	public Transform wall_check_left;
	public Transform wall_check_right;

	public LayerMask is_ground;

	public bool tap_right			= false;
	public bool tap_left			= false;
	public float tap_time 			= 0f;
	public float tap_cool			= .1f;								//time to double tap in seconds
	public float tap_last           = 0f;

	public float grav				= 2.5f;

	void Start () {
	
	}

	void Update(){

		if (!dashing){
			MoveWallGrab ();

			if (!wallgrabbing)  MoveJump ();
		}

		MoveDash();
		
	}

	void FixedUpdate () {

		if (!dashing){
			MoveHoriz();
		}
	
	}

	void MoveWallGrab(){

		if (!grounded){
			if (Input.GetAxis ("Horizontal") < 0 && Physics2D.Linecast (transform.position,wall_check_left.position,is_ground)){
				wallgrabbing = true;
			}else if (Input.GetAxis ("Horizontal") > 0 && Physics2D.Linecast (transform.position,wall_check_right.position,is_ground)){
				wallgrabbing = true;
			}else wallgrabbing = false;
		} else wallgrabbing = false;

		if (wallgrabbing)
		{
			if (Mathf.Abs (rigidbody2D.velocity.y) > wall_slide_speed)
			{
				rigidbody2D.velocity = new Vector2(0,wall_slide_speed);
			}
		}
	}

	void MoveDash(){
	if (grounded){
		if (!tap_right && Input.GetAxis ("Horizontal") > 0 && tap_last == 0) {
			tap_time = Time.time + tap_cool;
			tap_right = true;
			//Debug.Log("tap right");
		}
		else if (tap_right && Input.GetAxis ("Horizontal") > 0 && tap_last == 0 && !dashing && can_dash) {
			dashing = true;
			rigidbody2D.velocity = new Vector2 (dash_speed,0);
			dash_time = Time.time + dash_len;
			dash_cool_time = Time.time + dash_cool;
			rigidbody2D.gravityScale = 0f;
			can_dash = false;
			//Debug.Log("dash right");
		}

		if (!tap_left && Input.GetAxis ("Horizontal") < 0 && tap_last == 0) {
			tap_time = Time.time + tap_cool;
			tap_left = true;
			//Debug.Log("tap left");
		}
		else if (tap_left && Input.GetAxis ("Horizontal") < 0 && tap_last == 0 && !dashing && can_dash) {
			dashing = true;
			rigidbody2D.velocity = new Vector2 (-dash_speed,0);
			dash_time = Time.time + dash_len;
			dash_cool_time = Time.time + dash_cool;
			rigidbody2D.gravityScale = 0f;
			can_dash = false;
			//Debug.Log("dash left");
		}}

		if(Time.time > tap_time) {
			tap_left = false;
			tap_right = false;
		}

		if (Time.time > dash_time && dashing){
			dashing = false;
			rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x * dash_dampen,rigidbody2D.velocity.y);
			rigidbody2D.gravityScale = grav;
		}

		if (Time.time > dash_cool_time && !can_dash){
			can_dash = true;
		}

		tap_last = Input.GetAxis ("Horizontal");
	}

	void MoveHoriz(){
		float h = Input.GetAxis ("Horizontal");
		float velX = Mathf.Sign (rigidbody2D.velocity.x) * (Mathf.Max (Mathf.Abs (rigidbody2D.velocity.x) - decel_force, 0));
		
		if (Input.GetButton ("Sprint") && can_sprint){
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

	void MoveJump(){
		bool jump = Input.GetButtonDown("Jump");
		
		grounded = Physics2D.OverlapArea (new Vector2(transform.position.x-.1f,transform.position.y), new Vector2(ground_check.position.x+.1f,ground_check.position.y), 1 << LayerMask.NameToLayer ("Platform"));
		
		if (grounded && jump && can_jump){
			rigidbody2D.AddForce(Vector2.up * jump_force * 100);
			grounded = false;
		}
	}
}
