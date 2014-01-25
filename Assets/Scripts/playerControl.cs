using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {

	public float speed_min			= 6f;
	public float speed_max			= 10f;
	public float speed_curr			= 1f;
	public float speed_top			= 6f;								//current top speed available
	public float sprint_mult		= 1.2f;								//multiplies the speed_top

	public float jump_force_min		= 2f;
	public float jump_force_max		= 4f;
	public float jump_force_curr	= 2f;

	public float accel_force		= 100f;
	public float decel_force		= 2f;

	public float dash_len_min		= 1f;
	public float dash_len_max		= 2f;
	public float dash_len			= 1f;
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

	public Transform ground_check;

	public LayerMask is_ground;


	void Start () {
	
	}

	void Update () {
<<<<<<< HEAD
		if (Input.GetKey(KeyCode.RightArrow))
						gameObject.rigidbody2D.AddForce (new Vector2 (10f, 0f));
=======

		float h = Input.GetAxis ("Horizontal");

		rigidbody2D.AddForce (Vector2.right * h * accel_force);

		if (Mathf.Abs(rigidbody2D.velocity.x) > speed_top){ 
			rigidbody2D.velocity = new Vector2(Mathf.Sign (rigidbody2D.velocity.x) * speed_top, rigidbody2D.velocity.y);
			Debug.Log ("Top Speed maaaaaaxed!");
		}
	
>>>>>>> 4d28f45a375f9f2357bda043552d185dcb54c80a
	}
}
