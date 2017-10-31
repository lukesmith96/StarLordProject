using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingEnemy : MonoBehaviour {
   public const float maxHealth = 10f;
   public float currentHealth = 10f;
   
   private Rigidbody2D rb2d;
   
   public GameObject solidImage;
   public GameObject ghostImage;
   public GameObject forceField; //when not teleporting
   public GameObject teleportEffect; //when teleporting
   
   public GameObject poolGameObject;
   private DynamicObjectPool dynamicPool;
   
   public GameObject spawnedObject;
   public float spawnRate;
   public float teleportRate;
   public float teleportCountdown; //how long does it take to teleport
   public float closeToRange;
   
   private Vector3 saveScale;
   private float nextSpawn = 0.0f;
   private float nextTeleport = 0.0f;
   private Vector2 telePos;
   private float currentTeleCountdown = 0.0f;
   // Use this for initialization
   void Start () {
      saveScale = transform.localScale;
      dynamicPool = (DynamicObjectPool)poolGameObject.GetComponent(typeof(DynamicObjectPool));
      
      //teleport into battle
      nextTeleport = teleportRate;
      Teleport();
   }
   
   // Update is called once per frame
   void Update () {
      //teleport to some random location in range
      
      if (!Teleporting()) {
         if (!Teleport()) {
            LaunchEnemy();
         }
      }
   }
   
   //Choose a destination, start teleportation countdown and ghosting
   bool Teleport() {
      if (nextTeleport >= teleportRate) {
         float randX, randY;
         
         float theta = Random.Range(0f, 360f);
         float r = closeToRange;
         float radians = theta * Mathf.Deg2Rad; 
         telePos = new Vector2(r * Mathf.Sin(radians), r * Mathf.Cos(radians));
         
         //place ghost there
         ghostImage.transform.position = telePos;
         ghostImage.transform.up = (Vector3.zero - ghostImage.transform.position).normalized;
         ghostImage.SetActive(true);
         
         nextTeleport = 0.0f;
         currentTeleCountdown = teleportCountdown;
         
         //remove the force field
         forceField.SetActive(false);
         teleportEffect.SetActive(true);
         return true;
      }
      nextTeleport += Time.deltaTime;
      return false;
   }
   
   bool Teleporting() {
      if (currentTeleCountdown > 0.0f) {
         currentTeleCountdown -= Time.deltaTime;
         if (currentTeleCountdown <= 0.0f) {
            currentTeleCountdown = 0.0f;
            //finish teleporting
            transform.position = telePos;
            solidImage.transform.up = (Vector3.zero - solidImage.transform.position).normalized;
            ghostImage.SetActive(false);
            
            //set up the force field (no damage)
            forceField.SetActive(true);
            teleportEffect.SetActive(false);
         }
         return true;
      }
      return false;
   }
   
   void LaunchEnemy() {
      if (Time.time > nextSpawn) {
         nextSpawn = Time.time + spawnRate;
         
         Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y);
         
         //spawn enemy
         GameObject tmp = dynamicPool.GetPooledObject(spawnedObject);
         tmp.SetActive(true);
         tmp.transform.position = spawnPos;
         Vector2 direction = Vector2.zero - spawnPos;
         tmp.GetComponent<EnemyController>().Reset();
         tmp.GetComponent<Rigidbody2D>().AddForce(direction.normalized * 300f);
      }
   }
   
   public void Reset() {
      currentHealth = maxHealth;
      transform.localScale = saveScale;
   }
   
   public void InflictDamage(float dmg) {
      if (currentTeleCountdown > 0.0f) { //can only damage when it is teleporting
         currentHealth -= dmg;
         if (currentHealth <= 0f) {
            //add another type of explosion here?
            gameObject.SetActive(false);
            
            // Increment score
            GameControl.instance.score += 20;
            GameControl.instance.SetScoreText();
         }
      }
   }
}
