using UnityEngine;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour {

    //Variables
    public GameObject deathText, coinText, lockPar, scrollbar;
    //public GameObject[] levelObjects;
    GameManagerScript gMScript;

    public string worldCoins;
    //public string[] levels;
    public int world;
    public bool lockWorld = true, unlockWorld;

    int coins, deaths;
    //bool toggle = true;

    //Start
    void Start()
    {
        //Remove 1 from world so the value can be the world in the inspector but starts at 0 for arrays
        world--;

        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get GameManager

        //Lock world if it's not unlocked
        if (lockWorld && gMScript.LoadJson().levelDone[world * 10 - 1] && PlayerPrefs.GetInt("WorldToUnlock") != world) //Unlock if previos level is done, not current (which is why 1 is subtracted)
        {
            lockPar.SetActive(false);
        }

        //Get stored coins from world and display them
        for (int i = world * 10; i < 10 + (world * 10); i++)
        {
            coins += gMScript.LoadJson().levelCoins[i]; //Get coins
            deaths += gMScript.LoadJson().levelDeaths[i]; //Get deaths
            gMScript.SaveJson(world, -1, -1, -1, gMScript.LoadJson().levelDone[world], -1, -1, -1, coins, deaths);
        }

        //Update text
        coinText.GetComponent<Text>().text = coins.ToString() + " / " + worldCoins;
        deathText.GetComponent<Text>().text = deaths.ToString();

        //Load scrollbar position
        scrollbar.GetComponent<Scrollbar>().value = gMScript.LoadJson().worldScrollbarPosition;

        //Play unlock animation on start
        PlayUnlockAnimation();

        //LevelToggle(); //Update levels on start
    }

    //Play Unlock Animation
    public void PlayUnlockAnimation()
    {
        if (PlayerPrefs.HasKey("WorldToUnlock") && unlockWorld && PlayerPrefs.GetInt("WorldToUnlock") == world && !gMScript.LoadJson().levelDone[world * 10 + 1])
        {
            lockPar.SetActive(true);
            GetComponent<UIAnimation>().UIUnlockLevel(true);
        }
        if (gMScript.LoadJson().levelDone[world * 10 + 1]) lockPar.SetActive(false);
    }

    ////Old level select

    /*/Activate levels when clicking on World button
    public void LevelToggle()
    {
        toggle = !toggle;

        //Get Level UI Objects
        for (int i = 0; i < levelObjects.Length; i++) //Loop through all level objects
        {
            if (toggle)
            {
                levelObjects[i].SetActive(true);
            }
            else { levelObjects[i].SetActive(false); }
        }
    }*/
}
