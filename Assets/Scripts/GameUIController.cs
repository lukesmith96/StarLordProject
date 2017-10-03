﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour {
   public GameObject pauseMenu;
   
   // Use this for initialization
   void Start () {
      
   }
   
   // Update is called once per frame
   void Update () {
      
   }
   
   public void LoadStage(string target) {
      SceneManager.LoadScene(target);
   }
   
   public void TogglePauseMenu(bool open) {
      pauseMenu.SetActive(open);
      Time.timeScale = open ? 0 : 1F;
      GameControl.instance.togglePauseGame(open);
   }
}
