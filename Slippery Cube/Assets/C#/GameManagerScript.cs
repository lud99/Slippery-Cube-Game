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
    public GameObject fade, completeLevelUI, coinText;
    public bool completeLevelDone = true, addCoinsDone = true, loadNextLevel = true;
    public int currentCoins, spentCoins;
    public string[] levels;

    //Data to save
    public class Data
    {
        public int localDeaths = 0, currentCoins = 0, spentCoins = 0, gamemode = 0, currentOutfit = 0;
        public int[] levelCoins = new int[20], levelDeaths = new int[20];
        public bool[] levelDone = new bool[20];
    }

    //Activate Fade
    void Awake()
    {
        //Get objects
        if (coinText == null) coinText = GameObject.Find("CoinText");
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
            AsyncLoading();
            SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Remove local deaths
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
    //Save to json (outfits
    public void SaveJsonOutfits(int level, int currentOutfit)
    {
        //Setup
        Data data = new Data(); //Data to save
        data = LoadJson(); //Load saved data

        //Overwrite data with custom stuff
        if (currentOutfit != -1) data.currentOutfit = currentOutfit; //Save local deaths

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
        //If delete was succesfull
        if (!File.Exists(savePath)) Debug.Log("Save file has been succesfully deleted"); 
        else Debug.Log("Save file could not be deleted");
        //Restart(false); //Restart
    }

    //Complete Level function
    public void CompleteLevel()
    {
        if (!gameHasEnded)
        {
            //Start Complete Level animation
            completeLevelUI.SetActive(true);

            //Start loading current level and next level
            if (loadNextLevel) StartCoroutine(BeginAsyncLoading("", SceneManager.GetActiveScene().buildIndex + 1));

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

            StartCoroutine(BeginAsyncLoading(SceneManager.GetActiveScene().name, -1)); //Start loading current scene async

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
        if (asyncLoading.progress < 0.8)
        {
            Debug.Log("Cannot load scene");
        }

        //Set to switch to async loaded scene
        asyncLoading.allowSceneActivation = true;
    }

    //Load selected level (not async)
    public void LoadLevel(string Level)
    {
        //Remove local deaths
        SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Save

        GC.Collect(); //Clean garbage

        //Load selected scene
        SceneManager.LoadScene(Level);
    }

    //Normal Restart
    public void Restart(bool removeLocalDeaths)
    {
        GC.Collect(); //Clean garbage

        if (removeLocalDeaths) SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Remove local deaths

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Restart
    }
}