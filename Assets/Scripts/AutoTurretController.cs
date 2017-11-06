using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretController : TurretController {
   public bool isTouching;

   private SpriteRenderer sprite;
   private Rigidbody2D rb2d;

   private bool isAttached;

   private Vector3 defaultScale = new Vector3(1, 1, 0);
   
   private Vector3 scale = new Vector3(1, 1, 1);
   
   // Use this for initialization
   new void Start () {
      base.Start();

      sprite = GetComponent<SpriteRenderer> ();
      rb2d = GetComponent<Rigidbody2D> ();

      isAttached = false;
      isTouching = false;
      isInvincible = true;

      //scale = transform.localScale;
   }
   
   // Update is called once per frame
   new void Update () {
      base.Update();



      if (isAttached) {
         sprite.color = Color.white;
      } else if (isTouching) {
         sprite.color = Color.green;
      } else {
         sprite.color = Color.red;
      }

      if (isAttached) {
      
         AutoFire ();

      }

      // Reset scale if attached to player
      ResetScale();
   }
   
   public void AttachToPlayer() {
      Debug.Log ("ATTACHED");

      transform.SetParent (player.transform);

      isAttached = true;
      isInvincible = false;

      rb2d.isKinematic = true;
   }

   private void ResetScale() {
      transform.parent = null;
      transform.localScale = defaultScale;
      transform.SetParent (player.transform);
   }
   
   void OnCollisionEnter2D(Collision2D other) {
      if (other.gameObject.CompareTag ("Turret")) {
         isTouching = false;
      } else if (other.gameObject.CompareTag ("Player")) {
         isTouching = true;
      }

   }
   
   void OnCollisionExit2D(Collision2D other) {
      if (other.gameObject.CompareTag ("Player")) 
      {
         isTouching = false;
      }
   }

   public void Reset() {
      isAttached = isTouching = false;
      isInvincible = true;
      transform.position = Vector3.zero;

      ResetScale();

      rb2d.isKinematic = false;
   }
}
