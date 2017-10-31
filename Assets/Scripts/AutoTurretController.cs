using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretController : TurretController {
   public GameObject player;
   public GameObject enemy;
   public bool isTouching;

   private SpriteRenderer sprite;
   private Rigidbody2D rb2d;
   private bool isAttached;
   private DynamicObjectPool dynamicPool;
   private Vector3 defaultScale = new Vector3(1, 1, 0);
   
   // Use this for initialization
   new void Start () {
      base.Start();

      sprite = GetComponent<SpriteRenderer> ();
      rb2d = GetComponent<Rigidbody2D> ();

      dynamicPool = (DynamicObjectPool)poolGameObject.GetComponent(typeof(DynamicObjectPool));

      isAttached = false;
      isTouching = false;
      isInvincible = true;

      //scale = transform.localScale;
   }
   
   // Update is called once per frame
   new void Update () {
      base.Update();



      if (isAttached) {
         sprite.color = Color.white;
      } else if (isTouching) {
         sprite.color = Color.green;
      } else {
         sprite.color = Color.red;
      }

      if (isAttached) {
      
         //target is closest enemy with line of sight
         RaycastHit2D closestEnemy = new RaycastHit2D ();

         bool gotTarget = false;
         Vector3 startRay = transform.position;
         List<GameObject> enemyList = dynamicPool.GetPoolList (enemy);
         if (enemyList == null) {
            return;
         }
         //http://answers.unity3d.com/questions/1042247/how-to-make-a-simple-line-of-sight-in-a-2d-top-dow.html
         foreach (GameObject childPos in enemyList) {
            if (!childPos.activeInHierarchy)
               continue;
         
            //precompute our ray settings
            Vector3 directionRay = (childPos.transform.position - startRay).normalized;
            float distanceRay = maxRange;

            //draw the ray in the editor
            Debug.DrawRay (startRay, directionRay * distanceRay, Color.red);

            //do the ray test
            RaycastHit2D[] sightTestResults = Physics2D.RaycastAll (startRay, directionRay, distanceRay);


            //now iterate over all results to work out what has happened
            for (int i = 0; i < sightTestResults.Length; i++) {
               RaycastHit2D sightTest = sightTestResults [i];

               //check if this is the closest, but also check if the player is in the way
               //sightTest.transform.tag != "Player" && 
               if (sightTest.transform.tag != "Enemy")
                  continue;

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
         
            float distance2 = Vector2.Distance (turret, target + (target - newTarget) / 2f);
            float travelTime2 = distance2 / speed;
            Vector2 newTarget2 = target + closestEnemy.rigidbody.velocity * travelTime2;
            Vector2 direction = newTarget - turret;
         
            direction.Normalize ();
         
            transform.up = direction;
         
            if (closestEnemy.distance <= maxRange && closestEnemy.distance >= minRange) {
               FireBullet (direction);
            }
         }

      }

      // Reset scale if attached to player
      ResetScale();
   }
   
   public void AttachToPlayer() {
      Debug.Log ("ATTACHED");

      transform.SetParent (player.transform);

      isAttached = true;
      isInvincible = false;

      rb2d.isKinematic = true;
   }

   private void ResetScale() {
      transform.parent = null;
      transform.localScale = defaultScale;
      transform.SetParent (player.transform);
   }
   
   void OnCollisionEnter2D(Collision2D other) {
      if (other.gameObject.CompareTag ("Turret")) {
         isTouching = false;
      } else if (other.gameObject.CompareTag ("Player")) {
         isTouching = true;
      }

   }
   
   void OnCollisionExit2D(Collision2D other) {
      if (other.gameObject.CompareTag ("Player")) 
      {
         isTouching = false;
      }
   }

   public void Reset() {
      isAttached = isTouching = false;
      isInvincible = true;
      transform.position = Vector3.zero;

      ResetScale();

      rb2d.isKinematic = false;
   }
}
