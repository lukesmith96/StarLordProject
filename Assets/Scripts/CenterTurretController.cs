using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CenterTurretController : TurretController {

   public GameObject reloadIcon;

   // Use this for initialization
   new void Start () {
      base.Start();
   }
   
   // Update is called once per frame
   new void Update () {
      base.Update();
      
      Vector2 target = MouseControl.GetWorldPositionOnPlane(Input.mousePosition, 0f);
      Vector2 turret = transform.position;
      Vector2 direction = new Vector2(target.x - turret.x, target.y - turret.y);
      direction.Normalize();
      
      //make turret point towards mouse
      transform.up = direction;
      
      // Check if player clicks mouse in game
      if (Input.GetMouseButtonDown(0))
      {
         //if (EventSystem.current.IsPointerOverGameObject() == false)
         FireBullet(direction);
      }
      
      if (timeSinceFiring >= reloadTime) {
         reloadIcon.SetActive(false);
      } else {
         reloadIcon.SetActive(true);
      }
   }
}
