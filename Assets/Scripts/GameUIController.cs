﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour {
   //public GameObject pauseButton;
   //public GameObject shopButton;
   public GameObject upgradeButton;

   public GameObject pauseMenu;
   public GameObject shopMenu;
   public GameObject upgradeMenu;

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

   public void ToggleShopMenu(bool open) {
      shopMenu.SetActive (open);
      Time.timeScale = open ? 0 : 1F;
      GameControl.instance.togglePauseGame(open);
   }

   public void ToggleUpgradeMenu(bool open) {
      upgradeMenu.SetActive (open);
      Time.timeScale = open ? 0 : 1F;
      GameControl.instance.togglePauseGame(open);
   }

   public void ToggleUpgradeButton(bool open) {
      upgradeButton.SetActive (open);
   }
}
