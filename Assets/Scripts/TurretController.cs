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
public class TurretController : MonoBehaviour {
   public Rigidbody2D bullet;
   
   public float speed = 100.0f;
   public float maxRange = 40f;
   public float minRange = 0f;
   public float reloadTime = 2.0f; //seconds

   protected CircleCollider2D collider;
   
   private GameObject firingArc;
   private float timeSinceFiring = 2.0f; //seconds

   // Use this for initialization
   public void Start () {
      collider = GetComponent<CircleCollider2D> ();
      firingArc = transform.Find("TurretFiringArc").gameObject;
      
      maxRange = bullet.gameObject.GetComponent<BulletController>().maxRange;
      minRange = bullet.gameObject.GetComponent<BulletController>().minRange;
      speed = bullet.gameObject.GetComponent<BulletController>().speed;
      
      float arcScale = (maxRange * 2f) / firingArc.GetComponent<SpriteRenderer>().bounds.size.x;
      firingArc.transform.localScale = new Vector3(arcScale, arcScale, 1);
   }
   
   // Update is called once per frame
   public void Update () {
      if (timeSinceFiring < reloadTime) {
         timeSinceFiring += Time.deltaTime;
      }
   }

   protected void FireBullet(Vector2 direction) {
      if (timeSinceFiring >= reloadTime) {
         timeSinceFiring = 0f;
         
         // Create instance of Bullet
         Rigidbody2D clone = Instantiate(bullet, transform);
         clone.transform.parent = GameObject.Find("BulletHolder").transform;
         
         // Send bullet towards mouse pointer
         clone.AddForce(direction * speed);
      }
   }
}
