using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author: Joshua King
 * @since: 11.5.17
 * 
 * A tractor beam that is activated via right click;
 * pulls in objects toward the player;
 * interacts with various objects on the game field.
 */

public class TractorBeam : MonoBehaviour {
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		gameObject.GetComponent<Renderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 target = MouseControl.GetWorldPositionOnPlane(Input.mousePosition, 0f);
		Vector2 player = transform.position;
		Vector2 direction = new Vector2(target.x - player.x, target.y - player.y);
		direction.Normalize();
		transform.up = direction;

		//only draw the beam if the right mouse button is clicked
		if (Input.GetMouseButtonDown (1)) {
			gameObject.GetComponent<Renderer> ().enabled = true;
		} 
		else if (Input.GetMouseButtonUp (1)){
			gameObject.GetComponent<Renderer> ().enabled = false;
		}

	}


}
