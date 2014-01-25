using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {
	
	void Start () {
	
	}

	void Update () {
		if (Input.GetKey(KeyCode.RightArrow))
						gameObject.rigidbody2D.AddForce (new Vector2 (10f, 0f));
	}
}
