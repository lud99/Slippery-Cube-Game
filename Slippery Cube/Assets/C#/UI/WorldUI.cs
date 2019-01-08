using UnityEngine;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour {

    //Variables
    public GameObject deathText, coinText, lockPar;
    //public GameObject[] levelObjects;
    GameManagerScript gMScript;

    public string worldCoins;
    //public string[] levels;
    public int world;
    public bool lockWorld = true;

    int coins, deaths;
    //bool toggle = true;

    //Start
    void Start()
    {
        //Remove 1 from world so the value can be the world in the inspector but starts at 0 for arrays
        world--;

        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get GameManager

        //Lock world if it's not unlocked
        if (lockWorld && !gMScript.LoadJson().levelDone[world * 10 - 1])
        {
            lockPar.SetActive(true);
        }

        //Get stored coins from world and display them
        for (int i = world * 10; i < 10 + (world * 10); i++)
        {
            coins += gMScript.LoadJson().levelCoins[i]; //Get coins
            deaths += gMScript.LoadJson().levelDeaths[i]; //Get deaths
        }

        //Update text
        coinText.GetComponent<Text>().text = coins.ToString() + " / " + worldCoins;
        deathText.GetComponent<Text>().text = deaths.ToString();

        //LevelToggle(); //Update levels on start
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
