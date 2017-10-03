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
   public float speed = 500.0f;
   public Rigidbody2D bullet;

   protected CircleCollider2D collider;

	// Use this for initialization
	void Start () {
      collider = GetComponent<CircleCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
      
	}

   protected void FireBullet(Vector2 direction) {
      // Create instance of Bullet
      Rigidbody2D clone = Instantiate(bullet, transform);
      clone.transform.parent = GameObject.Find("BulletHolder").transform;

      // Send bullet towards mouse pointer
      clone.AddForce(direction * speed);
   }


}
