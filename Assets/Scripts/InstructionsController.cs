using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsController : MonoBehaviour {

   public Text subHeader;
   public Text instructions;
   public GameObject nextButtonObj;
   public GameObject prevButtonObj;
   
   private List<List<string>> headersAndInfo = new List<List<string>>();
   private int currentInstr = 0;
   
   // Use this for initialization
   void Start () {
      Time.timeScale = 0.0f;
      
      addHeaderAndInstr("Firing", "You can control your central turret with the mouse (click to fire at the mouse position).");
      addHeaderAndInstr("Enemies", "Enemies will attack your planet in waves, defend yourself using your central turret and auto turrets.");
      
      displayCurrentHeaderInstr();
   }
   
   // Update is called once per frame
   void Update () {
      
   }
   
   void addHeaderAndInstr(string hdr, string instr) {
      headersAndInfo.Add(new List<string>(new string [] {hdr, instr}));
   }
   
   void displayCurrentHeaderInstr() {
      subHeader.text = headersAndInfo[currentInstr][0];
      instructions.text = headersAndInfo[currentInstr][1];
   }
   
   public void nextButton() {
      if (currentInstr >= headersAndInfo.Count - 1) return;
      
      currentInstr++;
      prevButtonObj.SetActive(true);
      if (currentInstr == headersAndInfo.Count - 1) {
         nextButtonObj.SetActive(false);
      }
      displayCurrentHeaderInstr();
   }
   
   public void prevButton() {
      if (currentInstr == 0) return;
      
      currentInstr--;
      nextButtonObj.SetActive(true);
      if (currentInstr == 0) {
         prevButtonObj.SetActive(false);
      }
      displayCurrentHeaderInstr();
   }
   
   public void closeButton() {
      gameObject.SetActive(false);
      Time.timeScale = 1.0f;
   }
}
