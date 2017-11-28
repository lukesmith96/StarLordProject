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
public class EnemyController : Destructable {
   private Rigidbody2D rb2d;

   private Vector3 saveScale;
   
   void Awake() {
      saveScale = transform.localScale;
   }
   
   void Start() {
      base.Start();
      
      rb2d = GetComponent<Rigidbody2D>();
   }
	
	void FixedUpdate () {

	}

	void Update() {
      transform.up = (-transform.position).normalized;
	}
   
   void OnTriggerEnter2D(Collider2D other) {
      /*if (other.gameObject.CompareTag ("Enemy")) {
         other.gameObject.SetActive (false);
      }*/
   }
   
   public void Reset() {
      base.Reset();
      transform.localScale = saveScale;
   }
}
