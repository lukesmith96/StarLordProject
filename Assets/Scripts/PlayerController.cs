using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

   public float scaleValue = .01F;
   //public float scaleValue = 2F;
   public static PlayerController instance;
   public float rotationTime = .5f;
   public Vector3 newScale;
   public float mass;

   private bool isColliding;
   private bool rotatePU;
   private float currRotation;
   private Vector3 currScale;


   // Use this for initialization
   void Start () {
      rotatePU = false;
      instance = this;
      currScale = newScale = transform.localScale;
      mass = 100;
   }

   // Update is called once per frame
   void Update () {
      // Pressing '=' increases the scale of the player
      if (Input.GetKeyDown("="))
      {
         // add to total mass and mass based on that
         addMass();
      }
      // Pressing '-' decreases the mass of the player
      if (Input.GetKeyDown("-"))
      {
         reduceMass();
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

      scalePlayer ();
      // Interpolate to newScale
      Vector3 actualScale = Vector3.Lerp (currScale, newScale, 1.0f * Time.deltaTime);
      transform.localScale = actualScale;
      currScale = actualScale;
	}


   void FixedUpdate() {
   }
      
   void OnTriggerEnter2D(Collider2D other) {
      // Joshua King
      // If an enemy collides with the player, the player loses score
      if (other.gameObject.CompareTag ("Enemy")) {
         other.gameObject.SetActive (false);
         if (isColliding)
            return;
         isColliding = true;
         //transform.localScale -= (new Vector3 (scaleValue, scaleValue, 0) * Time.deltaTime);
         
         if (!GameControl.instance.godmode) {
            reduceMass();
         }
      } else if (other.gameObject.CompareTag ("Asteroid")) {
         // Joshua King
         // If an asteroid comes in contact with the player, the player gains mass
         other.gameObject.SetActive (false);
         if (isColliding)
            return;
         isColliding = true;

         GameControl.instance.score += 10;
         GameControl.instance.SetScoreText();
         addMass();
      }
   }

   public void scalePlayer() {
      newScale = new Vector3(scaleValue * mass, scaleValue * mass, 0);
   }

   public void addMass()
   {
      mass += 10;
   }

   public void reduceMass()
   {
      mass -= 10;
   }

   public void startRotationPU()
   {
      rotatePU = true;
      currRotation = Time.time;
   }
}