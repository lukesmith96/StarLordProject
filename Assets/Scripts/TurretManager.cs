using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour {

   public static TurretManager instance;
   public List<TurretController> pooledTurrets;

   public AutoTurretController autoPrefab;
   public AutoTurretController multiPrefab;
   public AutoTurretController laserPrefab;

   void Awake() {
      if (instance == null)
         instance = this;
      else if (instance != this)
         Destroy(gameObject);
   }

	// Use this for initialization
	void Start () {
      pooledTurrets = new List<TurretController> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public void SpawnTurret(string tag) {
      foreach (AutoTurretController turret in pooledTurrets) {
         if (turret.CompareTag(tag) && !turret.gameObject.activeInHierarchy) {
            turret.gameObject.SetActive (true);
            Debug.Log ("Found existing turret in pool, using that instead");
            // Also reset turret stats if upgraded
            turret.Reset();

            return;
         }
      }

      AutoTurretController turretToSpawn;
      switch (tag) {
      case "AutoTurret":
         turretToSpawn = autoPrefab;
         break;
      case "LaserTurret":
         turretToSpawn = laserPrefab;
         break;
      case "MultiShotTurret":
         turretToSpawn = multiPrefab;
         break;
      default:
         return;
      }

      AutoTurretController newTurret = Instantiate (turretToSpawn, Vector3.zero, Quaternion.identity);
      newTurret.gameObject.SetActive (true);

      pooledTurrets.Add (newTurret);
      Debug.Log ("Created new Turret, size of list: " + pooledTurrets.Count);
   }
}
