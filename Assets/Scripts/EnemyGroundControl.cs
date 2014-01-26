using UnityEngine;
using System.Collections;

public class EnemyGroundControl : MonoBehaviour {

	public float en_speed  		= 5f;
	public float en_speed_max	= 15f;

	public bool facing_right	= true;

	public Transform edge_check;
	private float lifeTime = 0f;


	void Start () {
	  
	}
	

	void Update () {
		if (facing_right){
			Debug.Log ("R");
			rigidbody2D.AddForce(Vector2.right * en_speed / Time.deltaTime);
			//rigidbody2D.velocity = new Vector2(en_speed / Time.deltaTime, rigidbody2D.velocity.y);

		} else{
			Debug.Log ("L");
			rigidbody2D.AddForce(Vector2.right * -en_speed / Time.deltaTime);
			//rigidbody2D.velocity = new Vector2(-en_speed / Time.deltaTime, rigidbody2D.velocity.y);
		}

		if (!Physics2D.Linecast (transform.position,edge_check.transform.position,1 << LayerMask.NameToLayer ("Platform")))
		{
			Debug.Log ("Swap!");
			//Debug.Log (Physics2D.Linecast (transform.position,edge_check.transform.position,1 << LayerMask.NameToLayer ("Platform")));
			Flip();
		}

		if (Physics2D.Linecast (transform.position,new Vector3(transform.position.x + (Mathf.Sign(transform.localScale.x) * .7f),transform.position.y,0), 1 << LayerMask.NameToLayer ("Platform")))
		{
			Debug.Log ("Swap");
			Flip();
		}

		lifeTime += Time.deltaTime;
		
		if (lifeTime > 40) 
			Destroy (gameObject);
	}

	void Flip(){
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		facing_right = !facing_right;
		rigidbody2D.velocity = Vector2.zero;
	}
}
