using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour {
	private Rigidbody2D rb2d;

	public float thrust = 50f;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		gameObject.GetComponent<Renderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 target = MouseControl.GetWorldPositionOnPlane(Input.mousePosition, 0f);
		Vector2 turret = transform.position;
		Vector2 direction = new Vector2(target.x - turret.x, target.y - turret.y);
		direction.Normalize();
		transform.up = direction;


		if (Input.GetMouseButtonDown (1)) {
			gameObject.GetComponent<Renderer> ().enabled = true;
		} 
		else if (Input.GetMouseButtonUp (1)){
			gameObject.GetComponent<Renderer> ().enabled = false;
		}

	}


}
