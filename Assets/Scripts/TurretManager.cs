using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour {

   public static TurretManager instance;

   public GameObject poolGameObject;
   public GameObject autoObject;
   public GameObject multiObject;
   public GameObject laserObject;

   private DynamicObjectPool dynamicPool;

   void Awake() {
      if (instance == null)
         instance = this;
      else if (instance != this)
         Destroy(gameObject);
   }

	// Use this for initialization
	void Start () {
      //pooledTurrets = new List<TurretController> ();
      dynamicPool = (DynamicObjectPool)poolGameObject.GetComponent(typeof(DynamicObjectPool));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public void SpawnTurret(string tag) {
      GameObject turretToSpawn;
      switch (tag) {
      case "AutoTurret":
         turretToSpawn = autoObject;
         break;
      case "LaserTurret":
         turretToSpawn = laserObject;
         break;
      case "MultiShotTurret":
         turretToSpawn = multiObject;
         break;
      default:
         return;
      }

      GameObject tmp = dynamicPool.GetPooledObject(turretToSpawn);
      tmp.SetActive(true);

   }
}
