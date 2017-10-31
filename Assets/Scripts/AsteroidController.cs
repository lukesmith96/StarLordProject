using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * @author: Luke Smith
 * @since: 10.1.17 
 */
public class AsteroidController : MonoBehaviour {
   private Rigidbody2D rb2d;

   public Vector2 originPoint = Vector2.zero;
   public float maxRange = 40f;
   public float randomOffset;
   public int speed;


   /*void Start()
   {
      rb2d = GetComponent<Rigidbody2D>();
      Vector2 target = MouseControl.GetWorldPositionOnPlane(new Vector2(0, 0), 0f);
      Vector2 current = transform.position;
      Vector2 vectorToOrigin = Vector2.MoveTowards(-current, target, 3 * Time.deltaTime) * speed;
      vectorToOrigin.x += Random.Range(25, randomOffset);
      vectorToOrigin.y += Random.Range(25, randomOffset);

      rb2d.velocity = Vector2.zero;
      rb2d.AddForce(vectorToOrigin);
   }*/

   void OnEnable()
   {
      rb2d = GetComponent<Rigidbody2D>();
      Vector2 target = MouseControl.GetWorldPositionOnPlane(new Vector2(0, 0), 0f);
      Vector2 current = transform.position;
      Vector2 vectorToOrigin = Vector2.MoveTowards(-current, target, 3 * Time.deltaTime) * speed;
      vectorToOrigin.x += Random.Range(25, randomOffset);
      vectorToOrigin.y += Random.Range(25, randomOffset);

      rb2d.velocity = Vector2.zero;
      rb2d.AddForce(vectorToOrigin);
   }

   void FixedUpdate()
   {
   }

   void Update()
   {
      if (Vector2.Distance(originPoint, transform.position) >= maxRange)
      {
         gameObject.SetActive(false);
      }
   }
}
