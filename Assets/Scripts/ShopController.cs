using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour {

   public int AutoTurretCost = 10;
   public int MultiTurretCost = 25;
   public int LaserTurretCost = 50;

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () { }

   public void PurchaseAutoTurret() {
      TurretManager.instance.SpawnTurret ("AutoTurret");
      GameControl.instance.score -= AutoTurretCost;
      GameControl.instance.SetScoreText ();
   }

   public void PurchaseMultiTurret() {
      TurretManager.instance.SpawnTurret ("MultiShotTurret");
      GameControl.instance.score -= MultiTurretCost;
      GameControl.instance.SetScoreText ();
   }

   public void PurchaseLaserTurret() {
      TurretManager.instance.SpawnTurret ("LaserTurret");
      GameControl.instance.score -= LaserTurretCost;
      GameControl.instance.SetScoreText ();
   }
}
