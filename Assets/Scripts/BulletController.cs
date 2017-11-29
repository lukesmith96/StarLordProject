using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//John Bradbury
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
   }
   

   // Update is called once per frame
   void Update () {
      maxRange = Mathf.Max(minRange, maxRange);
      if (Vector2.Distance(originPoint, transform.position) >= maxRange) {
         gameObject.SetActive(false);
      }
   }
   
   public void ResetBullet(Vector3 startPoint, Vector3 up, float theta, bool isEnemy) {
      originPoint = startPoint;
      gameObject.transform.position = startPoint;
      gameObject.transform.up = up;
      gameObject.transform.Rotate(0, 0, theta);
      isEnemyBullet = isEnemy;
   }
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      // Player shouldn't score if their bullets hit asteroid, right?
      //if (other.gameObject.CompareTag("Asteroid") && !isEnemyBullet)
      /*if (other.gameObject.CompareTag("Asteroid"))
      {
         GameObject exe = dynamicPool.GetPooledObject(explosion);
         exe.transform.position = transform.position;
         exe.SetActive(true);
         exe.GetComponent<ParticleSystem>().Play();

         other.gameObject.SetActive(false);
         this.gameObject.SetActive(false);

         //GameControl.instance.score += 10;
         //GameControl.instance.SetScoreText();
         //PlayerController.instance.addMass(10);

      }*/
      if (((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Enemy2") || other.gameObject.CompareTag("Enemy2")) && !isEnemyBullet)
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
         
         if (isEnemyBullet) {
            if (other.GetComponent<Destructable>().isCentralTurret) {
               PlayerController.instance.reduceMass(5);
            }
         }
         
         //inflict damage
         if (other.GetComponent<Destructable>()) {
            other.GetComponent<Destructable>().InflictDamage(damage);
         } else if (other.gameObject.transform.parent.gameObject.GetComponent<TeleportingEnemy>()) {
            other.gameObject.transform.parent.gameObject.GetComponent<TeleportingEnemy>().InflictDamage(damage);
         }
      }
   }

   void OnTriggerStay2D (Collider2D other){
      if (other.gameObject.CompareTag ("Beam") && other.gameObject.GetComponent<Renderer> ().enabled == true) {
         Vector2 target = MouseControl.GetWorldPositionOnPlane(new Vector2(0, 0), 0f);
         Vector2 current = transform.position;
         Vector2 vectorToOrigin = Vector2.MoveTowards(-current, target, 3 * Time.deltaTime) * 0.07f;
         rb2d.AddForce(vectorToOrigin);
      }
   }
}
