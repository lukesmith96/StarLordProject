using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {

   public bool isInvincible = false;
   public const float maxHealth = 10f;
   protected float currentHealth = 0f;
   public float collisionDamage = 20f; //how much damage does this inflict when it collides with something

   // Use this for initialization
   public void Start () {
      Reset();
   }
   
   // Update is called once per frame
   public void Update () {
      
   }
   
   public void Reset() {
      currentHealth = maxHealth;
   }
   
   public void InflictDamage(float dmg) {
      if (isInvincible) return;
      
      currentHealth -= dmg;
      if (currentHealth <= 0) {
         currentHealth = 0;
         gameObject.SetActive(false);
         
         //trigger explosion
         GameObject exe = EnemySpawner.GetPooledObject(EnemySpawner.instance.pooledExplosions);
         exe.transform.position = transform.position;
         exe.SetActive(true);
         exe.GetComponent<ParticleSystem>().Play();
      }
   }
   
   public float GetCollisionDamage() { return collisionDamage; }
}
