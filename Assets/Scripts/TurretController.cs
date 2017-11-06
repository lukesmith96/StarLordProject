using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
 * @author: Luke Smith, Ken Oshima
 * @since: 9.26.17
 * 
 * This class is the parent class to all Types of turrets.
 */
public class TurretController : Destructable {
   public GameObject player;
   public GameObject enemy;
   public GameObject bullet;
   
   public float speed = 100.0f;
   public float maxRange = 40f;
   public float minRange = 0f;
   public float reloadTime = 2.0f; //seconds
   public int numBullets = 1;
   public float spread = 0.0f; //inaccuracy

   protected CircleCollider2D collider;
   
   private GameObject firingArc;
   private float timeSinceFiring = 2.0f; //seconds

   private bool selected = false;

   // Use this for initialization
   public void Start () {
      base.Start();
      
      collider = GetComponent<CircleCollider2D> ();
      firingArc = transform.Find("TurretFiringArc").gameObject;

      maxRange = bullet.gameObject.GetComponent<BulletController>().maxRange;
      minRange = bullet.gameObject.GetComponent<BulletController>().minRange;
      speed = bullet.gameObject.GetComponent<BulletController>().speed;
      
      float arcScale = (maxRange * 2f) / firingArc.GetComponent<SpriteRenderer>().bounds.size.x;
      firingArc.transform.localScale = new Vector3(arcScale, arcScale, 1);

      isInvincible = true;
   }
   // Update is called once per frame
   public void Update () {
      if (timeSinceFiring < reloadTime) {
         timeSinceFiring += Time.deltaTime;
      }
   }

   protected void AutoFire() {
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

   protected void FireBullet(Vector2 direction) {
      if (timeSinceFiring >= reloadTime) {
         timeSinceFiring = 0f;
         transform.up = direction;
         
         float theta = -spread / 2;
         float incTheta = numBullets > 1 ? spread / (numBullets - 1) : 0f;
         if (numBullets == 1) {
            theta = 0f;
         }
         
         GameObject clone;
         Rigidbody2D cloneRb2d;
         for (int i = 0; i < numBullets; ++i, theta += incTheta) {
            //Get instance of Bullet
            clone = dynamicPool.GetPooledObject(bullet);
            //if (clone == null) continue; //for graceful error
            clone.SetActive(true);
            cloneRb2d = clone.GetComponent<Rigidbody2D>();
            
            cloneRb2d.transform.position = transform.position;
            cloneRb2d.velocity = Vector2.zero;
            cloneRb2d.transform.up = transform.up;
            cloneRb2d.transform.Rotate(0, 0, theta);
            cloneRb2d.transform.gameObject.GetComponent<BulletController>().isEnemyBullet = false;
            // Send bullet on its errand of destruction
            cloneRb2d.AddForce(clone.transform.up * speed);
         }
      }
   }

   void OnTriggerEnter2D(Collider2D other) {
      if (other.gameObject.CompareTag("Enemy") && !isInvincible) {
         Destructable otherScript = null;
         GameObject parent = other.gameObject;
         while (true) {
            if (parent.GetComponent<Destructable>()) {
               otherScript = parent.GetComponent<Destructable>();
               break;
            }
            
            parent = parent.transform.parent.gameObject;
         }
         
         otherScript.InflictDamage(collisionDamage);
         InflictDamage(otherScript.GetCollisionDamage());
      }
   }
}
