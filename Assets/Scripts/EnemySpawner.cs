using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * @author: Nicholas Berriochoa
 * @since: 9.26.17
 * 
 * v1 - added random spawning based on variabel spawnRate
 *    - ranges can be adjusted
 */
public class EnemySpawner : MonoBehaviour {

   public static EnemySpawner instance;

   //enemy prefabs
   public GameObject diveBomber;
   public GameObject orbiter;
   public GameObject teleportingBoss;

   //dynamic pool stuff
   public GameObject poolGameObject;
   private DynamicObjectPool dynamicPool;

   //wave stuff
   public enum WaveType {
      Intro, Planned, Scaled
   };
   public WaveType currentWaveType;

   //counters for each type of wave
   public int introCount = 0;
   public int plannedCount = 0;
   public int scaledCount = 0;

   private float maxShopTimer = 15.0f;
   private float shopTimer = 0.0f;

   //intro stuff
   private int easyEnemiesSpawned = 0;
   private int maxEasyEnemies = 2;
   private int orbitersSpawned = 0;
   private int maxOrbiters = 2;

   public int waveCount;

   //wave 5 - 

   //wave 6 -

   //wave 7 -

   //wave 8 -

   //wave 9 -

   //wave 10 - last planned wave. spawn a teleporting enemy



   //scaled stuff
   public bool spawnMode;
   public int maxEnemies; //increases as wavecount increases (totalEnemy)
   public int enemiesSpawned; //numEnemy
   private float spawnRate = 0.0f; //adjusted every wave depending on how many enemies
   private float nextSpawn = 0.0f; //time that the next enemy should spawn


   private string[] thoughts = {"They've stopped! I must find out where they are from.", 
      "Let's reload, I don't see anyone...",
      "I'm not detecting anything on my radar.",
      "All is silent on the front",
      "Get 'em! They have given up!"
   };
      
   private float radius = 50f;
  

   void Awake() 
   {
      //If we don't currently have a game control...
      if (instance == null)
      {
         //...set this one to be it...
         instance = this;
      }
      else if (instance != this)
      {
         //...destroy this one because it is a duplicate.
         Destroy(gameObject);
      }
   }

   void Start() {
      currentWaveType = WaveType.Intro;

      dynamicPool = (DynamicObjectPool)poolGameObject.GetComponent(typeof(DynamicObjectPool));

      waveCount = 1;
   }

   void Update() {
     
      switch (waveCount) {

      case 1: // intro wave to introduce asteroids
         //Asteroids instruction popup

         //move to the next wave when player has absorbed 5 asteroids
         if (PlayerController.instance.mass > 50) {
            waveCount++;

            //show text to show player is learning
            GameControl.instance.uiController.WriteThought("", "I'm growing stronger!", GameUIController.OUR_TEXT_COLOR, false);
           
         }
         break;

      case 2: //Introduce the enemy
         //spawn n enemies
         if (easyEnemiesSpawned < maxEasyEnemies) {
            SpawnEnemy (diveBomber);
            easyEnemiesSpawned++;
         } else if (dynamicPool.ActiveCount (diveBomber) == 0) {
            //both enemies have been destroyed
            waveCount++;

            //show text to show player is learning
            GameControl.instance.uiController.WriteThought("", "That was too easy! Send more!", GameUIController.OUR_TEXT_COLOR, false);


            //stuff for next wave
            //low spawn rate to make sure player gets hit
            spawnRate = 0.5f;
            nextSpawn = Time.time + spawnRate;
         }
         break;

      case 3: //Hard wave - player gets hit, introduce the shop system
         //move to next wave when player gets hit
         if (PlayerController.instance.beenHit) {
            waveCount++;
            //clear the enemies to be nice
            dynamicPool.ClearEnemies(diveBomber);

            //Shop Introduction popup
            GameControl.instance.ShopPopup ();

            GameControl.instance.uiController.WriteThought("", "What's this? A new enemy?", GameUIController.OUR_TEXT_COLOR, false);
         }

         //keep spawning enemies
         if (Time.time > nextSpawn) {
            SpawnEnemy (diveBomber);
            nextSpawn = Time.time + spawnRate;

         }
        
         break;

      case 4: // introduce the orbiter
         if (orbitersSpawned < maxOrbiters) {
            SpawnEnemy (orbiter);
            orbitersSpawned++;
         } else if (dynamicPool.ActiveCount (orbiter) == 0) {
            //next wave
            waveCount++;
            GameControl.instance.uiController.WriteThought("", "I'm ready for a real challenge!", GameUIController.OUR_TEXT_COLOR, false);
         }
         break;  

      case 5: //planned wave
         waveCount++;
         spawnMode = true;
         maxEnemies = waveCount * 2;
         spawnRate = 0.5f;
         enemiesSpawned = 0;
         nextSpawn = Time.time;

         break;

      default: //normal scaled mode
         if (spawnMode) {
            //spawn enemies or check if all have been destroyed
            if (enemiesSpawned < maxEnemies && Time.time > nextSpawn) {

               //decide what type of enemy to spawn


               SpawnEnemy (diveBomber);
               nextSpawn = Time.time + spawnRate;
               enemiesSpawned++;
            } else if (dynamicPool.ActiveCount (diveBomber) == 0 && dynamicPool.ActiveCount (orbiter) == 0
                       && dynamicPool.ActiveCount (teleportingBoss) == 0) {

               spawnMode = false;
               shopTimer = 0;

               GameControl.instance.uiController.WriteThought ("", thoughts [0], GameUIController.OUR_TEXT_COLOR, false);


            }
         } else {
            //allow player to shop
            shopTimer += Time.deltaTime;

            if (shopTimer > maxShopTimer) {
               spawnMode = true;
               waveCount++;

               maxEnemies = waveCount * 2;
               spawnRate = 0.5f;
               enemiesSpawned = 0;
               nextSpawn = Time.time;
            }
         }
         break;

      }
   }

   public static GameObject GetPooledObject(List<GameObject> collection) {
      for (int i = 0; i < collection.Count; i++) {
         if (!collection[i].activeInHierarchy) {
            return collection[i];
         }
      }
      return null;
   }

   public void SpawnEnemy (GameObject enemyObject)
   {
      // spawn an enemy

      float randX, randY;
      Vector2 spawnPos;

      float theta = Random.Range (0f, 360f);
      float r = radius; //rand later
      float radians = theta * Mathf.Deg2Rad; 
      spawnPos = new Vector2 (r * Mathf.Sin (radians), r * Mathf.Cos (radians));

      GameObject tmp = dynamicPool.GetPooledObject (enemyObject);
      tmp.SetActive (true);
      tmp.transform.position = spawnPos;
      Vector2 direction = Vector2.zero - spawnPos;
      tmp.GetComponent<EnemyController> ().Reset ();
      tmp.GetComponent<Rigidbody2D> ().AddForce (direction.normalized * 300f);
   }

  
}