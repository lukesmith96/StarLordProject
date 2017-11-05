using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
   public GameObject enemylevel1Object;
   public GameObject enemylevel2Object;
   public GameObject explosionObject;
   public int count = 0;
   public int poolSize;
   public float radius = 30f;

   //spawn every n seconds
   public float spawnRate = 2f;
   private float nextSpawn = 0.0f;
   
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
      //time to spawn an enemy
      if (Time.time > nextSpawn && !GameControl.instance.gameIsPaused) {
         float randX, randY;
         Vector2 spawnPos;

         nextSpawn = Time.time + spawnRate;

         //get random x and y within a certain range
         //randX = Random.Range (-8.4f, 8.4f);
         //randY = Random.Range (-8.4f, 8.4f);
         float theta = Random.Range(0f, 360f);
         float r = radius; //rand later
         float radians = theta * Mathf.Deg2Rad; 
         spawnPos = new Vector2(r * Mathf.Sin(radians), r * Mathf.Cos(radians));

         //spawn enemy
         GameObject tmp = dynamicPool.GetPooledObject(enemylevel2Object);
         tmp.SetActive (true);
         tmp.transform.position = spawnPos;
         Vector2 direction = Vector2.zero - spawnPos;
         tmp.GetComponent<EnemyController>().Reset();
         tmp.GetComponent<Rigidbody2D>().AddForce(direction.normalized * 300f);
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
}
