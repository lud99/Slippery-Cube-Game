using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

    //Variables
    public GameObject levelText, coinText, deathText, lockPar;
    GameManagerScript gMScript;
    LoadScene loadScene;

    public int level, coins, deaths;
    public string[] levelNames, levelCoins, levelsToLoad;
    public bool lockLevel = true;

    //Start
    public void Start()
    {
        //Remove 1 from level since array goes from 0 to 19 instead of 1 to 20. Because level 1 should get data from first array entry, 1 is subtracted
        level--;

        //Get GameManager
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        loadScene = GetComponent<LoadScene>();

        //Lock level if it's not unlocked
        if (lockLevel && !gMScript.LoadJson().levelDone[level - 1]) //Unlock if previos level is done, not current (which is why 1 is subtracted)
        {
            lockPar.SetActive(true);
        }

        //Get data for selected level
        deaths = gMScript.LoadJson().levelDeaths[level]; //Deaths
        coins = gMScript.LoadJson().levelCoins[level]; //Coins

        //Update UI
        levelText.GetComponent<Text>().text = levelNames[level]; //Display level name
        coinText.GetComponent<Text>().text = coins.ToString() + " / " + levelCoins[level].ToString(); //Display collected level coins out of coins in level
        deathText.GetComponent<Text>().text = deaths.ToString(); //Display level deaths
    }

    //Begin loading selected level async
    public void LoadLevel()
    {
        //If no levels is currently loading
        if (gMScript.asyncLoading == null && !lockPar.activeSelf)
        {
            StartCoroutine(gMScript.BeginAsyncLoading(levelsToLoad[level], -1));
            GameObject.Find("Fade").GetComponent<Animator>().SetTrigger("FadeOutShortLoadScene");
        }
    }
}
