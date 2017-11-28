using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour 
{
   public static GameControl instance;
   
   //stats, must be global
   static bool Stat_init = false;
   public static int Stat_HighScore = 0;
   public static int Stat_EnemiesKilled = 0;
   public static int Stat_NumGamesPlayed = 0;

   public int score = 0;
   public int enemiesKilled = 0;
   public Text scoreText;
   public Text massText;
   public bool gameOver = false;
   public GameUIController uiController;
   public bool godmode = false;

   public bool gameIsPaused = false; //soft pause, not actually pausing time, just waves
   
   public GameObject beamObject;
   public GameObject centerTurretObject;
   public CenterTurretController centerTurretControl;
   
   void Awake()
   {
      //If we don't currently have a game control...
      if (instance == null)
      {
         //...set this one to be it...
         instance = this;
         ReadStats();
      }
      else if (instance != this)
      {
         //...destroy this one because it is a duplicate.
         Destroy(gameObject);
      }
   }

   void Start() {
      beamObject.SetActive(false);
      centerTurretObject.SetActive(false);
      
      SetScoreText();
      RunIntroCutscene();
   }

   void Update()
   {
      if (Input.GetKeyDown("space")) {
         ActivateGodmode();
      }
      if (PlayerController.instance.mass <= -100 && !gameOver)
      { // Player is Dead run death sequence
         gameOver = true;
         Debug.Log("Game Over");
         UpdateStats(GameControl.instance.score, GameControl.instance.enemiesKilled);
         togglePauseGame(true);
         Time.timeScale = 0.0f;
         //display game over popup
         uiController.DisplayGameOver(score, enemiesKilled);
      }
   }

   public void SetScoreText()
   {
      scoreText.text = "Score: " + GameControl.instance.score.ToString ();
   }
   public void SetMassText()
   {
      massText.text = "Mass: " + PlayerController.instance.mass.ToString();
   }
     

   public void togglePauseGame(bool paused) {
      gameIsPaused = paused;
   }
   
   //probably should have cutscene files or something, but here goes
   void RunIntroCutscene() {
      uiController.TogglePause(true);
      uiController.WriteThought("", "Lorem ipsum dolor sit amet,\n"
         + "consectetur adipiscing\n"
         + "elit. Integer interdum varius\n"
         + "pretium.\n"
         + "Aliquam erat volutpat.\n"
         + "Vivamus vitae convallis sapien,\n"
         + "Wednesday is lit\n"
         + "Nunc eget erat tempor,\n"
         + "aliquet justo vel, auctor libero.\n"
         + "Morbi mattis lacus felis, non iaculis\n"
         + "lorem dictum sed. Morbi mi nisi, placerat at\n"
         + "commodo ac, varius et odio. Nulla a magna odio.\n"
         + "Nam in justo nec nulla bibendum auctor at ac sapien.", GameUIController.OUR_TEXT_COLOR, true);
      uiController.WriteThought("", "Hello?", GameUIController.OUR_TEXT_COLOR, false);
      uiController.WriteThought("", "World?", GameUIController.OUR_TEXT_COLOR, false);
      uiController.TogglePause(false);
      uiController.FadeIn("player");
      uiController.Sleep(3.0f);
      uiController.ShowPopup("AsteroidsInstructions");
      uiController.FadeIn("background");
      uiController.FadeIn("full");
      uiController.EnableObject("beam");
      uiController.EnableObject("center_turret");
   }
   
   public void ActivateGodmode() {
      godmode = true;
      PlayerController.instance.invokeGodMode();

      centerTurretControl.isInvincible = true;
   }

   public void ShopPopup(string type)
   {
      if (type == "AsteroidsInstructions")
      {
         uiController.TogglePause(true);
         uiController.ShowPopup("AsteroidsInstructions");
      }
      if (type == "ShopInstructions")
      {
         uiController.TogglePause(true);
         uiController.ShowPopup("ShopInstructions");
      }
   }
   
   //this is for the main menu on start
   public static void ReadStats() {
      if (!Stat_init) {
         if (File.Exists(@"score.txt")) {
            StreamReader sr = new StreamReader(@"score.txt");
            Stat_NumGamesPlayed = int.Parse(sr.ReadLine());
            Stat_HighScore = int.Parse(sr.ReadLine());
            Stat_EnemiesKilled = int.Parse(sr.ReadLine());
            sr.Close();
         }
         Stat_init = true;
      }
   }
   
   public void UpdateStats(int currentScore, int currentEnemiesKilled) {
      if (currentScore > Stat_HighScore) {
         Stat_HighScore = currentScore;
      }
      if (currentEnemiesKilled > Stat_EnemiesKilled) {
         Stat_EnemiesKilled = currentEnemiesKilled;
      }
      Stat_NumGamesPlayed++;
      
      //write it to the file
      StreamWriter sw = new StreamWriter(@"score.txt");
      sw.WriteLine(Stat_NumGamesPlayed);
      sw.WriteLine(Stat_HighScore);
      sw.WriteLine(Stat_EnemiesKilled);
      sw.Close();
   }
}
