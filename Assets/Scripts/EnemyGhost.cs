using UnityEngine;
using System.Collections;

public class EnemyGhost : MonoBehaviour {

	public float en_speed  		= 5f;
	public float en_speed_max	= 15f;
	public float en_inc			= .2f;   			// ghost speed increase per second
	
	public GameObject player;
	
	public bool facing_right	= true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (rigidbody2D.velocity.x > 0 && !facing_right) Flip();
		
		if (rigidbody2D.velocity.x < 0 && facing_right) Flip();

		en_speed += en_inc * Time.deltaTime;

		en_speed = Mathf.Min (en_speed,en_speed_max);

		Vector3 test_vect = player.transform.position - transform.position;
		test_vect.Normalize ();
		rigidbody2D.AddForce(test_vect*en_speed);

	}
	
	void Flip(){
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		facing_right = !facing_right;
	}
}
