using UnityEngine;
using System.Collections;

public class EnemyFlying : MonoBehaviour {
	public float en_speed  		= 5f;
	public float en_speed_max	= 15f;
	public float detect_range	= 200f;
	public float max_distance	= 500f;

	public Vector3 start_loc;

	public GameObject player;

	public bool hunting = false;
	
	public bool facing_right	= true;
	// Use this for initialization
	void Start () {
		start_loc = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		if (rigidbody2D.velocity.x > 0 && !facing_right) Flip();

		if (rigidbody2D.velocity.x < 0 && facing_right) Flip();

		if (Vector3.Distance (player.transform.position,transform.position) < detect_range && Vector3.Distance (start_loc,transform.position) < max_distance)
		{
			//float step = en_speed * Time.deltaTime;
			//transform.position = Vector3.MoveTowards(transform.position,player.transform.position,step);
			hunting = true;

		} else hunting = false;

		if (hunting)
		{
			Vector3 test_vect = player.transform.position - transform.position;
			test_vect.Normalize ();
			rigidbody2D.AddForce(test_vect*en_speed);
		}else 
		{
			if (Vector3.Distance (start_loc,transform.position) > 1f){
				Vector3 ret_vect = start_loc-transform.position;
				ret_vect.Normalize ();
				rigidbody2D.AddForce(ret_vect*en_speed);
			}
		}
	}

	void Flip(){
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		facing_right = !facing_right;
	}
}
