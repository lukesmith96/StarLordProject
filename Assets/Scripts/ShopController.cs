using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

   public GameObject poolGameObject;
   public GameObject turretType;
   public int turretCost = 50;

   private DynamicObjectPool dynamicPool;
   private GameObject turretToSpawn;
   private bool isDragged = false;
   private bool onImage = false;

	// Use this for initialization
	void Start () { 
      dynamicPool = (DynamicObjectPool)poolGameObject.GetComponent(typeof(DynamicObjectPool));
      turretToSpawn = null;
   }
	
	// Update is called once per frame
	void Update () {  }

   void FixedUpdate() {
      if (turretToSpawn != null && isDragged) {
         Vector2 target = MouseControl.GetWorldPositionOnPlane(Input.mousePosition, 0f);

         turretToSpawn.transform.position = target;
         turretToSpawn.GetComponent<CircleCollider2D> ().transform.position = target;
      }
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      onImage = true;
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      onImage = false;
   }

   public void OnPointerUp(PointerEventData eventData) {
      // Place turret if touching player
      if (turretToSpawn != null && turretToSpawn.GetComponent<AutoTurretController> ().isTouching) {
         AutoTurretController newTurret = turretToSpawn.GetComponent<AutoTurretController> ();

         newTurret.AttachToPlayer ();


         GameControl.instance.score -= turretCost;
         GameControl.instance.SetScoreText ();

         isDragged = false;
         //turretToSpawn = null;
      }

      // Otherwise, remove turret
      if (turretToSpawn != null && !turretToSpawn.GetComponent<AutoTurretController> ().isTouching) {
         turretToSpawn.SetActive (false);
         isDragged = false;
      }
   }

   public void OnPointerDown(PointerEventData eventData) {
      if (onImage && !isDragged) {
         isDragged = true;
         GetTurret ();
      }
   }

   public void GetTurret() {
      turretToSpawn = dynamicPool.GetPooledObject(turretType);
      turretToSpawn.SetActive (true);
      if (turretToSpawn != null)
         turretToSpawn.GetComponent<AutoTurretController> ().Reset ();
   }

   /*
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
   }*/
}
