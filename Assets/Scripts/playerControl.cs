using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {

	public float speed_min			= 2f;
	public float speed_max			= 6f;
	public float speed_curr			= 1f;
	public float speed_top			= 2f;								//current top speed available
	public float sprint_mult		= 1f;								//multiplies the speed_top
	public float sprint_max			= 1.8f;

	public float jump_force_min		= 5f;
	public float jump_force_max		= 10f;
	public float jump_force			= 5f;

	public float accel_force		= 45f;
	public float decel_force		= .2f;

	public float dash_len_min		= .3f;
	public float dash_len_max		= .8f;
	public float dash_len			= .3f;
	public float dash_time			= 0f;
	public float dash_cool_min		= .5f;
	public float dash_cool_max		= 4f;
	public float dash_cool			= 4f;
	public float dash_cool_time		= 0f;

	public float dash_speed			= 15f;
	public float dash_dampen		= .6f;

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
	public bool can_dub_jump		= false;
	public bool can_wall_jump		= true;
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

	public int	wall_side			= 0;

	public LayerMask is_ground;

	public bool tap_right			= false;
	public bool tap_left			= false;
	public float tap_time 			= 0f;
	public float tap_cool			= .45f;								//time to double tap in seconds
	public float tap_last           = 0f;

	public float grav				= 2.5f;

	public PlayerData data = new PlayerData();

	public bool facing_right		= true;
	public bool stunned				= false;
	public float stun_time			= 0f;
	public float stun_dur			= 5f;

	Animator anim;


	/************  Statistic Adjustment Variables  ************/
	public float speed_decay		= .1f;								//decay in seconds
	public float speed_inc			= .3f;								//time sprinting in seconds
	public float jump_decay			= .1f;
	public float jump_inc			= .5f;
	public float dash_cool_decay	= .05f;
	public float dash_cool_inc		= .3f;
	public float end_reg_decay		= .02f;
	public float end_reg_inc		= .03f;
	public float dash_len_decay		= .03f;
	public float dash_len_inc		= .08f;





	void Start () {
		anim = GetComponent<Animator>();
	}

	void Update(){

		if (!dashing){
			MoveWallGrab ();
			MoveJump ();
		}

		EndureRegen();
		EndureMin ();
		EndureReset();

		MoveDash();
		anim.SetBool ("dashing",dashing);

		float move_dir = rigidbody2D.velocity.x;

		if (move_dir >= .1 && !facing_right){
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
			facing_right = true;
		} else if (move_dir <= -.1 && facing_right){
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
			facing_right = false;
		}

		if (endure < endure_max/2 && !stunned){endure_regen += end_reg_inc * Time.deltaTime;}

		DecayValues();
		CapValues();
		data.PlayerCurrentStats (jump_force,dash_len,speed_top);
	}

	void FixedUpdate () {

		if (!dashing){
			MoveHoriz();
		}

		anim.SetFloat ("hSpeed",Mathf.Abs (rigidbody2D.velocity.x));
		anim.SetFloat ("vSpeed",rigidbody2D.velocity.y);
		CapValues ();
	}

	void OnGUI(){
		DevInfoGraphics();
	}

	void EndureRegen(){
		if (endure < endure_max){
			endure += endure_regen * Time.deltaTime;
		}else endure = endure_max;

		if (endure > endure_max) endure = endure_max;
	}

	void EndureReset(){
		if (Time.time > stun_time && stunned) stunned = false;
	}

	void EndureMin(){
		if (endure < 0) {
			endure = 0;
			stunned = true;
			stun_time = Time.time + stun_dur;
		}
	}

	void MoveWallGrab(){

		if (!grounded && !stunned){
			if (Physics2D.Linecast (transform.position,wall_check_right.position,is_ground ) ||Physics2D.Linecast (transform.position,wall_check_left.position,is_ground )){
				if (Input.GetAxis ("Horizontal") < 0){
					wallgrabbing = true;
					wall_side = -1;
				}else
				if (Input.GetAxis ("Horizontal") > 0){
					wallgrabbing = true;
					wall_side = 1;
				}
			}else wallgrabbing = false;
		}else wallgrabbing = false;

		if (wallgrabbing)
		{
			endure -= wall_grab_cost * Time.deltaTime;
			if (Mathf.Abs (rigidbody2D.velocity.y) > wall_slide_speed)
			{
				rigidbody2D.velocity = new Vector2(0f,wall_slide_speed);
			}
		} else wall_side = 0;


		anim.SetBool ("wallGrab", wallgrabbing);
	}

	void DecayValues(){
		speed_top -= speed_decay * Time.deltaTime;
		if (speed_top < speed_min) speed_top = speed_min;

		jump_force -= jump_decay * Time.deltaTime;
		if (jump_force < jump_force_min) jump_force = jump_force_min;

		dash_cool += dash_cool_decay * Time.deltaTime;
		if (dash_cool > dash_cool_max) dash_cool = dash_cool_max;

		endure_regen -= end_reg_decay * Time.deltaTime;
		if (endure_regen < endure_regen_min) endure_regen = endure_regen_min;

		dash_len -= dash_len_decay * Time.deltaTime;
		if (dash_len < dash_len_min) dash_len = dash_len_min;
	}
	void CapValues(){
		speed_top = Mathf.Min(speed_top,speed_max);
		jump_force = Mathf.Min (jump_force,jump_force_max);
		dash_cool = Mathf.Max(dash_cool,dash_cool_min);
		endure_regen = Mathf.Min (endure_regen,endure_regen_max);
		dash_len = Mathf.Min (dash_len,dash_len_max);
	}

	void MoveDash(){
	if ((grounded || can_airdash) && !stunned){
		if (!tap_right && Input.GetAxis ("Horizontal") > 0 && tap_last == 0) {
			tap_time = Time.time + tap_cool;
			tap_right = true;
			//Debug.Log("tap right");
		}
		else if (tap_right && Input.GetAxis ("Horizontal") > 0 && tap_last == 0 && !dashing && can_dash) {
			dashing = true;
			endure -= dash_cost;
			dash_cool -= dash_cool_inc;
				dash_len += dash_len_inc;
			data.PlayerDashed();
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
			endure -= dash_cost;
			dash_cool -= dash_cool_inc;
			data.PlayerDashed();
			rigidbody2D.velocity = new Vector2 (-dash_speed,0);
			dash_time = Time.time + dash_len;
			dash_cool_time = Time.time + dash_cool;
				dash_len += dash_len_inc;
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

		if (tap_last != 0 && Input.GetAxis ("Horizontal") == 0)
		{
			if (tap_last < 0 && tap_right) tap_right = false;

			if (tap_last > 0 && tap_left) tap_left = false;
		}

		tap_last = Input.GetAxis ("Horizontal");
	}

	void MoveHoriz(){
		float h = Input.GetAxis ("Horizontal");
		float velX = 0f;

		if (Input.GetButton ("Sprint") && can_sprint && !stunned){
			data.AddRunningTime(Time.deltaTime);
			sprint_mult = sprint_max;
			speed_top += speed_inc * Time.deltaTime;
			endure -= sprint_cost*Time.deltaTime;
			//Debug.Log ("Sprinting");
		} else sprint_mult = 1f;

		//Dampen Move Speed
		if (grounded){
			velX = Mathf.Sign (rigidbody2D.velocity.x) * (Mathf.Max (Mathf.Abs (rigidbody2D.velocity.x) - decel_force, 0));
			rigidbody2D.velocity = new Vector2 (velX, rigidbody2D.velocity.y);
		} else 
		{
			data.AddAirTime(Time.deltaTime);
		}
		
		rigidbody2D.AddForce (Vector2.right * h * accel_force * sprint_mult);
		
		if (Mathf.Abs(rigidbody2D.velocity.x) > (speed_top * sprint_mult)){ 
			rigidbody2D.velocity = new Vector2(Mathf.Sign (rigidbody2D.velocity.x) * (speed_top * sprint_mult), rigidbody2D.velocity.y);
			Debug.Log ("Top Speed reached!");
		}
	}

	void MoveJump(){
		bool jump = Input.GetButtonDown("Jump");
		float jForce = 2.2f;
		float jFWall = 200f;

		if (!wallgrabbing){
			grounded = Physics2D.OverlapArea (new Vector2(transform.position.x-.1f,transform.position.y), new Vector2(ground_check.position.x+.1f,ground_check.position.y), 1 << LayerMask.NameToLayer ("Platform"));
		
			if (grounded) {
				can_jump = true;
			}

			if (grounded && jump && can_jump && !stunned){
			//rigidbody2D.AddForce(Vector2.up * jump_force * jForce);
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,jump_force * jForce );
				endure -= jump_cost;
				jump_force += jump_inc;
			grounded = false;
			if (!can_dub_jump) can_jump = false;
			}
		else
			if (!grounded && jump && can_jump && !stunned){
				can_jump = false;
				jump_force += jump_inc;
				endure -= dub_jump_cost;
				//rigidbody2D.AddForce(Vector2.up * jump_force * jForce);
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,jump_force * jForce );
			}
		}
		else{
			if (jump && can_wall_jump && !stunned)
			{
				float jvX, jvY;
				Vector2 jVect;
				jvX = wall_side * -1f *  2f;
				jvY = 1f;
				jVect = new Vector2(jvX,jvY);
				jVect.Normalize();
				data.PlayerJumped();
				wallgrabbing = false;
				endure -= jump_cost;
				jump_force += jump_inc;
				//rigidbody2D.velocity = jVect * jump_force * jFWall ;
				rigidbody2D.AddForce (jVect * jump_force * jFWall);
				Debug.Log (rigidbody2D.velocity);

				if (!can_dub_jump)	can_jump = false;
			}
		}

		anim.SetBool ("grounded",grounded);
	}

	void DevInfoGraphics(){
		GUI.contentColor = Color.black;
		GUI.Label(new Rect(10,30,200,20),"Speed: "+speed_top);
		GUI.Label(new Rect(10,60,200,20),"Jump: "+jump_force);
		GUI.Label(new Rect(10,90,200,20),"Dash CD: "+dash_cool);
		GUI.Label(new Rect(10,120,200,20),"Dash Len: "+dash_len);
		GUI.Label(new Rect(10,150,200,20),"End Regen: "+endure_regen);
	}
}
