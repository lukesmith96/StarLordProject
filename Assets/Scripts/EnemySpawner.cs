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

   public List<GameObject> pooledEnemies;
   public List<GameObject> pooledExplosions;
   public GameObject poolGameObject;
   private DynamicObjectPool dynamicPool;
   public GameObject enemyObject;
   public GameObject explosionObject;
   public int poolSize;
   public float radius = 30f;
   public Text shopTimerText;
   public Text waveCountText;

   //spawn every n seconds
   public float nextSpawn = 0.0f;

   // wave stuff
   public bool spawnMode = false;
   public float maxShopTime = 15.0f;
   public float shopTimer = 0.0f;
   private int waveCount = 0;
   public int totalEnemy = 0;
   public int numEnemy = 0;
   public float spawnRate = 0.0f;
   public bool tutorialMode = false;


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
      dynamicPool = (DynamicObjectPool)poolGameObject.GetComponent(typeof(DynamicObjectPool));

      for (int i = 0; i < poolSize; i++) {
         GameObject obj = (GameObject)Instantiate(explosionObject);
         obj.SetActive(false);
         pooledExplosions.Add(obj);
      }
   }

   void Update() {

      if (spawnMode) {

         switch (waveCount) {

         default:
            if (numEnemy < totalEnemy) {
               if (Time.time > nextSpawn) {
                  SpawnEnemy ();
                  nextSpawn = Time.time + spawnRate;
                  numEnemy++;
               }
            } else if (dynamicPool.ActiveCount (enemyObject) == 0) {
               spawnMode = false;
               shopTimer = 0;

               //enable shop button
            }
            break;
         }
      } 
      else {
         if (!tutorialMode) {
            shopTimer += Time.deltaTime;
            shopTimerText.text = "Next wave in: " + (int)(maxShopTime - shopTimer);

         }

         if (shopTimer > maxShopTime) {
            // stop shopping and prepare for the wave
            shopTimerText.text = "Wave in progress";
            spawnMode = true;

            waveCountText.text = "Wave: " + waveCount;
            waveCount++;

            totalEnemy = waveCount * 2;   //calculate num enemies to spawn
            spawnRate = 0.5f;              //calculate spawn rate 
            numEnemy = 0;
            nextSpawn = Time.time + spawnRate;
         } 
         else {
            // do shop stuff


         }
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

   public void SpawnEnemy ()
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