using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingTurretController : TurretController {
   public GameObject player;
   public GameObject enemy;

   public float radius = 10;
   public int orbitalSpeed = 50;

   private float angle;

	void Start () {
      base.Start();

      // Place turret at random point on radius of player
      Vector2 startPos = GetRandPoint(player.transform.position, radius, out angle);
      transform.position = startPos;

      isInvincible = false;
	}
	
	// Update is called once per frame
	void Update () {
      

      // Orbit around player
      angle += orbitalSpeed * Time.deltaTime;
      transform.position = GetPoint (player.transform.position, radius, angle);

      base.Update ();

      //target is closest enemy with line of sight
      RaycastHit2D closestEnemy = new RaycastHit2D();

      bool gotTarget = false;
      Vector3 startRay = transform.position;
      List<GameObject> enemyList = dynamicPool.GetPoolList(enemy);
      if (enemyList == null)
      {
         return;
      }
      //http://answers.unity3d.com/questions/1042247/how-to-make-a-simple-line-of-sight-in-a-2d-top-dow.html
      foreach (GameObject childPos in enemyList) {
         if (!childPos.activeInHierarchy) continue;

         //precompute our ray settings
         Vector3 directionRay = (childPos.transform.position - startRay).normalized;
         float distanceRay = maxRange;

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
         Vector2 target = closestEnemy.transform.position;
         Vector2 turret = transform.position;

         /* From https://forum.unity.com/threads/leading-a-target.193445/ */

         float distance = Vector2.Distance (turret, target);
         float travelTime = distance / speed;
         Vector2 newTarget = target + closestEnemy.rigidbody.velocity * travelTime;

         float distance2 = Vector2.Distance (turret, target + (target-newTarget) / 2f);
         float travelTime2 = distance2 / speed;
         Vector2 newTarget2 = target + closestEnemy.rigidbody.velocity * travelTime2;
         Vector2 direction = newTarget - turret;

         direction.Normalize();

         transform.up = direction;

         if (closestEnemy.distance <= maxRange && closestEnemy.distance >= minRange) {
            FireBullet(direction);
         }
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
}
