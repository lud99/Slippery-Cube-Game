using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    //Variables
    public bool gameHasEnded = false;
    int coins, sceneBuildIndex;
    string level, savePath;

    GameObject coinText;
    AsyncOperation asyncRestart, asyncLoadNextLevel, asyncLoadLevel;
    public GameObject fade, completeLevelUI;
    public bool completeLevelDone = true, addCoinsDone = true;
    public int currentCoins, spentCoins;
    public string[] levels;

    //Data to save
    public class Data
    {
        public int localDeaths = 0, currentCoins = 0, spentCoins = 0, gamemode = 0;
        public int[] levelCoins = new int[20], levelDeaths = new int[20];
        public bool[] levelDone = new bool[20];
    }

    //Activate Fade
    void Awake()
    {
        //Get objects
        coinText = GameObject.Find("CoinText");
        level = SceneManager.GetActiveScene().name;
        if (SceneManager.GetActiveScene().buildIndex > 1) sceneBuildIndex = SceneManager.GetActiveScene().buildIndex - 2;
        else sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        savePath = Application.persistentDataPath + "/save.json";

        //Camera.main.GetComponent<Screenshot>().Awake(); //Run camera awake on new scene
        Camera.main.GetComponent<FollowPlayer>().Awake(); //Run camera awake on new scene

        fade.SetActive(true); //Activate fade

        //If savefile doesn't exist
        if (!File.Exists(savePath) || (File.Exists(savePath) && new FileInfo(savePath).Length == 0)) //Fill save file if it doesn't exist or is empty
        {
            Debug.Log("Savefile does not exist");
            Data data = new Data();
            string json = JsonUtility.ToJson(data); //Convert data to json
            File.WriteAllText(savePath, json); //Write data to file
            Debug.Log("Created new savefile");
            Restart(false);
        }

        //Game modes
        GameObject player = GameObject.Find("Player");
        if (LoadJson().gamemode == 0)
        {
            if (player != null) player.GetComponent<PracticeMode>().enabled = false; //Activate practice mode script
        }
        if (LoadJson().gamemode == 1)
        {
            if (player != null) player.GetComponent<PracticeMode>().enabled = true; //Disable practice mode script
        }
        CurrentCoins();
    }

    //Update   
    void Update()
    {
        //Restart level
        if (Input.GetButtonDown("Restart") && !gameHasEnded)
        {
            if (!completeLevelUI.activeSelf && !completeLevelDone) //If level is not complete (quick restarting)
            {
                SaveJson(sceneBuildIndex, -1, LoadJson().levelDeaths[sceneBuildIndex] + 1, LoadJson().localDeaths + 1, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Save
                fade.GetComponent<Animator>().SetTrigger("FadeOutShort");
            }
            if (completeLevelUI.activeSelf && completeLevelDone && addCoinsDone) //If level is completed, complete level animation is done and all bonus coins are added
            {
                Restart(true); //Restart and remove local deaths
            }
        }
        //Go to next level
        if (Input.GetButtonDown("NextLevel") && completeLevelDone && addCoinsDone)
        {
            LoadNextLevel();
        }
        //Pause menu and exiting level
        if (Input.GetButtonDown("Cancel"))
        {
            if (level == "LevelSelect") //If on level select
            {
                LoadLevel("TitleScreen");
            }
            if (completeLevelUI != null && completeLevelDone && addCoinsDone) //If completed level
            {
                LoadLevel("LevelSelect");
            }
        }

        //Take screenshot
        if (Input.GetButtonDown("Screenshot"))
        {
            Screenshot.TakeScreenshot_Static(Screen.width, Screen.height);
        }
    }

    //Save to json
    public void SaveJson(int level, int coins, int deaths, int localDeaths, bool done, int gamemode, int spentCoins, int currentCoins)
    {
        //Setup
        Data data = new Data(); //Data to save
        data = LoadJson(); //Load saved data

        //Overwrite data with custom stuff
        if (coins != -1) data.levelCoins[level] = coins; //Save coins to selected level
        if (deaths != -1) data.levelDeaths[level] = deaths; //Save deaths to selected level
        data.levelDone[level] = done; //Save coins to selected level
        if (localDeaths != -1) data.localDeaths = localDeaths; //Save local deaths
        if (spentCoins != -1) data.spentCoins = spentCoins; //Save local deaths
        if (currentCoins != -1) data.currentCoins = currentCoins; //Save local deaths
        if (gamemode != -1) data.gamemode = gamemode; //Save local deaths

        //Save
        string json = JsonUtility.ToJson(data); //Convert data to json
        File.WriteAllText(savePath, json); //Write data to file
    }

    //Load saved data
    public Data LoadJson()
    {
        if (File.Exists(savePath)) //If save file exists
        {
            string json = File.ReadAllText(savePath); //Read from file
            Data loadedData = JsonUtility.FromJson<Data>(json); //Convert json to data
            return loadedData;
        } else if (!File.Exists(savePath) || (File.Exists(savePath) && new FileInfo(savePath).Length == 0))
        {
            Data defaultData = new Data();
            return defaultData;
        }
        Data someData = new Data();
        return someData;
    }

    //Delete saved data
    public void DeleteJson()
    {
        File.Delete(savePath); //Delete save file
        PlayerPrefs.SetInt("MenuMusic", 0); //Reset menu music
        GameObject.Find("Music").GetComponent<Music>().SelectMenuMusic(0);
        //If delete was succesfull
        if (!File.Exists(savePath)) Debug.Log("Save file has been succesfully deleted"); 
        else Debug.Log("Save file could not be deleted");
        Restart(false); //Restart
    }

    //Complete Level function
    public void CompleteLevel()
    {
        if (!gameHasEnded)
        {
            //Start Complete Level animation
            completeLevelUI.SetActive(true);

            //Start loading current level and next level
            StartCoroutine(BeginAsyncLoadNextLevel());

            //Update save variables
            coins = coinText.GetComponent<CoinText>().coins;

            //Save
            if (coins > LoadJson().levelCoins[sceneBuildIndex]) //Only save if coins are higher than ones stored 
            {
                SaveJson(sceneBuildIndex, coins, -1, -1, true, -1, -1, -1);
            }

            //Mark level as completed if completed with 0 coins
            if (coins == 0 && LoadJson().levelDone[sceneBuildIndex] == false)
            {
                SaveJson(sceneBuildIndex, coins, -1, -1, true, -1, -1, -1);
            }
        }
    }

    //End game function
    public void EndGame()
    {
        if (!gameHasEnded && !completeLevelUI.activeSelf) //Only execute once and don't die if reached end trigger
        {
            gameHasEnded = true;

            //Increase total deaths by 1
            SaveJson(sceneBuildIndex, -1, LoadJson().levelDeaths[sceneBuildIndex] + 1, LoadJson().localDeaths + 1, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Save

            //Activate FadeOut Trigger
            fade.GetComponent<Animator>().SetTrigger("FadeOut");
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
            SaveJson(0, -1, -1, -1, LoadJson().levelDone[0], -1, -1, currentCoins - LoadJson().spentCoins);
        }
    }

    //Load current scene ahead of time
    public IEnumerator BeginAsyncRestart()
    {
        asyncRestart = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        asyncRestart.allowSceneActivation = false;
        yield return asyncRestart;
    }

    //Load next scene ahead of time
    public IEnumerator BeginAsyncLoadNextLevel()
    {
        asyncLoadNextLevel = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncLoadNextLevel.allowSceneActivation = false;
        yield return asyncLoadNextLevel;
    }

    //Load selected scene ahead of time
    public IEnumerator BeginAsyncLoadLevel(string scene)
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync(scene);
        asyncLoadLevel.allowSceneActivation = false;
        yield return asyncLoadLevel;
    }

    //Async Restart
    public void AsyncRestart()
    {
        System.GC.Collect();

        //If async loading is not done
        if (asyncRestart.progress < 0.8)
        {
            Debug.Log("Cannot restart");
            Restart(false); //Restart
        }
        //Set to switch to async loaded scene
        asyncRestart.allowSceneActivation = true;
    }

    //Normal Restart
    public void Restart(bool removeLocalDeaths)
    {
        System.GC.Collect();

        if (removeLocalDeaths) SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Remove local deaths

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Restart
    }

    //Async load level
    public void AsyncLoadLevel()
    {
        System.GC.Collect();

        //If async loading is not done
        if (asyncLoadLevel.progress < 0.8)
        {
            Debug.Log("Cannot load level");
        }
        //Set to switch to async loaded scene
        asyncLoadLevel.allowSceneActivation = true;
    }

    //Load next level
    public void LoadNextLevel()
    {
        //Remove local deaths
        SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Save

        System.GC.Collect();

        //If async loading is not done
        if (asyncLoadNextLevel.progress < 0.8)
        {
            Debug.Log("Cannot load next level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Load next level
        }
        //Set to switch to async loaded scene
        asyncLoadNextLevel.allowSceneActivation = true;
    }


    //Load selected level
    public void LoadLevel(string Level)
    {
        //Remove local deaths
        SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Save

        System.GC.Collect();

        //Load selected scene
        SceneManager.LoadScene(Level);
    }
}