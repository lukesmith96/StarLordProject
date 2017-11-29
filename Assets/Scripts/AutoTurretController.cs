using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretController : TurretController {
   public GameObject enemy;

   public bool isTouching;
   public bool isAttached;

   protected float angle;
   protected float currentScale;

   protected SpriteRenderer sprite;

   private Rigidbody2D rb2d;
   private Vector3 defaultScale = new Vector3(1, 1, 0);

   
   // Use this for initialization
   public override void Start () {
      base.Start();

      sprite = GetComponent<SpriteRenderer> ();
      rb2d = GetComponent<Rigidbody2D> ();

      isAttached = false;
      isTouching = false;
      isInvincible = true;
   }
   
   public virtual int turretCost()
   {
      return 25;
   }

   // Update is called once per frame
   public override void Update () {
      base.Update();

      ColorSprite ();

      if (isAttached) {
         AutoFire ();

         // Reset position according to scaling player
         if (!CompareTag ("OrbitalTurret")) {
            float newRadius = GetComponent<CircleCollider2D> ().radius + PlayerController.instance.GetComponent<CircleCollider2D> ().radius * PlayerController.instance.transform.localScale.x;
            Vector2 newPos = GetPoint (Vector2.zero, newRadius, angle);
            transform.position = newPos;
         }
      }

      if (Input.GetMouseButtonDown (1)) {
      }
         //gameObject.SetActive (false);
   }
   
   virtual public void AttachToPlayer() {
      angle = GetAngle ();

      isAttached = true;
      isInvincible = false;

      rb2d.isKinematic = true;
   }

   virtual protected void ColorSprite() {
      if (isAttached) {
         sprite.color = Color.white;
      } else if (isTouching) {
         sprite.color = Color.green;
      } else {
         sprite.color = Color.red;
      }
   }

   public float GetAngle() {
      Vector3 dir = transform.position;
      //Debug.Log ("Angle pos: " + dir);
      //dir = transform.InverseTransformDirection(dir);
      return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
   }

   protected Vector2 GetPoint (Vector2 origin, float radius, float angle) {
      float x = origin.x + radius * Mathf.Cos (Mathf.Deg2Rad * angle);
      float y = origin.y + radius * Mathf.Sin (Mathf.Deg2Rad * angle);

      return new Vector2 (x, y);
   }

   protected void AutoFire() {
      //target is closest enemy with line of sight
      RaycastHit2D closestEnemy = new RaycastHit2D();

      bool gotTarget = false;
      float minDistance = Mathf.Infinity;
         
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
            if (sightTest.transform.CompareTag ("Player"))
               continue;

            if (sightTest.transform.CompareTag ("Enemy") ||
               //sightTest.transform.CompareTag ("Boss") ||
               //sightTest.transform.CompareTag ("Enemy3") ||
               sightTest.transform.CompareTag ("Enemy2")) {
               closestEnemy = sightTest;
               gotTarget = true;
               break;
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

         if (!CompareTag("OrbitalTurret"))
            transform.up = direction;

         if (closestEnemy.distance <= maxRange && closestEnemy.distance >= minRange) {
            FireBullet (direction);
         }
      }
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

   virtual public void Reset() {
      isAttached = isTouching = false;
      isInvincible = true;
      transform.position = Vector3.zero;
      angle = GetAngle ();

      rb2d = GetComponent<Rigidbody2D> ();
      rb2d.isKinematic = false;
   }
}
