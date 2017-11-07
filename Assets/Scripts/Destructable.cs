using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {

   public bool isInvincible = false;
   public const float maxHealth = 10f;
   protected float currentHealth = 0f;
   public float collisionDamage = 20f; //how much damage does this inflict when it collides with something
   public GameObject explosionObject; //anything destructable can explode
   
   protected DynamicObjectPool dynamicPool;

   // Use this for initialization
   public void Start () {
      dynamicPool = (DynamicObjectPool)EnemySpawner.instance.poolGameObject.GetComponent(typeof(DynamicObjectPool));
      
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
         
         //trigger explosion
         GameObject exe = dynamicPool.GetPooledObject(explosionObject);
         exe.transform.position = transform.position;
         exe.SetActive(true);
         exe.GetComponent<ParticleSystem>().Play();
         
         gameObject.SetActive(false);
      }
   }
   
   public float GetCollisionDamage() { return collisionDamage; }
}
