using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour {

   public GameObject centerTurret;
   public Text dmgText;
   public Text reloadText;
   public Text bulletText;

   public int dmgCost = 50;
   public int dmgRefund = 5;

   public int reloadCost = 100;
   public int reloadRefund = 10;

   public int bulletCost = 200;
   public int bulletRefund = 20;

   public float dmgIncrement = 1f;
   public float reloadIncrement = 0.2f;
   public int bulletIncrement = 1;

   void Start() {
      setDmgText ();
      setReloadText ();
      setBulletText ();
   }

   void OnEnable() {
      setDmgText ();
      setReloadText ();
      setBulletText ();
   }

   public void UpgradeDamage() {
      if (decrementScore (dmgCost)) {
         centerTurret.GetComponent<CenterTurretController> ().collisionDamage += dmgIncrement;
         setDmgText ();
      } 
   }

   public void UpgradeReload() {
      if (decrementScore (reloadCost)) {
         centerTurret.GetComponent<CenterTurretController> ().reloadTime -= reloadIncrement;
         setReloadText ();
         Debug.Log ("New reload time: " + centerTurret.GetComponent<CenterTurretController> ().reloadTime);
      }
   }

   public void UpgradeBullet() {
      if (decrementScore (bulletCost)) {
         centerTurret.GetComponent<CenterTurretController> ().numBullets += bulletIncrement;
         setBulletText ();
      }
   }

   public void RefundDamage() {
      if (centerTurret.GetComponent<CenterTurretController> ().collisionDamage <= centerTurret.GetComponent<CenterTurretController> ().initDmgMult)
         return;
      
      centerTurret.GetComponent<CenterTurretController> ().collisionDamage -= dmgIncrement;
      incrementScore (dmgRefund);

      setDmgText ();
   }

   public void RefundReload() {
      if (centerTurret.GetComponent<CenterTurretController> ().reloadTime >= centerTurret.GetComponent<CenterTurretController> ().initReloadMult)
         return;

      centerTurret.GetComponent<CenterTurretController> ().reloadTime += reloadIncrement;
      incrementScore (reloadRefund);

      setReloadText ();
   }

   public void RefundBullet() {
      if (centerTurret.GetComponent<CenterTurretController> ().numBullets <= centerTurret.GetComponent<CenterTurretController> ().initNumMult)
         return;

      centerTurret.GetComponent<CenterTurretController> ().numBullets -= bulletIncrement;
      incrementScore (bulletRefund);

      setBulletText ();
   }

   private bool decrementScore(int cost) {
      if (PlayerController.instance.mass - cost < 0) {
         return false;
      }

      PlayerController.instance.mass -= cost;
      GameControl.instance.SetMassText ();

      return true;
   }

   private void incrementScore(int refund) {
      PlayerController.instance.mass += refund;
      GameControl.instance.SetMassText ();
   }

   private void setDmgText() {
      float curDmg = centerTurret.GetComponent<CenterTurretController> ().collisionDamage / centerTurret.GetComponent<CenterTurretController> ().initDmgMult;
      if (curDmg == Mathf.Infinity)
         curDmg = 1;

      dmgText.text = "x" + curDmg;
   }

   private void setReloadText() {
      float curReload =  centerTurret.GetComponent<CenterTurretController> ().reloadTime;
      if (curReload == Mathf.Infinity)
         curReload = 1;

      reloadText.text = "x" + Mathf.Abs(curReload);
   }

   private void setBulletText() {
      float curNum = centerTurret.GetComponent<CenterTurretController> ().numBullets / (float) centerTurret.GetComponent<CenterTurretController> ().initNumMult;
      if (curNum == Mathf.Infinity)
         curNum = 1;

      bulletText.text = "x" + curNum;
   }
}
