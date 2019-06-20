using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    //Variables
    public bool gameHasEnded = false;
    public int coins, sceneBuildIndex;
    string level, savePath;

    public AsyncOperation asyncLoading;
    public GameObject fade, completeLevelUI, coinText, practiceModePrefab;
    public GameObject[] practiceMode;
    public bool completeLevelDone = true, addCoinsDone = true, loadNextLevel = true, checkNextLevel = true, 
                checkEsc = true, checkSpace = true, saveOnLevelComplete = true, pressedSpace, allowPracticeMode = true;
    public float fogDensity = 0.02f;
    public int currentCoins, spentCoins;
    public string[] levels;

    GameObject[] levelEdges;
    Animator fadeAnim;
    Music music;

    //Data to save
    public class Data
    {
        public int localDeaths = 0, currentCoins = 0, spentCoins = 0, gamemode = 0, currentOutfit = 0, trailMaterial = 0;
        public float colorScrollPosition = 0f, trailColorScrollPosition = 0f, worldScrollbarPosition = 1f, trailSize = 0.05f;
        public int[] levelCoins = new int[20], levelDeaths = new int[20], worldCoins = new int[2], worldDeaths = new int[2];
        public bool[] levelDone = new bool[20], colorUnlocked = new bool[30], trailColorUnlocked = new bool[30], colorUsed = new bool[30], trailColorUsed = new bool[30];
    }

    //Awake
    public void Awake()
    {
        //Get objects
        if (coinText == null) coinText = GameObject.Find("CoinText");
        level = SceneManager.GetActiveScene().name;
        practiceMode = GameObject.FindGameObjectsWithTag("PracticeMode");
        levelEdges = GameObject.FindGameObjectsWithTag("LevelEdge");
        if (GameObject.Find("Music") != null) music = GameObject.Find("Music").GetComponent<Music>();
        if (SceneManager.GetActiveScene().buildIndex > 1) sceneBuildIndex = SceneManager.GetActiveScene().buildIndex - 2;
        else sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        savePath = Application.persistentDataPath + "/save.json";

        Camera.main.GetComponent<FollowPlayer>().Awake(); //Run camera awake on new scene

        fade.SetActive(true); //Activate fade
        fadeAnim = fade.GetComponent<Animator>();

        RenderSettings.fogDensity = fogDensity; //Set fog density here instead of manually going into every scene

        //If savefile doesn't exist
        if (!File.Exists(savePath) || (File.Exists(savePath) && new FileInfo(savePath).Length == 0)) //Fill save file if it doesn't exist or is empty
        {
            Debug.Log("Savefile does not exist");
            Data data = new Data();
            string json = JsonUtility.ToJson(data); //Convert data to json
            File.WriteAllText(savePath, json); //Write data to file
            Debug.Log("Created new savefile at '" + Application.persistentDataPath + "/save.json'");
            Restart(false);
        }

        //Game modes
        GameObject player = GameObject.Find("Player");
    }

    //Start
    void Start()
    {
        //Activate or disable practice mode
        PracticeMode();
    }

    //Enabled or disable practice mode
    public void PracticeMode()
    {
        if (allowPracticeMode)
        {
            int gamemode = LoadJson().gamemode;
            Debug.Log("Gamemode: " + gamemode);

            if (gamemode == 0)
                DisablePracticeMode();

            else if (gamemode == 1)
            {
                NewPracticeMode(false);

                practiceMode[0].GetComponent<PracticeMode>().enabled = true; //Activate practice mode script
                practiceMode[0].GetComponent<PracticeMode>().NewScene(levelEdges);
            }
        } else
        {
            DisablePracticeMode();
        }

        void DisablePracticeMode()
        {
            foreach (GameObject instance in practiceMode)
            {
                Destroy(instance);
            }
        }
    }

    //Create a new practice mode gameobject
    public void NewPracticeMode(bool find = true)
    {
        int len = find ? practiceMode.Length : GameObject.FindGameObjectsWithTag("PracticeMode").Length;

        if (len == 0)
        {
            practiceMode = new GameObject[1];
            practiceMode[0] = Instantiate(practiceModePrefab, Vector3.zero, Quaternion.identity) as GameObject;
            Debug.Log("Creating new practice mode");
        }
    }

    //Update   
    void Update()
    {
        //Restart level
        if (Input.GetButtonDown("Restart") && !gameHasEnded)
        {
            SpaceInput();
        }
        //Go to next level
        if (Input.GetButtonDown("NextLevel") && completeLevelDone && addCoinsDone && checkNextLevel)
        {
            EnterInput();
        }
        //Pause menu and exiting level
        if (Input.GetButtonDown("Cancel"))
        {
            EscapeInput();
        }

        //Take screenshot
        if (Input.GetButtonDown("Screenshot"))
        {
            Screenshot.TakeScreenshot_Static(Screen.width, Screen.height);
        }
    }
    //When exiting application
    void OnApplicationQuit()
    {
        //Remove local deaths
        SaveJson(0, -1, -1, 0, LoadJson().levelDone[0], -1, -1, -1, -1, -1);
        PlayerPrefs.DeleteKey("WorldLevelSelect"); //Remove automatic transition to previous world 
    }

    //Save to json
    public void SaveJson(int level, int coins, int deaths, int localDeaths, bool done, int gamemode, int spentCoins, int currentCoins, int worldCoins, int worldDeaths)
    {
        //Setup
        Data data = new Data(); //Data to save
        data = LoadJson(); //Load saved data

        //Overwrite data with custom stuff
        if (coins != -1) data.levelCoins[level] = coins; //Save coins to selected level
        if (worldCoins != -1) data.worldCoins[level] = worldCoins; //Save coins to selected level
        if (deaths != -1) data.levelDeaths[level] = deaths; //Save deaths to selected level
        if (worldDeaths != -1) data.worldDeaths[level] = worldDeaths; //Save coins to selected level
        data.levelDone[level] = done; //Save coins to selected level
        if (localDeaths != -1) data.localDeaths = localDeaths; //Save local deaths
        if (spentCoins != -1) data.spentCoins = spentCoins; //Save local deaths
        if (currentCoins != -1) data.currentCoins = currentCoins; //Save local deaths
        if (gamemode != -1) data.gamemode = gamemode; //Save local deaths

        //Save
        string json = JsonUtility.ToJson(data); //Convert data to json
        File.WriteAllText(Application.persistentDataPath + "/save.json", json); //Write data to file
    }
    //Save to json (outfits)
    public void SaveJsonOutfits(int outfit, int currentOutfit, bool unlocked, bool used)
    {
        //Setup
        Data data = new Data(); //Data to save
        data = LoadJson(); //Load saved data

        //Overwrite data with custom stuff
        if (currentOutfit != -1) data.currentOutfit = currentOutfit; //Save current outit
        data.colorUnlocked[outfit] = unlocked; //Set selected outfit as unlocked or locked
        data.colorUsed[outfit] = used; //Set trail color as unlocked or locked

        //Save
        string json = JsonUtility.ToJson(data); //Convert data to json
        File.WriteAllText(Application.persistentDataPath + "/save.json", json); //Write data to file
    }
    //Save to json (player trail)
    public void SaveJsonTrail(int index, int material, float size, bool unlocked, bool used)
    {
        //Setup
        Data data = new Data(); //Data to save
        data = LoadJson(); //Load saved data

        //Overwrite data with custom stuff
        if (material != -1) data.trailMaterial = material; //Save trail material (color)
        if (size != -1) data.trailSize = size; //Save trail size
        data.trailColorUnlocked[index] = unlocked; //Set trail color as unlocked or locked
        data.trailColorUsed[index] = used; //Set trail color as used or not used

        //Save
        string json = JsonUtility.ToJson(data); //Convert data to json
        File.WriteAllText(Application.persistentDataPath + "/save.json", json); //Write data to file
    }
    //Save to json (scrollbar positions)
    public void SaveJsonScrollbar(float colorPosition, float trailColorPosition, float worldPosition)
    {
        //Setup
        Data data = new Data(); //Data to save
        data = LoadJson(); //Load saved data

        //Overwrite data with custom stuff
        if (colorPosition != -1) data.colorScrollPosition = colorPosition; //Save player color scrollbar position
        if (trailColorPosition != -1) data.trailColorScrollPosition = trailColorPosition; //Save trail color scrollbar position
        if (worldPosition != -1) data.worldScrollbarPosition = worldPosition; //Save world scrollbar position

        //Save
        string json = JsonUtility.ToJson(data); //Convert data to json
        File.WriteAllText(Application.persistentDataPath + "/save.json", json); //Write data to file
    }
    //Unlock outfit (accesable from button)
    public void UnlockOutfit(int outfit)
    {
        SaveJsonOutfits(outfit, -1, true, LoadJson().colorUsed[outfit]);
    }

    //Load saved data
    public Data LoadJson()
    {
        if (File.Exists(Application.persistentDataPath + "/save.json")) //If save file exists
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/save.json"); //Read from file
            return JsonUtility.FromJson<Data>(json); //Convert json to data and return it
        } else if (!File.Exists(Application.persistentDataPath + "/save.json") || (File.Exists(Application.persistentDataPath + "/save.json") 
                    && new FileInfo(Application.persistentDataPath + "/save.json").Length == 0))
        {
            return new Data();
        }
        return new Data();
    }

    //Delete saved data
    public void DeleteJson()
    {
        File.Delete(savePath); //Delete save file

        //Check if deletion was succesfull
        if (!File.Exists(savePath)) Debug.Log("Save file has been succesfully deleted"); 
        else Debug.Log("Save file could not be deleted");

        //Remove temporary PlayerPrefs
        PlayerPrefs.DeleteKey("WorldLevelSelect"); //Delete what world to load when level is complete
        PlayerPrefs.DeleteKey("LevelToUnlock"); //Delete what level to unlock
    }

    //Complete Level function
    public void CompleteLevel()
    {
        Debug.Log("Completed Level. Coins: " + coinText.GetComponent<CoinText>().coins);
        if (!gameHasEnded)
        {
            //Start Complete Level animation
            completeLevelUI.SetActive(true);

            //Start loading current level and next level
            if (loadNextLevel) StartCoroutine(BeginAsyncLoading("LevelSelect", -1));

            //Update save variables
            coins = coinText.GetComponent<CoinText>().coins;

            //Save
            if (coins >= LoadJson().levelCoins[sceneBuildIndex] && saveOnLevelComplete) //Only save if coins are higher than ones stored or equal
            {
                SaveJson(sceneBuildIndex, coins, -1, -1, true, -1, -1, -1, -1, -1);
            }
        }
    }

    //End game function
    public void EndGame()
    {
        if (!gameHasEnded && !completeLevelUI.activeSelf) //Only execute once and don't die if reached end trigger
        {
            gameHasEnded = true;

            if (LoadJson().gamemode != 1)
            {
                StartCoroutine(BeginAsyncLoading("", sceneBuildIndex + 2)); //Start loading selected scene async

                //Increase total deaths by 1
                SaveJson(sceneBuildIndex, -1, LoadJson().levelDeaths[sceneBuildIndex] + 1, LoadJson().localDeaths + 1, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1, -1, -1); //Save

                //Activate FadeOut Trigger
                fade.GetComponent<Animator>().enabled = true;
                fade.GetComponent<Animator>().SetTrigger("FadeOut");
            }
            else
            {
                //Activate FadeOut Trigger
                fade.GetComponent<Animator>().enabled = true;
                fade.GetComponent<Animator>().SetTrigger("FadeOutPractice");
            }
        }
    }

    //Save current coins
    public void CurrentCoins()
    {
        currentCoins = 0;
        //Save coins to current coins
        for (int i = 0; i < levels.Length; i++)
        {
            currentCoins += LoadJson().levelCoins[i];
            SaveJson(0, -1, -1, -1, LoadJson().levelDone[0], -1, -1, currentCoins - LoadJson().spentCoins, -1, -1);
        }
    }

    //Load selected scene ahead of time
    public IEnumerator BeginAsyncLoading(string scene, int sceneIndex)
    {   
        //Load scene by string
        if (scene != "") asyncLoading = SceneManager.LoadSceneAsync(scene);

        //Load scene by build index
        if (sceneIndex != -1) asyncLoading = SceneManager.LoadSceneAsync(sceneIndex);

        asyncLoading.allowSceneActivation = false;
        
        yield return asyncLoading;
    }

    //Load selected scene
    public void AsyncLoading()
    {
        GC.Collect(); //Clean garbage

        //If async loading is not done
        if (asyncLoading != null)
        {
            //Set to switch to async loaded scene
            asyncLoading.allowSceneActivation = true;
        }
        else
        {
            Debug.Log("Cannot load scene async, loading normally instead");
            LoadLevel(SceneManager.GetActiveScene().name);
        }
    }

    //Load selected level (not async)
    public void LoadLevel(string Level)
    {
        //Remove local deaths
        SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1, -1, -1); //Save

        GC.Collect(); //Clean garbage

        //Load selected scene
        SceneManager.LoadScene(Level);
    }

    //Normal Restart
    public void Restart(bool removeLocalDeaths)
    {
        GC.Collect(); //Clean garbage

        if (removeLocalDeaths) SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1, -1, -1); //Remove local deaths

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Restart
    }

    //Space
    public void SpaceInput()
    {
        if (!checkSpace) return;

        int gamemode = LoadJson().gamemode;

        if (gamemode == 1 && allowPracticeMode)
        {
            practiceMode[0].GetComponent<PracticeMode>().LoadCheckpoint();
        }
        if (!completeLevelUI.activeSelf && !completeLevelDone && !GameObject.Find("CustomizationCanvas") && gamemode != 1) //If level is not complete (quick restarting)
        {
            //SaveJson(sceneBuildIndex, -1, LoadJson().levelDeaths[sceneBuildIndex] + 1, LoadJson().localDeaths + 1, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Save
            fadeAnim.SetTrigger("FadeOutShort");
        }
        if (completeLevelUI.activeSelf && completeLevelDone && addCoinsDone && checkSpace) //If level is completed, complete level animation is done and all bonus coins are added
        {
            Restart(true); //Restart and remove local deaths
        }
        if (level == "Customization" && GameObject.Find("CustomizationCanvas")) //If on color select on customization scene
        {
            GetComponent<Customization>().TestCustomization();
        }
    }

    //Enter
    public void EnterInput()
    {
        SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1, -1, -1); //Remove local deaths
        //Set what world to transition to and level to unlock
        for (int i = 0; i < 10; i++)
        {
            if (level == music.world1[i]) PlayerPrefs.SetInt("WorldLevelSelect", 1); //If on world 1, transition to world 1
            if (level == music.world2[i]) PlayerPrefs.SetInt("WorldLevelSelect", 2); //If on world 2, transition to world 2
            if (level == "Level10")
            {
                PlayerPrefs.DeleteKey("WorldLevelSelect");  //If on Level10, transition to worlds
                PlayerPrefs.SetInt("WorldToUnlock", 1); //Set to unlock world 2
                //PlayerPrefs.SetString("ObjectToSelectOnStart", "World2"); //Set world 2 as selected on start
                SaveJsonScrollbar(-1f, -1f, 0f); //Save scrollbar position so world 2 is visible
            }
            PlayerPrefs.SetInt("LevelToUnlock", sceneBuildIndex + 1); //Save level which will be unlocked (next level)
        }
        music.playLevelMusicInMenu = true;
        AsyncLoading();
    }

    //Escape
    public void EscapeInput()
    {
        if (level == "LevelSelect") //If on level select
        {
            GameObject viewport = GameObject.Find("WorldListViewport");
            Animator anim = viewport.GetComponent<Animator>();
            if (!anim.GetBool("WorldLevelsTransition")) //If not transitioning between levels and worlds
            {
                if (anim.GetBool("OnLevels"))
                {
                    viewport.GetComponent<UIAnimation>().LevelsToWorld(); //Transition to worlds
                    PlayerPrefs.DeleteKey("WorldLevelSelect");
                    music.playLevelMusicInMenu = false;
                }
                else
                {
                    music.playLevelMusicInMenu = false;
                    PlayerPrefs.DeleteKey("WorldLevelSelect");
                    LoadLevel("TitleScreen");
                }
            }
        }
        if (completeLevelUI != null && completeLevelDone && addCoinsDone && checkEsc) //If completed level
        {
            PlayerPrefs.DeleteKey("WorldLevelSelect");
            if (level == "Level10")
            {
                PlayerPrefs.SetInt("WorldToUnlock", 1); //Set to unlock world 2
                SaveJsonScrollbar(-1f, -1f, 0f); //Save scrollbar position so world 2 is visible
            }
            PlayerPrefs.SetInt("LevelToUnlock", sceneBuildIndex + 1); //Save level which will be unlocked (next level)
            music.playLevelMusicInMenu = false;
            LoadLevel("Customization");
        }
    }

}