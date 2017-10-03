using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
   public float maxRange = 20f;
   public Vector2 originPoint = Vector2.zero;
   
   private Rigidbody2D rb2d;
   // Use this for initialization
   void Start()
   {
      rb2d = GetComponent<Rigidbody2D>();
      originPoint = transform.position;
   }
   
   // Update is called once per frame
	void Update () {
      if (Vector2.Distance(originPoint, transform.position) >= maxRange) {
         Destroy(gameObject);
      }
	}

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("Enemy"))
      {
         other.gameObject.SetActive(false);
         this.gameObject.SetActive(false);
      }
      if (other.gameObject.CompareTag("Player") && originPoint != Vector2.zero) {
         this.gameObject.SetActive(false);
      }
   }
}
