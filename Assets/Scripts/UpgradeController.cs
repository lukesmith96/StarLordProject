using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour {

   public GameObject centerTurret;
   public Text dmgText;
   public Text reloadText;
   public Text bulletText;

   public int refundAmt = 10;
   public int upgradeCost = 50;

   public float dmgIncrement = 1f;
   public float reloadIncrement = 1f;
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
      centerTurret.GetComponent<CenterTurretController> ().collisionDamage += dmgIncrement;
      decrementScore ();

      setDmgText ();
   }

   public void UpgradeReload() {
      centerTurret.GetComponent<CenterTurretController> ().reloadTime -= reloadIncrement;
      decrementScore ();

      setReloadText ();
   }

   public void UpgradeBullet() {
      centerTurret.GetComponent<CenterTurretController> ().numBullets += bulletIncrement;
      decrementScore ();

      setBulletText ();
   }

   public void RefundDamage() {
      if (centerTurret.GetComponent<CenterTurretController> ().collisionDamage <= centerTurret.GetComponent<CenterTurretController> ().initDmgMult)
         return;
      
      centerTurret.GetComponent<CenterTurretController> ().collisionDamage -= dmgIncrement;
      incrementScore ();

      setDmgText ();
   }

   public void RefundReload() {
      if (centerTurret.GetComponent<CenterTurretController> ().reloadTime <= centerTurret.GetComponent<CenterTurretController> ().initReloadMult)
         return;

      centerTurret.GetComponent<CenterTurretController> ().reloadTime += reloadIncrement;
      incrementScore ();

      setReloadText ();
   }

   public void RefundBullet() {
      if (centerTurret.GetComponent<CenterTurretController> ().numBullets <= centerTurret.GetComponent<CenterTurretController> ().initNumMult)
         return;

      centerTurret.GetComponent<CenterTurretController> ().numBullets -= bulletIncrement;
      incrementScore ();

      setBulletText ();
   }

   private void decrementScore() {
      if (GameControl.instance.score - upgradeCost < 0) {
         return;
      }

      GameControl.instance.score -= upgradeCost;
      GameControl.instance.SetScoreText ();
   }

   private void incrementScore() {
      GameControl.instance.score += refundAmt;
      GameControl.instance.SetScoreText ();
   }

   private void setDmgText() {
      float curDmg = centerTurret.GetComponent<CenterTurretController> ().collisionDamage / centerTurret.GetComponent<CenterTurretController> ().initDmgMult;
      if (curDmg == Mathf.Infinity)
         curDmg = 1;

      dmgText.text = "x" + curDmg;
   }

   private void setReloadText() {
      float curReload = centerTurret.GetComponent<CenterTurretController> ().reloadTime / centerTurret.GetComponent<CenterTurretController> ().initReloadMult;
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
