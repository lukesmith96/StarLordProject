﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
   public float maxRange = 20f;
   public float minRange = 0f;
   public float speed = 500f;
   public float damage = 10f;
   public Vector2 originPoint = Vector2.zero;
   public GameObject explosion;
   
   private Rigidbody2D rb2d;
   
   // Use this for initialization
   void Start()
   {
      rb2d = GetComponent<Rigidbody2D>();
      originPoint = transform.position;
   }

   // Update is called once per frame
   void Update () {
      if (Vector2.Distance(originPoint, transform.position) >= maxRange) {
         gameObject.SetActive(false);
      }
   }
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("Asteroid"))
      {
         other.gameObject.SetActive(false);
         this.gameObject.SetActive(false);
         PlayerController.instance.addMass();
      }
      if (other.gameObject.CompareTag("Enemy"))
      {
         /*
          * We need to trigger explosion in enemySpawner not sure how to yet.
          */
         //trigger explosion
         GameObject exe = EnemySpawner.GetPooledObject(EnemySpawner.instance.pooledExplosions);
         exe.transform.position = transform.position;
         exe.SetActive(true);
         exe.GetComponent<ParticleSystem>().Play();
         
         this.gameObject.SetActive(false);

         PlayerController.instance.startRotationPU();
         //inflict damage
         if (other.GetComponent<EnemyController>()) {
            other.GetComponent<EnemyController>().InflictDamage(damage);
         } else if (other.gameObject.transform.parent.gameObject.GetComponent<TeleportingEnemy>()) {
            other.gameObject.transform.parent.gameObject.GetComponent<TeleportingEnemy>().InflictDamage(damage);
         }
      }
      if (other.gameObject.CompareTag("Player") && originPoint != Vector2.zero) {
         this.gameObject.SetActive(false);
      }
   }
}
