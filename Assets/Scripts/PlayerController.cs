using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float scaleValue = 2F;
	

	//private int score;
	private bool isColliding;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// Pressing '=' increases the scale of the player
		if (Input.GetKeyDown("="))
		{
			transform.localScale += (new Vector3 (scaleValue, scaleValue, 0) * Time.deltaTime);
		}
		// Pressing '-' decreases the scale of the player
		if (Input.GetKeyDown("-"))
		{
			transform.localScale -= (new Vector3 (scaleValue, scaleValue, 0) * Time.deltaTime);
		}

		isColliding = false;
	}

	void FixedUpdate() {
	}

   void OnTriggerEnter2D(Collider2D other) {
      if (other.gameObject.CompareTag ("Enemy")) {
         other.gameObject.SetActive (false);
			if (isColliding)
				return;
			isColliding = true;

         GameControl.instance.score -= 10;
         GameControl.instance.SetScoreText ();
      }
   }

	
}
