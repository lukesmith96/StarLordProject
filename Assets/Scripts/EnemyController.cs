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
   public const float maxHealth = 10f;
   public float currentHealth = 10f;
   
   private Rigidbody2D rb2d;
   private float rotation;

   void Start() {
      rb2d = GetComponent<Rigidbody2D>();
	   rotation = 45 * Random.value;
   }
	
	void FixedUpdate () {

	}

	void Update() {
      transform.Rotate (new Vector3 (0, 0, rotation) * Time.deltaTime);
	}
   
   void OnTriggerEnter2D(Collider2D other) {
      /*if (other.gameObject.CompareTag ("Enemy")) {
         other.gameObject.SetActive (false);
      }*/
   }
   
   public void Reset() {
      currentHealth = maxHealth;
      transform.localScale = new Vector3(1, 1, 1);
   }
   
   public void InflictDamage(float dmg) {
      currentHealth -= dmg;
      if (currentHealth <= 0f) {
         //add another type of explosion here?
         gameObject.SetActive(false);
      } else {
         transform.localScale = new Vector3(currentHealth / maxHealth,
            currentHealth / maxHealth, 1);
      }
   }
}
