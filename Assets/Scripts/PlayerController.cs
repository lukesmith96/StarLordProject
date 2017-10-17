using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

   public float scaleValue = 2F;
   public static PlayerController instance;

   //private int score;
   private bool isColliding;
   public float rotationTime = .5f;
   private bool rotatePU;
   private float currRotation;

   // Use this for initialization
   void Start () {
      rotatePU = false;
      instance = this;
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
      if (rotatePU && currRotation < (Time.time - rotationTime))
      {
         rotatePU = false;
      }
      if (Input.GetKey(KeyCode.UpArrow) && rotatePU)
      {
         transform.Rotate(new Vector3(0, 0, 45) * (Time.deltaTime * transform.localScale.x));
      }
      if (Input.GetKey(KeyCode.DownArrow) && rotatePU)
      {
         transform.Rotate(new Vector3(0, 0, -45) * (Time.deltaTime * transform.localScale.x));
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
   public void startRotationPU()
   {
      rotatePU = true;
      currRotation = Time.time;
   }

}
