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
         turretToSpawn = null;
      }

      // Otherwise, remove turret
      if (turretToSpawn != null && !turretToSpawn.GetComponent<AutoTurretController> ().isTouching) {
         RemoveTurret ();
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
      if (turretToSpawn != null) {
         turretToSpawn.SetActive (true);
         turretToSpawn.GetComponent<AutoTurretController> ().Reset ();
      }
   }

   public void RemoveTurret() {
      if (turretToSpawn != null) {
         turretToSpawn.SetActive (false);
         isDragged = false;
         turretToSpawn = null;
      }
   }
}
