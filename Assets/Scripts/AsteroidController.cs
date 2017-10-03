using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {
   private Rigidbody2D rb2d;
   public float randomOffset;
   public int speed;


   void Start()
   {
      rb2d = GetComponent<Rigidbody2D>();
      Vector2 target = MouseControl.GetWorldPositionOnPlane(new Vector2(0, 0), 0f);
      Vector2 current = transform.position;
      Vector2 vectorToOrigin = Vector2.MoveTowards(-current, target, 3 * Time.deltaTime) * speed;
      vectorToOrigin.x += Random.Range(25, randomOffset);
      vectorToOrigin.y += Random.Range(25, randomOffset);

      rb2d.AddForce(vectorToOrigin);
   }

   void FixedUpdate()
   {
   }

   void Update()
   {
   }
}
