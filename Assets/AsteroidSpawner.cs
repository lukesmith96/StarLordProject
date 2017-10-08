using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author: Luke Smith
 * @since: 10.1.17
 * 
 * This class controls random generation of astroids in game to increase player size and hp
 */
public class AsteroidSpawner : MonoBehaviour
{

   public List<GameObject> pooledEnemies;
   public GameObject asteroidObject;
   public int count = 0;
   public int poolSize = 1;
   public float radius = 30f;

   //spawn every n seconds
   public float spawnRate = 2f;
   private float nextSpawn = 0.0f;

   void Start()
   {
      for (int i = 0; i < poolSize; i++)
      {
         GameObject obj = (GameObject)Instantiate(asteroidObject);
         obj.SetActive(false);
         pooledEnemies.Add(obj);
      }
   }

   void Update()
   {
      //time to spawn an enemy
      if (Time.time > nextSpawn)
      {
         float randX, randY;
         Vector2 spawnPos;

         nextSpawn = Time.time + spawnRate;

         //get random x and y within a certain range
         float theta = Random.Range(0f, 360f);
         float r = radius; //rand later
         float radians = theta * Mathf.Deg2Rad;
         spawnPos = new Vector2(r * Mathf.Sin(radians), r * Mathf.Cos(radians));

         //spawn enemy
         GameObject tmp = GetPooledObject();
         tmp.SetActive(true);
         tmp.transform.position = spawnPos;
      }
   }

   public GameObject GetPooledObject()
   {
      for (int i = 0; i < pooledEnemies.Count; i++)
      {
         if (!pooledEnemies[i].activeInHierarchy)
         {
            return pooledEnemies[i];
         }
      }
      return null;
   }
}
