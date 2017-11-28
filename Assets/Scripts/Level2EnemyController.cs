using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Level2EnemyController : EnemyController {
   
   public float radius = 12;
   public int orbitalSpeed = 12;
   public Vector2 originPoint = Vector2.zero;
   private float angle;
   private int dir;
   private bool rotate = false;

   public GameObject bullet;
   public float maxRange = 40f;
   public float minRange = 0f;
   public float speed = 25.0f;
   public float reloadTime = 4.0f; //seconds
   private float timeSinceFiring = 4.0f; //seconds
   private Rigidbody2D rb2d;
   private bool isBeingPulled;

   // Use this for initialization
   void Start () {
      base.Start();
      rb2d = GetComponent<Rigidbody2D>();
   }
	
	// Update is called once per frame
	void Update () {
      Vector2 target = new Vector2(0,0);
      Vector2 turret = transform.position;
      Vector2 direction = new Vector2(target.x - turret.x, target.y - turret.y);
      direction.Normalize();
      transform.up = direction;

      if (Vector2.Distance(originPoint, transform.position) <= radius && !rotate)
      {
         Vector2 startPos = GetRandPoint(PlayerController.instance.transform.position, transform.position, radius, out angle, out dir);
         rotate = true;
      }
      if (rotate)
      {
         // Orbit around player
         angle += (orbitalSpeed * Time.deltaTime * dir);
         transform.position = GetPoint(PlayerController.instance.transform.position, radius, angle, dir);
         FireBullet(new Vector2(0, 0));

         if (timeSinceFiring < reloadTime)
         {
            timeSinceFiring += Time.deltaTime;
         }
      }
   }
   protected void FireBullet(Vector2 direction)
   {
      if (timeSinceFiring >= reloadTime && isBeingPulled == false)
      {
         timeSinceFiring = 0f;
         Vector2 newDir = -Vector2.MoveTowards(transform.position, direction, Time.deltaTime);
         
         GameObject clone;
         Rigidbody2D cloneRb2d;
         //Get instance of Bullet
         clone = dynamicPool.GetPooledObject(bullet);
         //if (clone == null) continue; //for graceful error
         clone.SetActive(true);
         cloneRb2d = clone.GetComponent<Rigidbody2D>();

         cloneRb2d.transform.position = transform.position;
         cloneRb2d.velocity = Vector2.zero;
         cloneRb2d.transform.up = transform.up;
         cloneRb2d.transform.gameObject.GetComponent<BulletController>().isEnemyBullet = true;
         // Send bullet on its errand of destruction
         cloneRb2d.AddForce(newDir * speed);
      }
   }

   private Vector2 GetRandPoint(Vector2 origin, Vector2 pos, float radius, out float angle, out int dir)
   {
      System.Random random = new System.Random();
      angle = Mathf.Atan2(pos.y - origin.y, pos.x - origin.x) * Mathf.Rad2Deg;
      dir = random.Next(2);
      if (dir == 0)
         --dir;
      return GetPoint(origin, radius, angle, dir);
   }

   private Vector2 GetPoint(Vector2 origin, float radius, float angle, int dir)
   {
      float x = origin.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
      float y = origin.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle);

      return new Vector2(x, y);
   }
   
   void OnEnable() {
      base.Reset();
      transform.position = originPoint;
      rotate = false;
   }

   void OnTriggerStay2D (Collider2D other){
      if (other.gameObject.CompareTag ("Beam") && other.gameObject.GetComponent<Renderer> ().enabled == true) {
         Vector2 target = MouseControl.GetWorldPositionOnPlane (new Vector2 (0, 0), 0f);
         Vector2 current = transform.position;
         Vector2 vectorToOrigin = Vector2.MoveTowards (-current, target, 3 * Time.deltaTime) * .5f;
         rb2d.AddForce (vectorToOrigin);
         isBeingPulled = true;
      }
   }

}
