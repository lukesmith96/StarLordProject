using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingTurretController : AutoTurretController {
   public float radius = 0;
   public int orbitalSpeed = 50;

   private float angle = 0;

   private TrailRenderer trailRenderer;

   public override void Start () {
      base.Start();

      // Place turret at random point on radius of player
      //Vector2 startPos = GetRandPoint(player.transform.position, radius, out angle);
      //transform.position = startPos;

      trailRenderer = GetComponent<TrailRenderer> ();
      trailRenderer.enabled = false;

      isInvincible = false;
      isTouching = true;
	}
	
	// Update is called once per frame
   public override void Update () {
      base.Update ();



      if (isAttached) {
         float currentRadius = radius * player.transform.localScale.x;

         // Orbit around player
         angle += orbitalSpeed * Time.deltaTime;
         transform.position = GetPoint (player.transform.position, currentRadius, angle);
      }
         
	}

   // Angle calculation from: https://answers.unity.com/questions/728680/how-to-get-the-angle-between-two-objects-with-ontr.html
   override public void AttachToPlayer() {
      Debug.Log ("ATTACHED");

      radius = Mathf.Sqrt (Mathf.Pow (transform.position.x, 2) + Mathf.Pow (transform.position.y, 2));

      Vector3 dir = transform.position;
      dir = transform.InverseTransformDirection(dir);
      float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

      angle = ang;
      //Debug.Log ("ANGLE: " + angle);

      isAttached = true;
      isInvincible = false;

      trailRenderer.enabled = true;
   } 

   override public void Reset() {
      isAttached = false;
      isTouching = isInvincible = true;
      trailRenderer = GetComponent<TrailRenderer> ();
      trailRenderer.enabled = false;

      transform.position = Vector3.zero;

   }

   override protected void ColorSprite() {
      if (isAttached) {
         sprite.color = Color.white;
      } else if (isTouching) {
         sprite.color = Color.green;
      } else {
         sprite.color = Color.red;
      }
   }

   private Vector2 GetRandPoint (Vector2 origin, float radius, out float angle) {
      angle = Random.Range (0, 360);
       
      float x = origin.x + radius * Mathf.Cos (Mathf.Deg2Rad * angle);
      float y = origin.y + radius * Mathf.Sin (Mathf.Deg2Rad * angle);

      return new Vector2 (x, y);
   }

   private Vector2 GetPoint (Vector2 origin, float radius, float angle) {
      float x = origin.x + radius * Mathf.Cos (Mathf.Deg2Rad * angle);
      float y = origin.y + radius * Mathf.Sin (Mathf.Deg2Rad * angle);

      return new Vector2 (x, y);
   }


   void OnCollisionEnter2D(Collision2D other) {
      if (other.gameObject.CompareTag ("Turret")) {
         isTouching = false;
      } else if (other.gameObject.CompareTag ("Player")) {
         isTouching = false;
      }

   }

   void OnCollisionExit2D(Collision2D other) {
      if (other.gameObject.CompareTag ("Player")) 
      {
         isTouching = true;
      }

      if (other.gameObject.CompareTag ("Turret")) {
         isTouching = true;
      }
   }
}
