using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3EnemyController : EnemyController
{

   private Transform target;
   private Vector2 dir;
   private Vector2 forward;
   private Vector2 side;
   private Vector2 cross;
   private Rigidbody2D rigidbody;
   
   public int x;
   public int y;
   public int alpha;
   public int a;
   public int X;
   public int Y;
   public int b;
   void Start()
   {
      //rigidbody = GetComponent<Rigidbody2D>();
      //target = PlayerController.instance.transform;
      //forward = transform.forward; //forward vector just to have it (in 3d you need a plane to calculate normalvector, this will be one side of the plane)
      //dir = target.transform.position - transform.position; //direction from your object towards the target object what you will orbit (the other side of the plane)
      //side = Vector3.Cross(dir, forward); //90 degrees (normalvector) to the plane closed by the forward and the dir vectors
   }

   /*void Update()
   {
      alpha += 10;
      X = x + (a * (int)Mathf.Cos(alpha * .005f));
      Y = y + (b * (int)Mathf.Sin(alpha * .005f));
      transform.position = target.transform.position + Vector3.Lerp(X,0,Y);
      /*dir = target.transform.position - transform.position;
      dir.Normalize();
      cross.x = dir.y;
      cross.y = -dir.x;
      //transform.LookAt(dir); // look at the target
      rigidbody.velocity = new Vector2(0,0);
      //rigidbody.velocity += dir * Vector2.Distance(target.position, transform.position);
      rigidbody.AddForce(dir * (Vector2.Distance(target.position, transform.position)));//add gravity like force pulling your object towards the target
      //cross = Vector3.Cross(dir, side); //90 degrees vector to the initial sideway vector (orbit direction) /I use it to make the object orbit horizontal not vertical as the vertical lookatmatrix is flawed/
      cross.Normalize();
      rigidbody.velocity += cross * 10;
      //rigidbody.AddForce(cross * 3); //add the force to make your object move (orbit)
   
   }*/
}
