using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISavableObject {

    private const string C_LEVEL = "level";
    private const int TITLE_SCREEN = -1;

    public GameObject[] levelPrefabs;
    private Level runningLevel;
    public GameObject ingameMenu;
    public GameObject titleScreen;
    public GameObject player;
    public GameObject carolineHead;

    public GameObject controller;
    
    [Header("Testing attributes")]
    public bool areYouTesting = false;
    public int startingLevel = -1;
    public GameObject testingLevel;

    public static GameManager instance;
    public int Level { get; private set; }
    public bool GamePaused { get; set; }
    public bool IngameMenuOpened { get; private set; }
    
    [Header("Fade Effect")]
    public Texture texture;
    public float timeToFade = 1f;
    public float deadFadeTime = 1f;
    
    private float alpha;
    private bool fadingIn, fadingOut, keepBlack, deadFadeIn, deadFadeOut;
    

    private void OnGUI()
    {
        if (fadingIn || deadFadeIn)
        {
            keepBlack = false;
            alpha -= Mathf.Clamp01(Time.deltaTime / (deadFadeIn ? deadFadeTime : timeToFade));
            GUI.color = new Color(0, 0, 0, alpha);
            GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), texture );
            if (alpha <= 0)
            {
                fadingIn = false;
                deadFadeIn = false;
            }
        }
        
        if (fadingOut || deadFadeOut)
        {
            alpha += Mathf.Clamp01(Time.deltaTime / (deadFadeOut ? deadFadeTime : timeToFade));
            GUI.color = new Color(0, 0, 0, alpha);
            GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), texture );
            if (alpha >= 1)
            {
                deadFadeOut = false;
                fadingOut = false;
                keepBlack = true;
            }
        }

        if (keepBlack)
        {
            GUI.color = new Color(0, 0, 0, 1);
            GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), texture );
        }
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);


        if (areYouTesting)
            return;
        
        if (startingLevel == TITLE_SCREEN)
        {
            GamePaused = true;
            Level = TITLE_SCREEN;
            player.SetActive(false);
            controller.SetActive(false);
        }
        else //start from a different level (mainly for testing)
        {
            Level = startingLevel;
            titleScreen.SetActive(false);
            player.SetActive(true);
            GameObject lvl = Instantiate(levelPrefabs[startingLevel]);
            lvl.SetActive(true);
            runningLevel = lvl.GetComponent<Level>();
            SetPlayerInLevel();
        }
        

    }

    private void Start()
    {
        if(!areYouTesting && startingLevel == TITLE_SCREEN)
            ManageInventory.instance.HideInventory();
    }

    private void SetPlayerInLevel()
    {
        if (runningLevel.playerSpawn)
        {
            player.transform.position = runningLevel.playerSpawn.position;
        }   
        else
            Debug.Log("Player position has not been set");
        
        
        if (runningLevel.camera)
        {
            //runningLevel.camera.target = carolineHead.GetComponent<SpriteRenderer>();
            runningLevel.camera.target = carolineHead.GetComponent<SkinnedMeshRenderer>();
        }
        else
            Debug.Log("Camera has not been set");
    }


    public GameObject GetPlayer()
    {
        return player;
    }

    public void PlayerDied()
    {
        StartCoroutine(LoadCheckPoint());
    }

    IEnumerator LoadCheckPoint()
    {
        player.SetActive(false);
        runningLevel.camera.Freezed = true;
        fadingOut = true;
        alpha = 0;
        yield return new WaitForSeconds(deadFadeTime);
        player.SetActive(true);
        runningLevel.camera.Freezed = false;
        //SceneManager.LoadScene(scene);
        //yield return null;
        //Debug.Log("loaded scene: " + scene);
        //yield return null;
        fadingIn = true;
        alpha = 1;
    }
    
    
    
    /// <summary>
    /// returns the number of the last saved level
    /// </summary>
    public int GetSavedLevel()
    {
        return PlayerPrefs.GetInt(C_LEVEL,TITLE_SCREEN);
    }

    /// <summary>
    /// pause the game
    /// </summary>
    public void PauseGame() { GamePaused = true; Time.timeScale = 0; }

    /// <summary>
    /// resumes the game if the inventory is not opened
    /// </summary>
    public void ResumeGame() {
        if (!ManageInventory.instance.OpenedInventory)
        {
            GamePaused = false;
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// opens the ingame menu pausing the game
    /// </summary>
    public void OpenIngameMenu()
    {
        IngameMenuOpened = true;
        ingameMenu.SetActive(true);
        PauseGame();
    }

    /// <summary>
    /// closes the ingame menu resuming the game
    /// </summary>
    public void CloseIngameMenu() {
        IngameMenuOpened = false;
        ingameMenu.SetActive(false);
        ResumeGame();
    }

    /// <summary>
    /// goes back to the title screen
    /// </summary>
    public void ReturnToTitle()
    {
        if (ManageInventory.instance.OpenedInventory)
            ManageInventory.instance.CloseInventory();
        CloseIngameMenu();
        Destroy(runningLevel.gameObject);
        Level = TITLE_SCREEN;
        titleScreen.SetActive(true);
        player.SetActive(false);

    }

    /// <summary>
    /// laods the next level
    /// </summary>
    public void NextLevel()
    {
        Debug.Log("level" + Level +" finished");
        if (Level + 1 == levelPrefabs.Length)
        {
            ManageInventory.instance.HideInventory();
            controller.SetActive(false);
            changeScene("GameEnd1", null);    
        }
        else
        {
            StartCoroutine(LoadNextLevel());
            
        }

        if (Level == levelPrefabs.Length - 1)
        {
            Debug.Log("LAST LEVEL REACHED");
            
            //player.SetActive(false); //deactive player from to be continued screen
        }
        
        
    }

   
    
    /// <summary>
    /// closes the application
    /// </summary>
    public void GameEnd()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }


    public void NewGame()
    {
        /*
        DestroyLevels();
        ManageInventory.instance.NewInventory();
        titleScreen.SetActive(false);
        
        player.SetActive(true);
        runningLevel = Instantiate(levelPrefabs[0]).GetComponent<Level>();
        runningLevel.gameObject.SetActive(true);
        SetPlayerInLevel();
        ResumeGame();
        */
        
        ManageInventory.instance.NewInventory();
        changeScene("GameIntro", null);
        //Initiate.Fade("GameIntro", Color.black, 1);
        titleScreen.SetActive(false);
        
    }

    public void changeScene(string scene, IEnumerator callback)
    {
        if(scene == "GameEnd1")
        {
            ManageInventory.instance.HideInventory();
            ManageInventory.instance.DisableInventory();
            controller.SetActive(false);
        }
        StartCoroutine(LoadScene(scene, callback));
    }

    public void StopPlayer()
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Animator>().SetFloat("VelocityX",0);
    }
    
    IEnumerator LoadScene(String scene, IEnumerator callback)
    {
        fadingOut = true;
        alpha = 0;
        yield return new WaitForSeconds(timeToFade);
        SceneManager.LoadScene(scene);
        yield return null;
        Debug.Log("loaded scene: " + scene);
        //yield return null;
        fadingIn = true;
        alpha = 1;
        if (callback!=null)
        {   
            StartCoroutine(callback);
        }
    }
    
    
    IEnumerator LoadNextLevel()
    {
        runningLevel.camera.Freezed = true;
        GamePaused = true;
        fadingOut = true;
        alpha = 0;
        yield return new WaitForSeconds(timeToFade);
        Destroy(runningLevel.gameObject);
        //runningLevel.gameObject.SetActive(false);
            
        runningLevel = Instantiate(levelPrefabs[Level + 1]).GetComponent<Level>();
        runningLevel.gameObject.transform.position = new Vector3();
        runningLevel.gameObject.SetActive(true);
        Level++;
        GamePaused = false;
        SetPlayerInLevel();
        ResumeGame();
        player.SetActive(true);
        GamePaused = false;
        fadingIn = true;
        alpha = 1;
        
    }
    public void IntroSceneFinished()
    {
        StartCoroutine(LoadScene("Clean_Scene",LoadFirstLevel()));
    }


    IEnumerator LoadFirstLevel()
    {
        yield return null;
        DestroyLevels();
        titleScreen.SetActive(false);
        player.SetActive(true);
        runningLevel = Instantiate(levelPrefabs[0]).GetComponent<Level>();
        runningLevel.gameObject.transform.position = new Vector3();
        runningLevel.gameObject.SetActive(true);
        SetPlayerInLevel();
        ResumeGame();
        ManageInventory.instance.ShowInventory();
        controller.SetActive(true);
        Level = 0;
        
    }
    

    public void DestroyLevels()
    {

        if (runningLevel != null)
        {
            Destroy(runningLevel.gameObject);
        }

        runningLevel = null;
        Level = -1;
    }
    /// <summary>
    /// save the level state
    /// </summary>
    public void Save()
    {//TODO: testing
        PlayerPrefs.SetInt(C_LEVEL, Level);

        if (areYouTesting)
            foreach (GameObject o in testingLevel.GetComponent<Level>().objectsToSave)
                foreach (ISavableObject s in o.GetComponents<ISavableObject>())
                {
                    s.Save();
                    Debug.Log("saving: " + o.name);
                }
        else
            foreach (GameObject o in runningLevel.objectsToSave)
                foreach (ISavableObject s in o.GetComponents<ISavableObject>())
                    s.Save();

        ManageInventory.instance.Save();
        PlayerPrefs.Save();

                    
        
    }

    /// <summary>
    /// loads the saved level
    /// </summary>
    public void Load()
    {
        DestroyLevels();
        titleScreen.SetActive(false);
        Level = PlayerPrefs.GetInt(C_LEVEL);
        runningLevel = Instantiate(levelPrefabs[Level]).GetComponent<Level>();
        if (areYouTesting)
            foreach (GameObject o in testingLevel.GetComponent<Level>().objectsToSave)
                foreach (ISavableObject s in o.GetComponents<ISavableObject>())
                    s.Load();
        else
            foreach (GameObject o in runningLevel.objectsToSave)
                foreach (ISavableObject s in o.GetComponents<ISavableObject>())
                    s.Load();


        ManageInventory.instance.NewInventory();
        ManageInventory.instance.Load();
        ResumeGame();

        
    }

}