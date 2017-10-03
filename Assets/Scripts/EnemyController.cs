using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Nicholas Berriochoa
 * @since 9.26.17
 * 
 * v1 - basic mover for enemy (may rename or move code somewhere else)
 * 
 */
public class EnemyController : MonoBehaviour {
   private Rigidbody2D rb2d;
   private float rotation;


   void Start() {
      rb2d = GetComponent<Rigidbody2D>();
	   rotation = 45 * Random.value;
   }
	
	void FixedUpdate () {
      Vector2 origin = new Vector2 (0, 0);
      rb2d.AddForce(Vector2.MoveTowards (new Vector2 (-transform.position.x, -transform.position.y), origin, 3 * Time.deltaTime));

	}

	void Update() {
	  transform.Rotate (new Vector3 (0, 0, rotation) * Time.deltaTime);
	}

   void OnTriggerEnter2D(Collider2D other) {
      if (other.gameObject.CompareTag ("Enemy")) {
         other.gameObject.SetActive (false);
      }
   }
}
