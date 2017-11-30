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
   public GameObject level3;

   private Tuple<float, float, float>[] waveDifficulty = {
      new Tuple<float, float, float>(.8f, .2f, 0),
      new Tuple<float, float, float>(.75f, .25f, 0),
      new Tuple<float, float, float>(.70f, .30f, 0),
      new Tuple<float, float, float>(.65f, .35f, 0),
      new Tuple<float, float, float>(.60f, .40f, 0),
      new Tuple<float, float, float>(.55f, .35f, .1f),
      new Tuple<float, float, float>(.50f, .35f, .15f),
      new Tuple<float, float, float>(.50f, .30f, .2f),
      new Tuple<float, float, float>(.40f, .30f, .3f),
      new Tuple<float, float, float>(.45f, .35f, .2f),
      new Tuple<float, float, float>(.60f, .1f, .3f),
      new Tuple<float, float, float>(.60f, 0f, .4f),
      new Tuple<float, float, float>(.2f, .4f, .4f),
      new Tuple<float, float, float>(0f, .5f, .5f),
      new Tuple<float, float, float>(.33f, .33f, .33f)
   };

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
   private int maxWaveCount = 4;
   private int wavespawncount = 0;
   private bool bossSpawned = false;

   public int waveCount;

   //scaled stuff
   public bool spawnMode;
   public int maxEnemies; //increases as wavecount increases (totalEnemy)
   public int enemiesSpawned; //numEnemy
   private float spawnRate = 0.0f; //adjusted every wave depending on how many enemies
   private float nextSpawn = 0.0f; //time that the next enemy should spawn


   private string[] waveOverThoughts = {
      "They've stopped! I must find out where they are from.", 
      "Reload! I don't see anyone...",
      "I'm not detecting anything on my radar.",
      "All is silent on the frontline!",
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

      if (GameControl.instance.godmode == true) {
         if (Input.GetKeyDown (KeyCode.UpArrow)) {
            waveCount++;
         }
      }

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
            GameControl.instance.ShopPopup ("ShopInstructions");
            
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
            spawnMode = true;
            GameControl.instance.uiController.WriteThought("", "I'm ready for a real challenge!", GameUIController.OUR_TEXT_COLOR, false);
         }
         break;

      case 19:
         if (spawnMode)
         {
            SpawnEnemy(teleportingBoss);
         }
         goto case 18;
      case 5: case 6: case 7: case 8: case 9: case 10: case 11: case 12:
         case 13: case 14: case 15: case 16: case 17: case 18://planned wave
         if (spawnMode)
         {
            if (wavespawncount < maxWaveCount && Time.time > nextSpawn)
            {
               float percentBomber = waveDifficulty[waveCount - 5].Get(0);
               float percentOrbital = waveDifficulty[waveCount - 5].Get(1);
               float percentLevel3 = waveDifficulty[waveCount - 5].Get(2);
               float value = Random.value;
               if (value < percentBomber)
               {
                  SpawnEnemy(diveBomber);
               }
               else if (value - percentBomber < percentOrbital)
               {
                  SpawnEnemy(orbiter);
               }
               else if (value - percentBomber - percentOrbital < percentLevel3)
               {
                  SpawnEnemy(level3);
               }
               spawnRate = 0.5f;
               nextSpawn = Time.time + spawnRate;
               wavespawncount++;
            }
            else if (dynamicPool.ActiveCount(orbiter) == 0 && dynamicPool.ActiveCount(diveBomber) == 0)
            {
               spawnMode = false;
               shopTimer = 0;

               int thoughtValue = Random.Range(0, waveOverThoughts.Length);
               GameControl.instance.uiController.WriteThought("", waveOverThoughts[thoughtValue], GameUIController.OUR_TEXT_COLOR, false);
            }
         }
         else {
            //allow player to shop
            shopTimer += Time.deltaTime;
            if (shopTimer > maxShopTimer)
            {
               spawnMode = true;
               //next wave
               waveCount++;
               maxWaveCount += 2;
               wavespawncount = 0;
               nextSpawn = Time.time;
            }
         }
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

               int thoughtValue = Random.Range(0, waveOverThoughts.Length);
               GameControl.instance.uiController.WriteThought("", waveOverThoughts[thoughtValue], GameUIController.OUR_TEXT_COLOR, false);
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

   private class Tuple<T1, T2, T3>
   {
      private float v1;
      private float v2;
      private float v3;

      public Tuple(float v1, float v2, float v3)
      {
         this.v1 = v1;
         this.v2 = v2;
         this.v3 = v3;
      }
      public float Get(int i)
      {
         if (i == 0)
            return v1;
         else if (i == 1)
            return v2;
         else
            return v3;
      }
   }
}