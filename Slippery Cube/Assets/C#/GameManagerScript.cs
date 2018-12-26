using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    //Variables
    bool gameHasEnded = false;
    int coins, sceneBuildIndex;
    string level, savePath;

    GameObject coinText;
    public GameObject fade, completeLevelUI;
    public bool completeLevelDone, addCoinsDone = false;
    public int currentCoins, spentCoins;
    public string[] levels;

    //Data to save
    public class Data
    {
        public int localDeaths = 0, currentCoins = 0, spentCoins = 0, gamemode = 0;
        public int[] levelCoins = new int[12], levelDeaths = new int[12];
        public bool[] levelDone = new bool[12];
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

        fade.SetActive(true); //Activate fade

        //If savefile doesn't exist
        if (!File.Exists(savePath) || (File.Exists(savePath) && new FileInfo(savePath).Length == 0)) //Fill save file if it doesn't exist or is empty
        {
            Debug.Log("Savefile does not exist");
            Data data = new Data();
            string json = JsonUtility.ToJson(data); //Convert data to json
            File.WriteAllText(savePath, json); //Write data to file
            Debug.Log("Created new savefile");
            Restart();
        }
        //Game modes
        GameObject player = GameObject.Find("Player");
        if (LoadJson().gamemode == 0)
        {
            if (player != null) player.GetComponent<PracticeMode>().enabled = true; //Activate practice mode script
        }
        if (LoadJson().gamemode == 1)
        {
            if (player != null) player.GetComponent<PracticeMode>().enabled = false; //Disable practice mode script
        }

        CurrentCoins();
    }

    //Update   
    void Update()
    {
        //Debug.Log("Screen width: " + Screen.width);
        //Debug.Log("Screen height: " + Screen.height);
        //Restart level
        if (Input.GetButton("Restart") && !gameHasEnded)
        {
            if (!completeLevelUI.activeSelf && !completeLevelDone) //If level is not complete
            {
                SaveJson(sceneBuildIndex, -1, LoadJson().levelDeaths[sceneBuildIndex] + 1, LoadJson().localDeaths + 1, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Save
                Restart();
            }
            if (completeLevelUI.activeSelf && completeLevelDone && addCoinsDone) //If level is completed
            {
                //Remove local deaths
                SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Save
                Restart();
            }
        }
        //Go to next level
        if (Input.GetButton("NextLevel") && completeLevelDone && addCoinsDone)
        {
            LoadNextLevel();
        }
        //Pause menu and exiting level
        if (Input.GetButton("Cancel"))
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
        string json = File.ReadAllText(savePath); //Read from file
        Data loadedData = JsonUtility.FromJson<Data>(json); //Convert json to data
        return loadedData;
    }

    //Delete saved data
    public void DeleteJson()
    {
        File.Delete(savePath);
        //If delete was succesfull
        if (!File.Exists(savePath)) Debug.Log("Save file has been succesfully deleted"); 
        else Debug.Log("Save file could not be deleted");
        Restart();
    }

    //Complete Level function
    public void CompleteLevel()
    {
        if (!gameHasEnded)
        {
            //Start Complete Level animation
            completeLevelUI.SetActive(true);

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

    //Restart function
    void Restart()
    {
        //Load current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Load next level
    void LoadNextLevel()
    {
        //Remove local deaths
        SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Save

        //Load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Load selected level
    void LoadLevel(string Level)
    {
        //Remove local deaths
        if (sceneBuildIndex < 0) sceneBuildIndex += 2;
        SaveJson(sceneBuildIndex, -1, -1, 0, LoadJson().levelDone[sceneBuildIndex], -1, -1, -1); //Save

        //Load selected scene (probably menu)
        SceneManager.LoadScene(Level);
    }
}