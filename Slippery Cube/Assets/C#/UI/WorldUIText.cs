using UnityEngine;
using UnityEngine.UI;

public class WorldUIText : MonoBehaviour {

    //Variables
    public GameObject deathText, coinText;
    public GameObject[] levelObjects;
    GameManagerScript gMScript;

    public string worldCoins;
    public string[] levels;

    int coins, deaths;
    bool toggle = true;

    //Start
    void Start()
    {
        gMScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); //Get GameManager
        //Get stored coins from world and display them
        for (int i = 0; i < gMScript.levels.Length; i++)
        {
            coins += gMScript.LoadJson().levelCoins[i]; //Get coins
            deaths += gMScript.LoadJson().levelDeaths[i]; //Get deaths
        }

        //Print
        coinText.GetComponent<Text>().text = coins.ToString() + " / " + worldCoins;
        deathText.GetComponent<Text>().text = deaths.ToString();

        LevelToggle(); //Update levels on start
    }

    //Activate levels when clicking on World button
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
    }
}
