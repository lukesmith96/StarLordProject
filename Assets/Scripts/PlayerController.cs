﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour {

   public float scaleValue = .01F;
   public static PlayerController instance;
   public float rotationTime = .5f;
   public Vector3 newScale;

   private bool isColliding;
   private bool rotatePU;
   private float currRotation;
   private Vector3 currScale;
   public int mass;// -100 is min value at that point die
   private bool god = false;
   public bool beenHit = false;

   // Use this for initialization
   void Start () {
      rotatePU = false;
      instance = this;
      currScale = newScale = transform.localScale;
      mass = 0;
      GameControl.instance.SetMassText();
   }

   // Update is called once per frame
   void Update () {
      
      //scalePlayer ();
      // Interpolate to newScale
      //Vector3 actualScale = Vector3.Lerp (currScale, newScale, 1.0f * Time.deltaTime);
      //transform.localScale = actualScale;
      //currScale = actualScale;

      // rotation controls:
      if (rotatePU && currRotation < (Time.time - rotationTime))
      {
         rotatePU = false;
      }
      if (Input.GetKey(KeyCode.UpArrow) && rotatePU)
      {
         transform.Rotate(new Vector3(0, 0, 45) * (Time.deltaTime * transform.localScale.x));
      }
      if (Input.GetKey(KeyCode.DownArrow) && rotatePU)
      {
         transform.Rotate(new Vector3(0, 0, -45) * (Time.deltaTime * transform.localScale.x));
      }
      isColliding = false;
      float s = mass + 100;
      if (s > 1100.0f) s = 1100.0f;
      transform.localScale = new Vector3((scaleValue * s), (scaleValue * s), 1);
   }

   void FixedUpdate() {
   }
      
   void OnTriggerEnter2D(Collider2D other) {
      // Joshua King
      // If an enemy collides with the player, the player loses score
      if (other.gameObject.CompareTag ("Enemy")) {
         other.gameObject.SetActive (false);
         if (isColliding)
            return;
         isColliding = true;
         //transform.localScale -= (new Vector3 (scaleValue, scaleValue, 0) * Time.deltaTime);
         reduceMass(10);
         beenHit = true;

      } else if (other.gameObject.CompareTag ("Asteroid")) {
         // Joshua King
         // If an asteroid comes in contact with the player, the player gains mass
         other.gameObject.SetActive (false);
         if (isColliding)
            return;
         isColliding = true;

         GameControl.instance.score += 10;
         GameControl.instance.SetScoreText();
         addMass(10);
      }
   }

   public void invokeGodMode()
   {
      
   }

   public void scalePlayer() {
      float s = mass;
      if (s > 1000.0f) s = 1000.0f;
      newScale = new Vector3(scaleValue * s, scaleValue * s, 0);
   }

   public void addMass(int add)
   {
      mass += add;
      GameControl.instance.SetMassText();
   }

   public bool reduceMass(int reduce)
   {
      if (mass - reduce < -100)
         return false;
      mass -= reduce;
      GameControl.instance.SetMassText();
      return true;
   }

   public void startRotationPU()
   {
      rotatePU = true;
      currRotation = Time.time;
   }
}