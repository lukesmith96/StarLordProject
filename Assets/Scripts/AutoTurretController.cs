using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretController : TurretController {
   public GameObject player;
   public float detectionRange = 40f;
   
   private bool isDragged;  // Allows smooth dragging
   private bool isAttached;
   private bool isTouching;
   
	// Use this for initialization
	void Start () {
      isDragged = false;
      isAttached = false;
      isTouching = false;
	}
	
	// Update is called once per frame
	void Update () {
      //target is closest enemy with line of sight
      RaycastHit2D closestEnemy = new RaycastHit2D();
      bool gotTarget = false;
      Vector3 startRay = transform.position;
      
      //http://answers.unity3d.com/questions/1042247/how-to-make-a-simple-line-of-sight-in-a-2d-top-dow.html
      foreach (GameObject childPos in EnemySpawner.instance.pooledEnemies) {
         //precompute our ray settings
         Vector3 directionRay = (childPos.transform.position - startRay).normalized;
         float distanceRay = detectionRange;

         //draw the ray in the editor
         Debug.DrawRay(startRay, directionRay * distanceRay, Color.red);

         //do the ray test
         RaycastHit2D[] sightTestResults = Physics2D.RaycastAll(startRay,  directionRay, distanceRay);

         //now iterate over all results to work out what has happened
         for(int i = 0; i < sightTestResults.Length; i++) {
            RaycastHit2D sightTest = sightTestResults[i];
            
            //check if this is the closest, but also check if the player is in the way
            if (sightTest.transform.tag != "Player" && sightTest.transform.tag != "Enemy") continue;
            
            if (!gotTarget) {
               closestEnemy = sightTest;
               gotTarget = true;
            } else {
               if (sightTest.distance < closestEnemy.distance) {
                  closestEnemy = sightTest;
               }
            }
         }
      }
      
      if (gotTarget) {
         if (closestEnemy.transform.tag == "Player") {
            gotTarget = false;
         }
      }
      
      if (gotTarget) {
         Vector2 target = MouseControl.GetWorldPositionOnPlane(Input.mousePosition, 0f);
         Vector2 turret = transform.position;
         Vector2 direction = new Vector2(target.x - turret.x, target.y - turret.y);
         direction.Normalize();
        
         transform.up = direction;
      }

      // Only release turret when mouse button released
      if (isDragged && Input.GetMouseButtonUp (1)) 
      {
         isDragged = false;
      }

      if (isTouching && Input.GetKeyDown ("a")) {
         Debug.Log ("ATTACHED");

         transform.SetParent (player.transform);
         isAttached = true;
      }

      // Debugging
      if (isTouching && Input.GetKeyDown ("d")) {
         Debug.Log ("DETACHED");

         transform.SetParent (null);
         isAttached = false;
      }
	}
   
   void FixedUpdate() {
      Vector2 target = MouseControl.GetWorldPositionOnPlane(Input.mousePosition, 0f);

      if (isDragged && !isAttached) 
      {
         transform.position = target;
         GetComponent<Collider>().transform.position = target;
      }
   }
   
   void OnMouseOver() {
      // Dragging only works while mouse is over turret.
      // Sometimes mouse moves faster than the turret transform and drops it,
      // so isDragged prevents this.
      if (Input.GetMouseButton(1)) 
      {   
         isDragged = true;
      }
   }

   void OnCollisionEnter2D(Collision2D other) {
      if (other.gameObject.CompareTag ("Player")) 
      {
         isTouching = true;
      }
   }

   void OnCollisionExit2D(Collision2D other) {
      if (other.gameObject.CompareTag ("Player")) 
      {
         isTouching = false;
      }
   }
}
