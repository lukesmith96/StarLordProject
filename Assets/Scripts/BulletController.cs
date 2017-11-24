using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
   public float maxRange = 20f;
   public float minRange = 0f;
   public float speed = 500f;
   public float damage = 10f;
   public Vector2 originPoint = Vector2.zero;
   public GameObject explosion;
   public bool isEnemyBullet = false;
   
   private Rigidbody2D rb2d;
   private DynamicObjectPool dynamicPool;
   
   // Use this for initialization
   void Start()
   {
      rb2d = GetComponent<Rigidbody2D>();
      dynamicPool = (DynamicObjectPool)EnemySpawner.instance.poolGameObject.GetComponent(typeof(DynamicObjectPool));
      originPoint = transform.position;
   }
   
   /*void OnEnable() {
	   originPoint = transform.position;
   }*/

   // Update is called once per frame
   void Update () {
	  
      if (Vector2.Distance(originPoint, transform.position) >= maxRange) {
		  Debug.Log("Origin: " + originPoint + " Position: " + transform.position);
         gameObject.SetActive(false);
      }
   }
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("Asteroid") && !isEnemyBullet)
      {
         other.gameObject.SetActive(false);
         this.gameObject.SetActive(false);
         PlayerController.instance.addMass();
      }
      if (((other.gameObject.CompareTag("Enemy")) && !isEnemyBullet)
         || (other.gameObject.CompareTag("Turret") && isEnemyBullet))
      {
         /*
          * We need to trigger explosion in enemySpawner not sure how to yet.
          */
         //trigger explosion
         GameObject exe = dynamicPool.GetPooledObject(explosion);
         exe.transform.position = transform.position;
         exe.SetActive(true);
         exe.GetComponent<ParticleSystem>().Play();
         
         this.gameObject.SetActive(false);

         PlayerController.instance.startRotationPU();
         //inflict damage
         if (other.GetComponent<Destructable>()) {
            other.GetComponent<Destructable>().InflictDamage(damage);
         } else if (other.gameObject.transform.parent.gameObject.GetComponent<TeleportingEnemy>()) {
            other.gameObject.transform.parent.gameObject.GetComponent<TeleportingEnemy>().InflictDamage(damage);
         }
      }
      /*
      if (other.gameObject.CompareTag("Player") && originPoint != Vector2.zero) {
         this.gameObject.SetActive(false);
      }
      */
   }
}
